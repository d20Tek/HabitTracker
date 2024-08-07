//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.UnitTests.Users;

[TestClass]
public class EnsureCurrentUserExistsCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithExistingCurrentUser_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new EnsureCurrentUserExistsCommand(AnsiConsole.Console, userService, null!);
        var context = CommandContextFactory.Create("ensure-current-user", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().Contain("Welcome back, Foo");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithNoCurrentUser_AddsNewUserAndWritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateEmptyMemoryRepo();

        using var testConsole = TestConsole.Create("test-user-new", "Foo", "Last");
        var addCommand = new AddUserCommand(testConsole, userService);

        var command = new EnsureCurrentUserExistsCommand(AnsiConsole.Console, userService, addCommand);
        var context = CommandContextFactory.Create("ensure-current-user", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().Contain("No default user");
        testConsole.Output.Should().ContainAll("Success", "test-user-new");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithUserServiceERror_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateEmptyMemoryRepo();

        using var testConsole = TestConsole.Create("test-user-new", "Foo", "Last");
        var addCommand = new AddUserCommand(testConsole, UserServiceFactory.CreateUserServiceWithErrors());

        var command = new EnsureCurrentUserExistsCommand(AnsiConsole.Console, userService, addCommand);
        var context = CommandContextFactory.Create("ensure-current-user", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("could not create first user account");
    }
}
