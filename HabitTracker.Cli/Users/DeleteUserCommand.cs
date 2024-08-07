//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Users;

internal class DeleteUserCommand : AsyncCommand<DeleteUserCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<USER-ID>")]
        [Description("The user account identifier to delete.")]
        public string UserId { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly IUserService _userService;

    public DeleteUserCommand(IAnsiConsole console, IUserService userService)
    {
        _console = console;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DeleteUserCommand.Request request)
    {
        var getResult = await _userService.DeleteUser(request.UserId);

        return getResult.IfOrElse(
            x => _console.RenderSuccess($"User account for [{x.UserId}] was deleted."),
            errors => errors.Render(_console));
    }
}
