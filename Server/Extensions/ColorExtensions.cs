using System.Globalization;
using System.Text.RegularExpressions;

namespace DOOH.Server.Extensions;

public static class ColorExtensions
{
    private static readonly Regex Hex6Regex = new Regex(@"^#?([0-9a-fA-F]{6})$", RegexOptions.Compiled);
    private static readonly Regex Hex3Regex = new Regex(@"^#?([0-9a-fA-F]{3})$", RegexOptions.Compiled);

    public static string ToRgba(this string color, double alpha = 1.0)
    {
        color = color.TrimStart('#');

        if (Hex6Regex.IsMatch(color))
        {
            return $"rgba({int.Parse(color.Substring(0, 2), NumberStyles.HexNumber)}, {int.Parse(color.Substring(2, 2), NumberStyles.HexNumber)}, {int.Parse(color.Substring(4, 2), NumberStyles.HexNumber)}, {alpha})";
        }
        else if (Hex3Regex.IsMatch(color))
        {
            return $"rgba({int.Parse(new string(color[0], 2), NumberStyles.HexNumber)}, {int.Parse(new string(color[1], 2), NumberStyles.HexNumber)}, {int.Parse(new string(color[2], 2), NumberStyles.HexNumber)}, {alpha})";
        }
        else
        {
            throw new ArgumentException("Invalid color format", nameof(color));
        }
    }
}