//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class TodaysHabitsCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public TodaysHabitsCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
    {
        _console = console;
        _habitService = habitService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _habitService.GetUserHabits(user.UserId);

        return result.IfOrElse(RenderHabits, errors => errors.Render(_console));
    }

    private int RenderHabits(Habit[] habits)
    {
        _console.MarkupLine("Today's Habit Progress:");

        var table = new Table()
            .Border(TableBorder.Rounded);

        table.AddColumns(
            new TableColumn("Id").Centered().Width(5),
            new TableColumn("Name").NoWrap().Width(50),
            new TableColumn("Status").Centered().Width(6),
            new TableColumn("").Width(6));

        if (habits.Any() is false)
        {
            table.AddRow("", "No habits exist... please add some.");
        }
        else
        {
            foreach (var habit in habits)
            {
                var completionCount = habit.GetCompletionCount(DateTimeOffset.Now);
                var habitIsCompleted = habit.IsCompleted(DateTimeOffset.Now);
                var doneText = habitIsCompleted ? "[[done]]" : "";

                table.AddRow(
                    habit.HabitId,
                    habit.Name.CapOverflow(50),
                    $"{completionCount}/{habit.TargetAttempts}",
                    $"[green]{doneText}[/]");
            }
        }

        _console.Write(table);
        return Globals.S_OK;
    }
}
