//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class EnableHabitCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepoWithDisabledHabits();

        var command = new EnableHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("enable-habit", ["3"]);
        var request = new EnableHabitCommand.Request { HabitId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "3", "enabled");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithEnabledHabit_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new EnableHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("enable-category", ["3"]);
        var request = new EnableHabitCommand.Request { HabitId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "already enabled");
    }
}
