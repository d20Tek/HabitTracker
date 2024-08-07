//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace HabitTracker.Cli.UnitTests.Fakes;

[ExcludeFromCodeCoverage]
internal class TestConsole : IAnsiConsole, IDisposable
{
    private readonly IAnsiConsole _console;
    private readonly StringWriter _writer;
    private IAnsiConsoleCursor? _cursor;

    public Profile Profile => _console.Profile;

    public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

    public IAnsiConsoleInput Input => TestInput;

    public RenderPipeline Pipeline => _console.Pipeline;

    public IAnsiConsoleCursor Cursor => _cursor ?? _console.Cursor;

    public TestConsoleInput TestInput { get; set; }

    public string Output => _writer.ToString();

    public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(new char[] { '\n' });

    public bool EmitAnsiSequences { get; set; }

    public TestConsole()
    {
        _writer = new StringWriter();
        EmitAnsiSequences = false;
        TestInput = new TestConsoleInput();

        _console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Yes,
            ColorSystem = (ColorSystemSupport)ColorSystem.TrueColor,
            Out = new AnsiConsoleOutput(_writer),
            Interactive = InteractionSupport.No,
            Enrichment = new ProfileEnrichment
            {
                UseDefaultEnrichers = false,
            },
        });

        _console.Profile.Width = 80;
        _console.Profile.Height = 24;
        _console.Profile.Capabilities.Ansi = true;
        _console.Profile.Capabilities.Unicode = true;
    }

    private bool isDisposed = false;

    public void Dispose()
    {
        if (isDisposed == false)
        {
            _writer.Dispose();
            isDisposed = true;
        }
    }

    public void Clear(bool home) => _console.Clear(home);

    public void Write(IRenderable renderable)
    {
        if (EmitAnsiSequences)
        {
            _console.Write(renderable);
        }
        else
        {
            foreach (var segment in renderable.GetSegments(this))
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                Profile.Out.Writer.Write(segment.Text);
            }
        }
    }

    internal void SetCursor(IAnsiConsoleCursor? cursor) => _cursor = cursor;

    internal static TestConsole Create(params string[] args)
    {
        var testInput = new TestConsoleInput();
        foreach (string arg in args)
        {
            testInput.PushTextWithEnter(arg);
        }

        return new TestConsole
        {
            TestInput = testInput,
        };
    }
}
