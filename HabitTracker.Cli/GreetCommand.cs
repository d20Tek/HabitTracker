//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli;

public class GreetCommand : Command<GreetCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("The name of the user")]
        public string Name { get; set; } = "";
    }

    private readonly IAnsiConsole _console;

    public GreetCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (string.IsNullOrEmpty(settings.Name))
        {
            settings.Name = _console.Ask<string>("What's your [green]name[/]?");
        }

        _console.MarkupLine($"Hello, [bold yellow]{settings.Name}[/]!");
        return Globals.S_OK;
    }
}
