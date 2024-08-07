//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.UnitTests.Users;

[TestClass]
public class ListCurrentUserCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithUsers_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new ListUsersCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("list-user", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        var text = AnsiConsole.ExportText();
        text.Should().Contain("user accounts");
        text.Should().Contain("test-user-1");
        text.Should().Contain("test-user-2");
        text.Should().Contain("test-user-3");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithNoUsers_WritesDifferentMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateEmptyMemoryRepo();

        var command = new ListUsersCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("list-user", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("user accounts", "No user account");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceErrors_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateUserServiceWithErrors();

        var command = new ListUsersCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("list-user", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }
}
