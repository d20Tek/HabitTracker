//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.UnitTests.Users;

[TestClass]
public class EditUserCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new EditUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("add-user", ["test-user-1", "-g", "Updated", "-f", "Name"]);
        var request = new EditUserCommand.Request { UserId = "test-user-1", GivenName = "Updated", FamilyName = "Name" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "test-user-1");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithMissingUserId_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new EditUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("add-user", ["test-user-1", "-g", "Updated", "-f", "Name"]);
        var request = new EditUserCommand.Request { UserId = "test-user-bogus", GivenName = "Updated", FamilyName = "Name" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test-user-bogus", "not be found");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithServiceException_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateUserServiceWithErrors();

        var command = new EditUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("add-user", ["test-user-1", "-g", "Updated", "-f", "Name"]);
        var request = new EditUserCommand.Request { UserId = "test-user-1", GivenName = "Updated", FamilyName = "Name" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test error message");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        var userService = UserServiceFactory.CreateMemoryRepo();

        using var testConsole = TestConsole.Create("Foo", "Last");
        var command = new EditUserCommand(testConsole, userService);
        var context = CommandContextFactory.Create("edit-user", ["test-user-2"]);
        var request = new EditUserCommand.Request { UserId = "test-user-2" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().ContainAll("Success", "[test-user-2]");
    }
}
