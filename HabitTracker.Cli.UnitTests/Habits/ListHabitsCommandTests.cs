//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class ListHabitsCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithHabitsForUser_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new ListHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("list-habits");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        var text = AnsiConsole.ExportText();
        text.Should().Contain("Attempts");
        text.Should().Contain("Habit1");
        text.Should().Contain("Habit2");
        text.Should().Contain("Habit3");
        text.Should().Contain("Habit4");
        text.Should().Contain("Habit5");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithNoCategories_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateEmptyMemoryRepo();

        var command = new ListHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("list-habits");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Name", "No habits exist");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateHabitServiceWithErrors();

        var command = new ListHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("list-habits");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
