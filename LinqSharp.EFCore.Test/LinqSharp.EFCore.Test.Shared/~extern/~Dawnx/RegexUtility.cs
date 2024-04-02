using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dawnx.Utilities;

public static class RegexUtility
{
    /// <summary>
    /// Compile IPv4RangeRegexExp to Regex.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static string IPRange(string expression)
    {
        if (expression.Count(".") != 3) throw new FormatException("IPv4 range expression must have 4 parts.");

        var parts = expression.Split('.').Select(x => x.Trim());
        var regexParts = new string[4];
        foreach (var (index, part) in parts.Pairs())
        {
            switch (part)
            {
                case "*":
                    regexParts[index] = $"(?:{NumberRange(0, 255)})";
                    break;

                case string s when new Regex(@"^\d+$").Match(s).Success:
                    var value = int.Parse(s);

                    if (!CheckIPPart(value))
                        throw new FormatException("Each part of IPv4 must be between 0 and 255.");

                    regexParts[index] = s;
                    break;

                case string s:
                    var regex = new Regex(@"^(\d+) *~ *(\d+)$");
                    var match = regex.Match(s);

                    if (match.Success)
                    {
                        var from = int.Parse(match.Groups[1].Value);
                        var to = int.Parse(match.Groups[2].Value);

                        if (!CheckIPPart(from) || !(CheckIPPart(to)))
                            throw new FormatException("Each part of IPv4 must be between 0 and 255.");

                        regexParts[index] = $"(?:{NumberRange(from, to)})";
                    }
                    break;
            }
        }

        return regexParts.Join("\\.");
    }
    private static bool CheckIPPart(int value) => 0 <= value && value <= 255;

    public static string NumberRange(int from, int to)
    {
        if (from < to) return NumberRange(from.ToString(), to.ToString());
        else if (from == to) return from.ToString();
        else return "";
    }
    private static string NumberRange(string from, string to)
    {
        var fromLength = from.Length;
        var toLength = to.Length;

        if (fromLength == toLength)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < fromLength; i++)
            {
                if (i == 0)
                {
                    if (from[i] == to[i])
                        sb.Append(from[i]);
                    else sb.Append($"[{from[i]}-{to[i]}]");
                }
                else
                {
                    sb.Append("\\d");
                    if (from[i] > '1')
                        sb.Append($"(?<!{from.Slice(0, i)}[0-{(char)(from[i] - 1)}])");
                    else if (from[i] == '1')
                        sb.Append($"(?<!{from.Slice(0, i)}0)");

                    if (to[i] < '8')
                        sb.Append($"(?<!{to.Slice(0, i)}[{(char)(to[i] + 1)}-9])");
                    else if (to[i] == '8')
                        sb.Append($"(?<!{to.Slice(0, i)}9)");
                }
            }
            return sb.ToString();
        }
        else
        {
            var parts = new List<string>();

            for (int count = toLength; count >= fromLength; count--)
            {
                if (count == toLength)
                    parts.Add(NumberRange($"1{"0".Repeat(count - 1)}", to));
                else if (count == fromLength)
                    parts.Add(NumberRange(from, "9".Repeat(count)));
                else parts.Add("\\d".Repeat(count));
            }
            return parts.Join("|");
        }
    }

}
