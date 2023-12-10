using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem08
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (map, directions) = await ParseMap(input);
        var pos = 0;
        var current = "AAA";
        
        while (current != "ZZZ")
        {
            var d = directions[pos++ % directions.Length];
            current = d == 'L' ? map[current].left : map[current].right;
        }

        return pos.ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var (map, directions) = await ParseMap(input);
        var nodes = map.Keys.Where(k => k.EndsWith('A')).ToList();
        var stepsNeeded = new List<long>();

        foreach (var node in nodes)
        {
            var pos = 0;
            var current = node;
        
            while (!current.EndsWith('Z') || (pos % directions.Length) != 0)
            {
                var d = directions[pos++ % directions.Length];
                current = d == 'L' ? map[current].left : map[current].right;
            }

            stepsNeeded.Add(pos);
        }
        
        // Find the prime numbers needed for the steps
        var primeNumbers = new Dictionary<int, int>();
        foreach (var steps in stepsNeeded)
        {
            var stepsPrimes = new Dictionary<int, int>();
            var remainder = steps;
            while (remainder > 1)
            {
                for (var j = 2; j <= remainder; j++)
                {
                    if ((remainder % j) != 0) continue;
                    remainder /= j;
                    if (!stepsPrimes.TryAdd(j, 1))
                        stepsPrimes[j]++;
                    break;
                }
            }

            foreach (var (k, v) in stepsPrimes)
                if (!primeNumbers.TryAdd(k, v))
                    primeNumbers[k] = Math.Max(primeNumbers[k], v);
        }
        
        // Now that we know the steps needed, find where they overlap.
        return primeNumbers.Select(kv => kv.Key).Aggregate(1L, (agg, val) => agg * val).ToString();
    }

    private static async Task<(Dictionary<string, (string left, string right)> map, string directions)> ParseMap(Stream input)
    {
        using StreamReader reader = new(input);
        var map = new Dictionary<string, (string left, string right)>();
        var pattern = new Regex(@"^(?<node>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)$");

        var directions = await reader.ReadLineAsync() ?? throw new NotImplementedException();
        
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.Length == 0)
                continue;

            if (pattern.Match(line) is { Success: true } match)
                map.Add(match.Groups["node"].Value, (match.Groups["left"].Value, match.Groups["right"].Value));
            else
                throw new NotImplementedException();
        }

        return (map, directions);
    }
}