//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Common;

internal class ExitCommand : Command
{
    private readonly IAnsiConsole _console;

    public ExitCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override int Execute(CommandContext context)
    {
        _console.MarkupLine("[bold green]Bye![/] Keep up your habits...");
        return Globals.S_EXIT;
    }
}
