//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace HabitTracker.Cli.Common;

public static class ErrorPresenter
{
    public static int Render(this Error error, IAnsiConsole console)
    {
        var msg = Markup.Escape(error.Message);
        console.MarkupLine($"[red]Error[/]: {msg}");
        return Globals.E_FAIL;
    }

    public static int Render(this IEnumerable<Error> errors, IAnsiConsole console)
    {
        if (errors.Count() > 1)
        {
            console.MarkupLine("[red]Multiple error messages[/]:");
            foreach (var error in errors)
            {
                var msg = Markup.Escape(error.Message);
                console.MarkupLine($" - {msg}");
            }
        }
        else if (errors.Count() == 1)
        {
            errors.First().Render(console);
        }

        return Globals.E_FAIL;
    }
}
