//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using HabitTracker.Cli.Common;
using HabitTracker.Cli.UnitTests.Fakes;

namespace HabitTracker.Cli.UnitTests.Common;

[TestClass]
public class ErrorPresenterTests
{
    [TestMethod]
    public void Render_WithSingleError_PrintsErrorMessage()
    {
        // arrange
        var testConsole = TestConsole.Create();
        Error[] errors = [ Error.Invalid("Test.Error", "test error.") ];

        // act
        var result = errors.Render(testConsole);

        // assert
        result.Should().Be(-1);
        testConsole.Output.Should().ContainAll("Error", "test error.");
    }

    [TestMethod]
    public void Render_WithMultipleErrors_PrintsErrorMessages()
    {
        // arrange
        var testConsole = TestConsole.Create();
        Error[] errors =
        [
            Error.Validation("Error.Test1", "test error 1."),
            Error.Validation("Error.Test2", "test error 2."),
            Error.Validation("Error.Test3", "test error 3."),
        ];

        // act
        var result = errors.Render(testConsole);

        // assert
        result.Should().Be(-1);
        testConsole.Output.Should().ContainAll(
            "Multiple error messages",
            "test error 1.",
            "test error 2.",
            "test error 3.");
    }
}
