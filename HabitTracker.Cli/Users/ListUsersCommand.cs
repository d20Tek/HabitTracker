//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.Users;

internal class ListUsersCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IUserService _userService;

    public ListUsersCommand(IAnsiConsole console, IUserService userService)
    {
        _console = console;
        _userService = userService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var result = await _userService.GetUsers();

        return result.IfOrElse(RenderUsers, errors => errors.Render(_console));
    }

    private int RenderUsers(User[] users)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Width(60);

        table.AddColumn("Local user accounts");

        if (users.Any() is false)
        {
            table.AddRow("No user account have been set up... please add an account.");
        }
        else
        {
            foreach (var user in users)
            {
                table.AddRow($"[[{user.UserId}]] - {user.GivenName} {user.FamilyName}");
            }
        }

        _console.Write(table);
        return Globals.S_OK;
    }
}
