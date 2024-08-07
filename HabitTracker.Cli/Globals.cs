//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
global using Spectre.Console;
global using Spectre.Console.Cli;
global using System.ComponentModel;

global using HabitTracker.Engine;
global using HabitTracker.Engine.Domain;

internal static class Globals
{
    public const int S_OK = 0;

    public const int S_EXIT = unchecked((int)0x80000001);

    public const int E_FAIL = -1;

    public const string AppTitle = "Habit Tracker";

    public const string AppInitializeSuccessMsg = "What would you like to track today?";

    public const string AppPrompt = "HT>";
}