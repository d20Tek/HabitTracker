//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Habits;

internal class DisableHabitCommand : AsyncCommand<DisableHabitCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<HABIT-ID>")]
        [Description("The id of the habit to disable.")]
        public string HabitId { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly IHabitService _habitService;
    private readonly IUserService _userService;

    public DisableHabitCommand(IAnsiConsole console, IHabitService habitService, IUserService userService)
    {
        _console = console;
        _habitService = habitService;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request request)
    {
        var user = await _userService.GetCurrentUser();
        var result = await _habitService.DisableHabit(user.UserId, request.HabitId);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The habit with id=[{x.HabitId}] was disabled."),
            errors => errors.Render(_console));
    }
}
