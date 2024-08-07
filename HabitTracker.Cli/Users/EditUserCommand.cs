//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Users;

internal class EditUserCommand : AsyncCommand<EditUserCommand.Request>
{
    internal class Request : CommandSettings
    {
        [CommandArgument(0, "<USER-ID>")]
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

    public EditUserCommand(IAnsiConsole console, IUserService userService)
    {
        _console = console;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, EditUserCommand.Request request)
    {
        var getResult = await _userService.GetUser(request.UserId);

        return await getResult.IfOrElse<Task<int>>(
            user => PerformUserEdit(request, user),
            errors => Task.FromResult(errors.Render(_console)));
    }

    private async Task<int> PerformUserEdit(EditUserCommand.Request request, User prevUser)
    {
        _console.MarkupLine($"Editing account data for [[{request.UserId}]]");

        if (string.IsNullOrEmpty(request.GivenName))
        {
            request.GivenName = _console.Ask<string>($"Change given (first) name [[{prevUser.GivenName}]]:");
        }

        if (string.IsNullOrEmpty(request.FamilyName))
        {
            request.FamilyName = _console.Ask<string>($"Change family (last) name [[{prevUser.FamilyName}]]:");
        }

        var result = await _userService.EditUser(request.UserId, request.GivenName, request.FamilyName);

        return result.IfOrElse(
            x => _console.RenderSuccess($"The user account for [{x.UserId}] was updated."),
            errors => errors.Render(_console));
    }
}
