//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.UnitTests.Users;

[TestClass]
public class AddUserCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithValidSettings_WritesSuccessMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new AddUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("add-user", ["-i", "test-user-new", "-g", "Foo", "-f", "Bar"]);
        var request = new AddUserCommand.Request { UserId = "test-user-new", GivenName = "Foo", FamilyName = "Bar" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "test-user-new");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithDuplicateUserId_WritesErrorMessage()
    {
        // arrange
        AnsiConsole.Record();
        var userService = UserServiceFactory.CreateMemoryRepo();

        var command = new AddUserCommand(AnsiConsole.Console, userService);
        var context = CommandContextFactory.Create("add-user", ["-i", "test-user-2", "-g", "Foo", "-f", "Bar"]);
        var request = new AddUserCommand.Request { UserId = "test-user-2", GivenName = "Foo", FamilyName = "Bar" };

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(-1);
        AnsiConsole.ExportText().Should().ContainAll("Error", "test-user-2", "already exists");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        var userService = UserServiceFactory.CreateMemoryRepo();

        using var testConsole = TestConsole.Create("test-user-new", "Foo", "Last");
        var command = new AddUserCommand(testConsole, userService);
        var context = CommandContextFactory.Create("add-user", []);
        var request = new AddUserCommand.Request();

        // act
        var result = await command.ExecuteAsync(context, request);

        // assert
        result.Should().Be(0);
        AnsiConsole.ExportText().Should().ContainAll("Success", "[test-user-new]");
    }
}
