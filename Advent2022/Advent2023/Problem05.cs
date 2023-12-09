using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem05
{
    private class LongRange(long start, long length)
    {
        public long Start { get; } = start;
        public long End { get; } = start + length - 1;
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (seeds, ranges) = await ParseGarden(input);
        return (
            from seed in seeds 
            select MapRange(seed, ranges["seed-to-soil"]) into soil 
            select MapRange(soil, ranges["soil-to-fertilizer"]) into fertilizer 
            select MapRange(fertilizer, ranges["fertilizer-to-water"]) into water 
            select MapRange(water, ranges["water-to-light"]) into light 
            select MapRange(light, ranges["light-to-temperature"]) into temperature 
            select MapRange(temperature, ranges["temperature-to-humidity"]) into humidity 
            select MapRange(humidity, ranges["humidity-to-location"])).Min().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        throw new NotImplementedException();
    }

    private static async Task<(List<long> seeds, Dictionary<string, List<(LongRange destination, LongRange source)>> ranges)> ParseGarden(Stream input)
    {
        using StreamReader reader = new(input);
        var seedsPattern = new Regex(@"^seeds:\s*(?<seeds>[0-9\s]+)$");
        var mapPattern = new Regex(@"^(?<mapName>[a-z\-]+) map:$");
        var rangePattern = new Regex(@"^(?<destination>\d+)\s*(?<source>\d+)\s*(?<length>\d+)$");
        var seedMatch = seedsPattern.Match((await reader.ReadLineAsync())!);
        var seeds = seedMatch.Groups["seeds"].Value.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        var ranges = new Dictionary<string, List<(LongRange destination, LongRange source)>>();
        List<(LongRange destination, LongRange source)>? currentRange = null;
        
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.Length == 0)
            {
                currentRange = null;
            }

            if (mapPattern.Match(line) is { Success: true } mapMatch)
            {
                if (currentRange != null)
                    throw new NotImplementedException();
                
                ranges.Add(mapMatch.Groups["mapName"].Value, currentRange = []);
            }
            else if (rangePattern.Match(line) is { Success: true } rangeMatch)
            {
                if (currentRange == null)
                    throw new NotImplementedException();

                var (destination, source, length) = (long.Parse(rangeMatch.Groups["destination"].Value), long.Parse(rangeMatch.Groups["source"].Value), long.Parse(rangeMatch.Groups["length"].Value));
                currentRange.Add((new LongRange(destination, length), new LongRange(source, length)));
            }
        }

        return (seeds, ranges);
    }

    private static long MapRange(long value, List<(LongRange destination, LongRange source)> ranges)
    {
        foreach (var range in ranges.Where(range => value >= range.source.Start && value <= range.source.End))
            return range.destination.Start + (value - range.source.Start);
        return value;
    }
}