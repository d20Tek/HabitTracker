//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class AddHabitCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new AddHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("add-habit", ["-n", "New habit", "-c", "2", "-a", "1"]);
        var request = new AddHabitCommand.Request { Name = "New habit", CatId = "2", TargetAttempts = 1 };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "New habit", "created");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        using var testConsole = TestConsole.Create("HabitAdded", "2", "1");
        var command = new AddHabitCommand(testConsole, habitService, userService);
        var context = CommandContextFactory.Create("add-habit");
        var request = new AddHabitCommand.Request();

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().ContainAll("Success", "HabitAdded", "created");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateHabitServiceWithErrors();

        var command = new AddHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("add-habit", ["-n", "New habit", "-c", "2", "-a", "1"]);
        var request = new AddHabitCommand.Request { Name = "New habit", CatId = "2", TargetAttempts = 1 };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
