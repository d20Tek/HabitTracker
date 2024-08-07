//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;
using System;

namespace HabitTracker.Cli.Habits;

internal class WeeklyHabitsCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public WeeklyHabitsCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
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
        var dateRange = GetDateRangeForWeek();
        _console.MarkupLine("Habits 7-Day History:");

        var table = new Table()
            .Border(TableBorder.Rounded);

        table.AddColumns(
            new TableColumn("Id").Centered().Width(5),
            new TableColumn("Name").NoWrap().Width(30),
            new TableColumn(dateRange[0].ToString("MM-dd")).Centered().Width(5),
            new TableColumn(dateRange[1].ToString("MM-dd")).Centered().Width(5),
            new TableColumn(dateRange[2].ToString("MM-dd")).Centered().Width(5),
            new TableColumn(dateRange[3].ToString("MM-dd")).Centered().Width(5),
            new TableColumn(dateRange[4].ToString("MM-dd")).Centered().Width(5),
            new TableColumn(dateRange[5].ToString("MM-dd")).Centered().Width(5),
            new TableColumn(dateRange[6].ToString("MM-dd")).Centered().Width(5));

        if (habits.Any() is false)
        {
            table.AddRow("", "No habits exist... please add some.");
        }
        else
        {
            foreach (var habit in habits)
            {
                var rowColumns = RenderHabitRow(habit, dateRange);
                table.AddRow(rowColumns);
            }
        }

        _console.Write(table);
        return Globals.S_OK;
    }

    private string[] RenderHabitRow(Habit habit, DateTimeOffset[] dateRange)
    {
        List<string> row = [habit.HabitId, habit.Name.CapOverflow(30)];

        foreach (var date in dateRange)
        {
            var completionCount = habit.GetCompletionCount(date);
            var habitIsCompleted = habit.IsCompleted(date);
            var status = habitIsCompleted ? "[green] X [/]" : $"{completionCount}/{habit.TargetAttempts}";

            row.Add(status);
        }

        return row.ToArray();
    }

    private DateTimeOffset[] GetDateRangeForWeek()
    {
        DateTimeOffset[] dates = new DateTimeOffset[7];
        DateTimeOffset today = DateTimeOffset.Now;

        for (int i = 0; i < dates.Length; i++)
        {
            dates[i] = today.AddDays(-6 + i);
        }

        return dates;
    }
}
