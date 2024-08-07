//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class MarkHabitCompleteCommand : AsyncCommand<MarkHabitCompleteCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<HABIT-ID>")]
        [Description("The id of the habit to mark as done.")]
        public string HabitId { get; set; } = "";

        [CommandOption("-c|--count")]
        [Description("The number of completion marks to apply to the habit.")]
        public int Count { get; set; } = 1;

        [CommandOption("-d|--date")]
        [Description("The date to mark the habit completion.")]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    }

    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public MarkHabitCompleteCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
    {
        _console = console;
        _habitService = habitService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _habitService.MarkHabit(user.UserId, request.HabitId, request.Date, request.Count);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The habit with id=[{result.Value.HabitId}] incremented its completion count."),
            errors => errors.Render(_console));
    }
}
