//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Habits;

internal static class CommandConfigurator
{
    public static IConfigurator ConfigureHabitCommands(this IConfigurator config)
    {
        config.AddCommand<AddHabitCommand>("add-habit")
              .WithAlias("ah")
              .WithDescription("Adds a new habit for the current user.")
              .WithExample(["add-habit", "--name", "Habit 1", "--cat-id", "12", "--target-attempts", "1"])
              .WithExample(["ah", "-n", "Habit 1", "-c", "12", "-a", "1"]);

        config.AddCommand<EditHabitCommand>("edit-habit")
              .WithAlias("eh")
              .WithDescription("Edit the specified habit with new property values.")
              .WithExample(["edit-habit", "habit-id", "--name", "Habit Updated", "--cat-id", "15", "--target-attempts", "3"])
              .WithExample(["eh", "habit-id", "-n", "Habit Updated", "-c", "15", "-a", "3"]);

        config.AddCommand<ListHabitsCommand>("list-habits")
              .WithAlias("lh")
              .WithDescription("Displays the list of habits for the current user.")
              .WithExample(["list-habits"])
              .WithExample(["lh"]);

        config.AddCommand<DisableHabitCommand>("disable-habit")
              .WithAlias("dh")
              .WithDescription("Disables the specified habit so it no longer can be used.")
              .WithExample(["disable-habit", "habit-id"])
              .WithExample(["dh", "habit-id"]);

        config.AddCommand<EnableHabitCommand>("enable-habit")
              .WithAlias("enh")
              .WithDescription("Enables the specified habit so it can be used again.")
              .WithExample(["enable-habit", "habit-id"])
              .WithExample(["enh", "habit-id"]);

        config.AddCommand<MarkHabitCompleteCommand>("mark-habit")
              .WithAlias("mark")
              .WithAlias("mh")
              .WithAlias("complete")
              .WithDescription("Increments the specified habit completion counter by specified amount (defaults to 1).")
              .WithExample(["mark", "habit-id"])
              .WithExample(["mark-habit", "habit-id", "--date", "07/30/2024", "--count", "3"])
              .WithExample(["mh", "habit-id", "-d", "07/30/2024", "-c", "3"]);

        config.AddCommand<UnmarkHabitCompleteCommand>("unmark-habit")
              .WithAlias("unmark")
              .WithAlias("uh")
              .WithAlias("uncomplete")
              .WithDescription("Decrements the specified habit completion counter by specified amount (defaults to 1).")
              .WithExample(["mark", "habit-id"])
              .WithExample(["mark-habit", "habit-id", "--date", "07/30/2024", "--count", "3"])
              .WithExample(["mh", "habit-id", "-d", "07/30/2024", "-c", "3"]);

        config.AddCommand<TodaysHabitsCommand>("todays-habits")
              .WithAlias("today")
              .WithAlias("t")
              .WithDescription("Shows a table with the current status of today's habit statuses.")
              .WithExample(["today"])
              .WithExample(["todays-habits"])
              .WithExample(["t"]);

        config.AddCommand<WeeklyHabitsCommand>("weekly-habits")
              .WithAlias("week")
              .WithAlias("w")
              .WithDescription("Shows a table with the habit statuses for the last week.")
              .WithExample(["week"])
              .WithExample(["weekly-habits"])
              .WithExample(["w"]);

        return config;
    }
}
