//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;
using HabitTracker.Cli.Common;
using HabitTracker.Cli.Configuration;
using HabitTracker.Cli.Users;
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.Cli.UnitTests.Common;

[TestClass]
public class InteractiveLoopCommandTests
{
    [TestMethod]
    public async Task ExecuteAsync_WithGreetCommand_CallsDownstreamCommand()
    {
        // arrange
        AnsiConsole.Record();
        var app = CreateConfiguredCommandApp();
        using var testConsole = TestConsole.Create("greet -n \"Foo Tester\"", "exit");
        var ensureUser = GetEnsureCurrentUserExists(testConsole);

        var command = new InteractiveLoopCommand(app, testConsole, ensureUser);
        var context = CommandContextFactory.Create("default", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().Contain("greet -n \"Foo Tester\"");
        AnsiConsole.ExportText().Should().Contain("Hello, Foo Tester!");
    }

    [TestMethod]
    public async Task ExecuteAsync_WithComplexGreetCommand_CallsDownstreamCommand()
    {
        // arrange
        AnsiConsole.Record();
        var app = CreateConfiguredCommandApp();
        using var testConsole = TestConsole.Create("greet -n \"Foo Tester\" --verbose -c temp", "exit");
        var ensureUser = GetEnsureCurrentUserExists(testConsole);

        var command = new InteractiveLoopCommand(app, testConsole, ensureUser);
        var context = CommandContextFactory.Create("default", []);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().Contain("greet -n \"Foo Tester\" --verbose -c temp");
        AnsiConsole.ExportText().Should().Contain("Hello, Foo Tester!");
    }

    private static EnsureCurrentUserExistsCommand GetEnsureCurrentUserExists(TestConsole console)
    {
        var userService = UserServiceFactory.CreateMemoryRepo();
        var addUserCommand = new AddUserCommand(console, userService);

        return new EnsureCurrentUserExistsCommand(console, userService, addUserCommand);
    }

    private static CommandApp<InteractiveLoopCommand> CreateConfiguredCommandApp()
    {
        var services = new ServiceCollection()
                            .AddServices();

        var registrar = new DependencyInjectionTypeRegistrar(services);

        // Create the CommandApp with specified command type and type registrar.
        var app = new CommandApp<InteractiveLoopCommand>(registrar);
        services.AddSingleton<CommandApp<InteractiveLoopCommand>>(sp => app);

        // Configure any commands in the application.
        app.Configure(config => config.ConfigureCommands());

        return app;
    }
}
