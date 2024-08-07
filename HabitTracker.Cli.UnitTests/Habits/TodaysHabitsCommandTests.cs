//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class TodaysHabitsCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithUnmarkedHabits_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new TodaysHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("today");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        var text = AnsiConsole.ExportText();
        text.Should().Contain("Status");
        text.Should().Contain("Habit1");
        text.Should().Contain("Habit2");
        text.Should().Contain("Habit3");
        text.Should().Contain("Habit4");
        text.Should().Contain("Habit5");
        text.Should().Contain("0/3");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithMarkedHabits_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        await habitService.MarkHabit("test-user-1", "1", DateTimeOffset.Now);
        await habitService.MarkHabit("test-user-1", "3", DateTimeOffset.Now);

        var command = new TodaysHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("today");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        var text = AnsiConsole.ExportText();
        text.Should().Contain("Status");
        text.Should().Contain("Habit1");
        text.Should().Contain("Habit2");
        text.Should().Contain("Habit3");
        text.Should().Contain("Habit4");
        text.Should().Contain("Habit5");
        text.Should().Contain("1/1");
        text.Should().Contain("[done]");
        text.Should().Contain("1/3");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithNoCategories_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateEmptyMemoryRepo();

        var command = new TodaysHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("today");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Status", "No habits exist");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateHabitServiceWithErrors();

        var command = new TodaysHabitsCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("today");

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
