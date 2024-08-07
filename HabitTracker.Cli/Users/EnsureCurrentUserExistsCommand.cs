//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Users;

internal class EnsureCurrentUserExistsCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IUserService _userService;
    private readonly AddUserCommand _addUserCommand;

    public EnsureCurrentUserExistsCommand(IAnsiConsole console, IUserService userService, AddUserCommand addUserCommand)
    {
        _console = console;
        _userService = userService;
        _addUserCommand = addUserCommand;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var currentUser = await _userService.GetCurrentUser();
        if (currentUser.IsEmpty())
        {
            _console.MarkupLine("No default user account exists yet. Let's create your account first...");
            _console.WriteLine();

            var result = await _addUserCommand.ExecuteAsync(context, new());

            if (result is not Globals.S_OK)
            {
                _console.MarkupLine("[red]Error:[/] could not create first user account. App cannot continue.");
                return result;
            }
        }
        else
        {
            _console.MarkupLine($"Welcome back, {currentUser.GivenName}!");
        }

        return Globals.S_OK;
    }
}
