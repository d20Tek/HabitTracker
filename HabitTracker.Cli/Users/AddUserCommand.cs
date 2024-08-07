//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Users;

internal class AddUserCommand : AsyncCommand<AddUserCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandOption("-i|--id")]
        [Description("The user account unique identifier.")]
        public string UserId { get; set; } = "";

        [CommandOption("-g|--given-name")]
        [Description("The user's given (or first) name.")]
        public string GivenName { get; set; } = "";

        [CommandOption("-f|--family-name")]
        [Description("The user's family (or last) name.")]
        public string FamilyName { get; set; } = "";
    }

    private readonly IAnsiConsole _console;
    private readonly IUserService _userService;

    public AddUserCommand(IAnsiConsole console, IUserService userService)
    {
        _console = console;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, AddUserCommand.Request request)
    {
        if (string.IsNullOrEmpty(request.UserId))
        {
            request.UserId = _console.Ask<string>("Enter unique user name for your account:");
        }

        if (string.IsNullOrEmpty(request.GivenName))
        {
            request.GivenName = _console.Ask<string>("Enter your given (first) name:");
        }

        if (string.IsNullOrEmpty(request.FamilyName))
        {
            request.FamilyName = _console.Ask<string>("Enter your family (last) name:");
        }

        var result = await _userService.AddUser(request.UserId, request.GivenName, request.FamilyName);

        return result.IfOrElse(
            x => _console.RenderSuccess($"New user account for [{x.UserId}] was created."),
            errors => errors.Render(_console));
    }
}
