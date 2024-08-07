//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.UnitTests.Users;

[TestClass]
public class SetCurrentUserCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new SetCurrentUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("set-current-user", ["test-user-3"]);
        var request = new SetCurrentUserCommand.Request { UserId = "test-user-3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "test-user-3", "Current user changed");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithInvalidId_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new SetCurrentUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("set-current-user", [""]);
        var request = new SetCurrentUserCommand.Request { UserId = "" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "user id is invalid");
    }
}
