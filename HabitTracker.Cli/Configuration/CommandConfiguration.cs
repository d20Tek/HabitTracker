//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Categories;
using HabitTracker.Cli.Common;
using HabitTracker.Cli.Habits;
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.Configuration;

internal static class CommandConfiguration
{
    public static IConfigurator ConfigureCommands(this IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None)
              .SetApplicationName("HabitTracker")
              .SetApplicationVersion("1.0")
              .ValidateExamples();

        config.AddCommand<GreetCommand>("greet")
              .WithDescription("Greeting command that displays welcome message.")
              .WithExample(["greet", "--name", "Tester"]);

        config.AddCommand<ExitCommand>("exit")
              .WithAlias("x")
              .WithAlias("quit")
              .WithAlias("q")
              .WithDescription("Exits the console application interactive mode.")
              .WithExample(["exit"])
              .WithExample(["x"]);

        return config.ConfigureUserCommands()
                     .ConfigureCategoryCommands()
                     .ConfigureHabitCommands();
    }
}
