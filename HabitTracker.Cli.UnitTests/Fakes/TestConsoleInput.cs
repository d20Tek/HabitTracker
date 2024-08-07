//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace HabitTracker.Cli.UnitTests.Fakes;

[ExcludeFromCodeCoverage]
internal class TestConsoleInput : IAnsiConsoleInput
{
    private readonly Queue<ConsoleKeyInfo> _input;

    public TestConsoleInput()
    {
        _input = new Queue<ConsoleKeyInfo>();
    }

    public void PushText(string input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        foreach (var character in input)
        {
            PushCharacter(character);
        }
    }

    public void PushTextWithEnter(string input)
    {
        PushText(input);
        PushKey(ConsoleKey.Enter);
    }

    public void PushCharacter(char input)
    {
        var control = char.IsUpper(input);
        _input.Enqueue(new ConsoleKeyInfo(input, (ConsoleKey)input, false, false, control));
    }

    public void PushKey(ConsoleKey input) =>
        _input.Enqueue(new ConsoleKeyInfo((char)input, input, false, false, false));

    public bool IsKeyAvailable() => _input.Count > 0;

    public ConsoleKeyInfo? ReadKey(bool intercept)
    {
        if (_input.Count == 0)
        {
            throw new InvalidOperationException("No input available.");
        }

        return _input.Dequeue();
    }

    public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken) =>
        Task.FromResult(ReadKey(intercept));
}