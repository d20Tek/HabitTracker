//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Habits;

[TestClass]
public class MarkHabitCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new MarkHabitCompleteCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("mark-habit", ["3"]);
        var request = new MarkHabitCompleteCommand.Request { HabitId = "3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "3", "incremented");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithInvalidCount_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();
        var habitService = HabitServiceFactory.CreateMemoryRepo();

        var command = new MarkHabitCompleteCommand(AnsiConsole.Console, habitService, userService);
        var context = CommandContextFactory.Create("enable-category", ["3", "-c", "-1"]);
        var request = new MarkHabitCompleteCommand.Request { HabitId = "3", Date = DateTimeOffset.Now, Count = -1 };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "out of range");
    }
}
