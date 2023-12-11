using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem09
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var readings = await ParseReadings(input);
        return readings.Select(FindNextValuePart1).Sum().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var readings = await ParseReadings(input);
        return readings.Select(FindNextValuePart2).Sum().ToString();
    }

    private static async Task<List<List<long>>> ParseReadings(Stream input)
    {
        using StreamReader reader = new(input);
        var result = new List<List<long>>(); 
        
        while (await reader.ReadLineAsync() is { } line)
        {
            result.Add(line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());
        }

        return result;
    }

    private static long FindNextValuePart1(List<long> values)
    {
        if (values.All(v => v == 0))
            return 0;

        var nextRow = new List<long>();
        for (var i = 1; i < values.Count; i++)
        {
            nextRow.Add(values[i] - values[i-1]);
        }

        var nextValue = FindNextValuePart1(nextRow);
        return values.Last() + nextValue;
    }
    
    private static long FindNextValuePart2(List<long> values)
    {
        if (values.All(v => v == 0))
            return 0;

        var nextRow = new List<long>();
        for (var i = 1; i < values.Count; i++)
        {
            nextRow.Add(values[i] - values[i-1]);
        }

        var nextValue = FindNextValuePart2(nextRow);
        return values.First() - nextValue;
    }
}