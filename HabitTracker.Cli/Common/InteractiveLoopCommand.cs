//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Users;

namespace HabitTracker.Cli.Common;

internal class InteractiveLoopCommand : AsyncCommand
{
    private readonly CommandApp<InteractiveLoopCommand> _app;
    private readonly IAnsiConsole _console;
    private readonly EnsureCurrentUserExistsCommand _ensureUserCommand;

    public InteractiveLoopCommand(
        CommandApp<InteractiveLoopCommand> app,
        IAnsiConsole console,
        EnsureCurrentUserExistsCommand ensureUserCommand)
    {
        _app = app;
        _console = console;
        _ensureUserCommand = ensureUserCommand;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        await Initialize(context);

        while (true)
        {
            _console.WriteLine();
            var commandText = _console.Prompt(new TextPrompt<string>(Globals.AppPrompt));

            var args = CliParser.SplitCommandLine(commandText);
            var result = await _app.RunAsync(args);

            if (result == Globals.S_EXIT)
            {
                return Globals.S_OK;
            }
        }
    }

    private async Task Initialize(CommandContext context)
    {
        _console.Write(
            new FigletText(Globals.AppTitle)
                .Centered()
                .Color(Color.Green));

        var result = await _ensureUserCommand.ExecuteAsync(context);
        if (result == Globals.S_OK)
        {
            _console.MarkupLine(Globals.AppInitializeSuccessMsg);
        }
    }
}
