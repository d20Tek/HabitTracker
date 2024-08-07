//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Users;

internal class SetCurrentUserCommand : AsyncCommand<SetCurrentUserCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<USER-ID>")]
        [Description("The user account identifier to set as current.")]
        public string UserId { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly IUserService _userService;

    public SetCurrentUserCommand(IAnsiConsole console, IUserService userService)
    {
        _console = console;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Request settings)
    {
        var result = await _userService.SetCurrentUserId(settings.UserId);

        return result.IfOrElse(
            x => _console.RenderSuccess($"Current user changed to user account [{x.UserId}]."),
            errors => errors.Render(_console));
    }
}
