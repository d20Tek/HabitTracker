//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Common;

public static class SuccessPresenter
{
    public static int RenderSuccess(this IAnsiConsole console, string message)
    {
        console.MarkupLine($"[green]Success:[/] {Markup.Escape(message)}");
        return Globals.S_OK;
    }
}
