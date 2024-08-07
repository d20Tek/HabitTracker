//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.UnitTests.Users;

[TestClass]
public class DeleteUserCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidUserId_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new DeleteUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("delete-user", ["test-user-3"]);
        var request = new DeleteUserCommand.Request { UserId = "test-user-3" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "test-user-3", "deleted");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithMissingUserId_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new DeleteUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("delete-user", ["test-user-bogus"]);
        var request = new DeleteUserCommand.Request { UserId = "test-user-bogus" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test-user-bogus", "not be found");
    }
}
