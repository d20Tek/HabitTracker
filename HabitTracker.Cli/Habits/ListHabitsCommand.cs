//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class ListHabitsCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public ListHabitsCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
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
        _console.MarkupLine("List of My Habits");

        var table = new Table()
            .Border(TableBorder.Rounded);

        table.AddColumns(
            new TableColumn("Id").Centered().Width(5),
            new TableColumn("Name").NoWrap().Width(40),
            new TableColumn("CatId").Centered().Width(5),
            new TableColumn("Attempts").Centered().Width(8));

        if (habits.Any() is false)
        {
            table.AddRow("", "No habits exist... please add some.");
        }
        else
        {
            foreach (var habit in habits)
            {
                table.AddRow(habit.HabitId, habit.Name.CapOverflow(40), habit.CategoryId, $"{habit.TargetAttempts}");
            }
        }

        _console.Write(table);
        return Globals.S_OK;
    }
}
