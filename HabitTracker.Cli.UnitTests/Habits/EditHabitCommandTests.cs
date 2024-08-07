//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class EditHabitCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new EditHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("edit-habit", ["2", "-n", "Updated habit", "-c", "2", "-a", "3"]);
        var request = new EditHabitCommand.Request { HabitId = "2", Name = "Updated habit", CatId = "2", TargetAttempts = 3 };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "Updated habit", "updated");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        using var testConsole = TestConsole.Create("HabitUpdated", "2", "1");
        var command = new EditHabitCommand(testConsole, habitService, userService);
        var context = CommandContextFactory.Create("edit-habit", ["2"]);
        var request = new EditHabitCommand.Request { HabitId = "2" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().ContainAll("Success", "HabitUpdated", "updated");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithMissingHabitId_WritesErrorMessage()
    {
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new EditHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("edit-habit", ["bogus-id", "-n", "New habit", "-c", "2", "-a", "1"]);
        var request = new EditHabitCommand.Request { HabitId = "bogus-id", Name = "New habit", CatId = "2", TargetAttempts = 1 };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "bogus-id", "not be found");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateHabitServiceWithErrors();

        var command = new EditHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("edit-habit", ["bogus-id", "-n", "New habit", "-c", "2", "-a", "1"]);
        var request = new EditHabitCommand.Request { HabitId = "bogus-id", Name = "New habit", CatId = "2", TargetAttempts = 1 };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
