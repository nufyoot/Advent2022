using System.Text;
using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem10
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var instructionPattern = new Regex(@"^(?<instruction>noop|addx)(?:\s(?<amount>-?\d+))?$");
        using StreamReader reader = new(input);
        int clockCycle = 1;
        int x = 1;
        int nextReportCycle = 20;
        int reportCycleIncrement = 40;
        List<int> reportedValues = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            if (instructionPattern.Match(line) is { Success: true } match)
            {
                var instruction = match.Groups["instruction"].Value;
                int nextClockCycle = clockCycle;
                int newX = x;

                if (instruction == "noop")
                {
                    nextClockCycle++;
                }
                else if (instruction == "addx")
                {
                    nextClockCycle += 2;
                    newX += int.Parse(match.Groups["amount"].Value);
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (clockCycle < nextReportCycle && nextClockCycle > nextReportCycle)
                {
                    // This is where the report clock cycle will be skipped
                    reportedValues.Add(x * nextReportCycle);
                    nextReportCycle += reportCycleIncrement;
                }
                else if (nextClockCycle == nextReportCycle)
                {
                    reportedValues.Add(newX * nextReportCycle);
                    nextReportCycle += reportCycleIncrement;
                }

                clockCycle = nextClockCycle;
                x = newX;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        return reportedValues.Sum().ToString();
    }

    public static async Task<string> SolvePart2Async(Stream input)
    {
        var instructionPattern = new Regex(@"^(?<instruction>noop|addx)(?:\s(?<amount>-?\d+))?$");
        using StreamReader reader = new(input);
        int clockCycle = 1;
        int x = 1;
        List<char> screen = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            if (instructionPattern.Match(line) is { Success: true } match)
            {
                if (Math.Abs(((clockCycle - 1) % 40) - x) <= 1)
                {
                    screen.Add('#');
                }
                else
                {
                    screen.Add('.');
                }

                var instruction = match.Groups["instruction"].Value;

                if (instruction == "noop")
                {
                    clockCycle++;
                }
                else if (instruction == "addx")
                {
                    if (Math.Abs((clockCycle % 40) - x) <= 1)
                    {
                        screen.Add('#');
                    }
                    else
                    {
                        screen.Add('.');
                    }

                    clockCycle += 2;
                    x += int.Parse(match.Groups["amount"].Value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        var builder = new StringBuilder();
        for (int i = 0; i < screen.Count; i++)
        {
            builder.Append(screen[i]);
            if ((i + 1) % 40 == 0)
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }
}
