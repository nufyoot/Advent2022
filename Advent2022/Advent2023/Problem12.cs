using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem12
{
    private class SpringMap
    {
        public SpringMap(char[] springs, int[] brokenCounts)
        {
            Springs = springs;
            BrokenCounts = brokenCounts;
            BrokenCountsAsString = string.Join(',', brokenCounts);
            UnknownPositions = springs.Select((c, index) => (c, index)).Where(t => t.c == '?').Select(t => t.index).ToArray();
        }
        
        public char[] Springs { get; }
        public int[] BrokenCounts { get; }
        public string BrokenCountsAsString { get; }
        public int[] UnknownPositions { get; }
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var springMaps = await ParseSprings(input);
        var possibleArrangements = 0;

        foreach (var springMap in springMaps)
        {
            var maxIterations = (long)Math.Pow(2, springMap.UnknownPositions.Length) - 1;
            
            // Initialize everything
            foreach (var position in springMap.UnknownPositions)
                springMap.Springs[position] = '#';
            
            for (var i = 0; i < maxIterations; i++)
            {
                List<int> brokenSprings = [0];
                foreach (var c in springMap.Springs)
                {
                    if (c == '.')
                        brokenSprings.Add(0);
                    else
                        brokenSprings[^1]++;
                }

                if (string.Join(',', brokenSprings.Where(t => t != 0)) == springMap.BrokenCountsAsString)
                {
                    possibleArrangements++;
                }
                
                // Change the current state. Do this at the end of the loop.
                foreach (var p in springMap.UnknownPositions)
                {
                    if (springMap.Springs[p] == '.')
                    {
                        springMap.Springs[p] = '#';
                    }
                    else
                    {
                        springMap.Springs[p] = '.';
                        break;
                    }
                }
            }
        }

        return possibleArrangements.ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input, int expansionRate)
    {
        throw new NotImplementedException();
    }

    private static async Task<List<SpringMap>> ParseSprings(Stream input)
    {
        using StreamReader reader = new(input);
        var linePattern = new Regex(@"^(?<springs>[\.\?\#]+)\s+(?<counts>[\d,]+)$");
        var springMaps = new List<SpringMap>(); 

        while (await reader.ReadLineAsync() is { } line)
        {
            if (linePattern.Match(line) is { Success: true } match)
            {
                springMaps.Add(new SpringMap(match.Groups["springs"].Value.ToCharArray(), match.Groups["counts"].Value.Split(",").Select(int.Parse).ToArray()));
            }
        }

        return springMaps;
    }
}