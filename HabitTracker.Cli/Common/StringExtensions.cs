//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace HabitTracker.Cli.Common;

internal static class StringExtensions
{
    public enum Overflow
    {
        Wrap,
        Crop,
        Ellipsis
    }

    private const string _ellipsis = "...";
    private const int _ellipsisLength = 3;

    public static string CapOverflow(this string str, int maxLength, Overflow overflow = Overflow.Ellipsis)
    {
        var result = str;
        if (str.Length > maxLength)
        {
            if (overflow == Overflow.Crop)
            {
                result = str.Substring(0, maxLength);
            }
            else if (overflow == Overflow.Ellipsis)
            {
                result = string.Concat(str.AsSpan(0, maxLength - _ellipsisLength), _ellipsis);
            }
        }

        return result;
    }
}
