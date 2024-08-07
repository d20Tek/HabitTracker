//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using HabitTracker.Cli.Common;

namespace HabitTracker.Cli.UnitTests.Common;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void CapOverflow_WithLongTextAndEllipsis_ShortensText()
    {
        // arrange
        var text = "This is a message with really long text";

        // act
        var result = text.CapOverflow(10);

        // assert
        result.Length.Should().Be(10);
        result.Should().Contain("...");
    }

    [TestMethod]
    public void CapOverflow_WithLongTextAndCrop_ShortensText()
    {
        // arrange
        var text = "This is a message with really long text";

        // act
        var result = text.CapOverflow(10, Cli.Common.StringExtensions.Overflow.Crop);

        // assert
        result.Length.Should().Be(10);
        result.Should().NotContain("...");
    }

    [TestMethod]
    public void CapOverflow_WithLongTextAndWrap_DoesNotShortenText()
    {
        // arrange
        var text = "This is a message with really long text";

        // act
        var result = text.CapOverflow(10, Cli.Common.StringExtensions.Overflow.Wrap);

        // assert
        result.Length.Should().BeGreaterThan(10);
        result.Should().NotContain("...");
    }

    [TestMethod]
    public void CapOverflow_WithShortTextAndEllipsis_DoesNotShortenText()
    {
        // arrange
        var text = "Short message text";

        // act
        var result = text.CapOverflow(20);

        // assert
        result.Length.Should().Be(text.Length);
        result.Should().NotContain("...");
    }
}
