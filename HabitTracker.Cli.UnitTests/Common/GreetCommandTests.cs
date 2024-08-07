//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Common;

[TestClass]
public class GreetCommandTests
{
    [TestMethod]
    public void ExecuteAsync_WithoutRequestParameters_CollectsUserInput()
    {
        // arrange
        using var testConsole = TestConsole.Create("Foo");
        var command = new GreetCommand(testConsole);
        var context = CommandContextFactory.Create("greet", []);
        var request = new GreetCommand.Settings();

        // act
        var result = command.Execute(context, request);

        // assert
        result.Should().Be(0);
        testConsole.Output.Should().Contain("Hello, Foo!");
    }
}
