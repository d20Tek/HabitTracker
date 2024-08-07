//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace HabitTracker.Cli.UnitTests.Fakes;

internal static class CommandContextFactory
{
    [ExcludeFromCodeCoverage]
    internal class RemainingArgumentsFake : IRemainingArguments
    {
        private readonly string[] _args;

        public RemainingArgumentsFake(string[] args)
        {
            _args = args;
        }

        public ILookup<string, string?> Parsed => throw new NotImplementedException();

        public IReadOnlyList<string> Raw => _args.AsReadOnly();
    }

    public static CommandContext Create(string name = "none", string[]? args = null)
    {
        args ??= [];
        var remaining = new RemainingArgumentsFake(args);

        return new CommandContext(args, remaining, name, null);
    }
}
