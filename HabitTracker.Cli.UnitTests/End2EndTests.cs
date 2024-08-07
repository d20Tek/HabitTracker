//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HabitTracker.Cli.UnitTests;

[TestClass]
public class End2EndTests
{
    [TestMethod]
    public void Run_GreetCommand()
    {
        // arrange
        System.String[] args = ["greet", "--name", "Test User"];
        AnsiConsole.Record();

        // act
        var result = ExecuteMainMethod(args);

        // assert
        result.Should().Be(0);

        var text = AnsiConsole.ExportText();
        text.Should().Contain("Hello, Test User!");
    }

    [ExcludeFromCodeCoverage]
    private int ExecuteMainMethod(System.String[] args)
    {
        var programType = typeof(Program);

        // Get the Main method
        var mainMethod = programType.GetMethod("<Main>$", BindingFlags.Static | BindingFlags.NonPublic);

        if (mainMethod != null)
        {
            Console.WriteLine("Program.Main method found!");

            var result = mainMethod.Invoke(null, new object[] { args });
            return result.As<int>();
        }
        else
        {
            throw new InvalidOperationException("Program Main method not found.");
        }
    }
}