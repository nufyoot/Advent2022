using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem05
{
    private class LongRange(long start, long length)
    {
        public long Start { get; } = start;
        public long End { get; } = start + length - 1;

        public long Length { get; } = length;

        public bool Intersects(LongRange other)
        {
            return Start <= other.End && End >= other.Start;
        }

        public bool Within(LongRange other)
        {
            return Start >= other.Start && End <= other.End;
        }
        
        public LongRange? Intersect(LongRange other)
        {
            if (!Intersects(other))
                return null;
            
            var maxStart = Math.Max(Start, other.Start);
            var minEnd = Math.Min(End, other.End);

            return new LongRange(maxStart, minEnd - maxStart + 1);
        }

        public List<LongRange> Except(LongRange other)
        {
            if (!Intersects(other))
                return [this];
            if (Within(other))
                return [];
            
            var result = new List<LongRange>();
            if (Start < other.Start)
                result.Add(new LongRange(Start, other.Start - Start));
            if (End > other.End)
                result.Add(new LongRange(other.End + 1, End - other.End));
            return result;
        }

        public override string ToString()
        {
            return $"{Start}..{End}({length}";
        }
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (seeds, ranges) = await ParseGarden(input);
        var locations = (
            from seed in seeds
            select MapRange([new LongRange(seed, 1)], ranges["seed-to-soil"]) into soil
            select MapRange(soil, ranges["soil-to-fertilizer"]) into fertilizer
            select MapRange(fertilizer, ranges["fertilizer-to-water"]) into water
            select MapRange(water, ranges["water-to-light"]) into light
            select MapRange(light, ranges["light-to-temperature"]) into temperature
            select MapRange(temperature, ranges["temperature-to-humidity"]) into humidity
            select MapRange(humidity, ranges["humidity-to-location"]));
        return locations.Min(l => l.Min(r => r.Start)).ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var (seeds, ranges) = await ParseGarden(input);
        var seedRanges = new List<LongRange>();
        for (var i = 0; i < seeds.Count; i += 2)
        {
            seedRanges.Add(new LongRange(seeds[i], seeds[i+1]));
        }
        
        var locations = (
            from seedRange in seedRanges
            select MapRange([seedRange], ranges["seed-to-soil"]) into soil
            select MapRange(soil, ranges["soil-to-fertilizer"]) into fertilizer
            select MapRange(fertilizer, ranges["fertilizer-to-water"]) into water
            select MapRange(water, ranges["water-to-light"]) into light
            select MapRange(light, ranges["light-to-temperature"]) into temperature
            select MapRange(temperature, ranges["temperature-to-humidity"]) into humidity
            select MapRange(humidity, ranges["humidity-to-location"]));
        return locations.Min(l => l.Min(r => r.Start)).ToString();
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

    private static List<LongRange> MapRange(List<LongRange> values, List<(LongRange destination, LongRange source)> ranges)
    {
        var result = new List<LongRange>();
        var valueRangeQueue = new Queue<LongRange>(values);

        while (valueRangeQueue.Count > 0)
        {
            var valueRange = valueRangeQueue.Dequeue();
            
            foreach (var (destination, source) in ranges)
            {
                // The value range we are looking at is entirely within the source range. Just map it back.
                if (valueRange.Within(source))
                {
                    var offset = valueRange.Start - source.Start;
                    result.Add(new LongRange(destination.Start + offset, valueRange.Length));
                    valueRange = null;
                    break;
                }
                
                if (valueRange.Intersect(source) is { } intersection)
                {
                    var offset = intersection.Start - source.Start;
                    result.Add(new LongRange(destination.Start + offset, intersection.Length));
                    foreach (var outsideIntersection in valueRange.Except(intersection))
                    {
                        valueRangeQueue.Enqueue(outsideIntersection);
                    }
                    valueRange = null;
                    break;
                }
            }

            if (valueRange is not null)
            {
                result.Add(valueRange);
            }
        }
        
        return result;
    }
}