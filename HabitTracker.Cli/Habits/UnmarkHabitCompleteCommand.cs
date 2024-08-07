//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class UnmarkHabitCompleteCommand : AsyncCommand<UnmarkHabitCompleteCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<HABIT-ID>")]
        [Description("The id of the habit to remove done mark.")]
        public string HabitId { get; set; } = "";

        [CommandOption("-c|--count")]
        [Description("The number of completion marks to remove from the habit.")]
        public int Count { get; set; } = 1;

        [CommandOption("-d|--date")]
        [Description("The date to unmark the habit completion.")]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    }

    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public UnmarkHabitCompleteCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
    {
        _console = console;
        _habitService = habitService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _habitService.UnmarkHabit(user.UserId, request.HabitId, request.Date, request.Count);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The habit with id=[{result.Value.HabitId}] decremented its completion count."),
            errors => errors.Render(_console));
    }
}
