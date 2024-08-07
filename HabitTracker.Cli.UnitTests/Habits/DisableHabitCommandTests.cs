//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class DisableHabitCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new DisableHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("disable-habit", ["3"]);
        var request = new DisableHabitCommand.Request { HabitId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "3", "disabled");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithDisabledHabit_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepoWithDisabledHabits();

        var command = new DisableHabitCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("disable-habit", ["3"]);
        var request = new DisableHabitCommand.Request { HabitId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "already disabled");
    }
}
