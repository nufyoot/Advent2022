namespace Advent2022.Advent2023;

public static class Problem11
{
    private class Galaxy(long x, long y)
    {
        public long X { get; set; } = x;
        public long Y { get; set; } = y;
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var galaxies = await ParseGalaxies(input);
        return SolveWithExpansionRate(galaxies, 2);
    }
    
    public static async Task<string> SolvePart2Async(Stream input, int expansionRate)
    {
        var galaxies = await ParseGalaxies(input);
        return SolveWithExpansionRate(galaxies, expansionRate);
    }

    private static string SolveWithExpansionRate(List<Galaxy> galaxies, int expansionRate)
    {
        
        // Expand along y-axis
        var maxY = galaxies.Max(g => g.Y);
        var minY = galaxies.Min(g => g.Y);
        for (var y = maxY; y >= minY; y--)
        {
            if (galaxies.Any(g => g.Y == y)) continue;
            
            foreach (var galaxy in galaxies.Where(g => g.Y > y))
            {
                galaxy.Y += expansionRate - 1;
            }
        }
        
        // Expand along x-axis
        var maxX = galaxies.Max(g => g.X);
        var minX = galaxies.Min(g => g.X);
        for (var x = maxX; x >= minX; x--)
        {
            if (galaxies.Any(g => g.X == x)) continue;
            
            foreach (var galaxy in galaxies.Where(g => g.X > x))
            {
                galaxy.X += expansionRate - 1;
            }
        }
        
        // Great, now sum the distances
        var distanceSum = 0L;
        for (var g = 0; g < galaxies.Count-1; g++)
        {
            for (var g2 = g + 1; g2 < galaxies.Count; g2++)
            {
                var d = Math.Abs(galaxies[g].Y - galaxies[g2].Y) + Math.Abs(galaxies[g].X - galaxies[g2].X);
                distanceSum += d;
            }
        }

        return distanceSum.ToString();
    }

    private static async Task<List<Galaxy>> ParseGalaxies(Stream input)
    {
        using StreamReader reader = new(input);
        var galaxies = new List<Galaxy>();

        var row = 0;
        while (await reader.ReadLineAsync() is { } line)
        {
            row++;
            for (var col = 0; col < line.Length; col++)
            {
                if (line[col] == '#')
                    galaxies.Add(new Galaxy(col, row));
            }
        }

        return galaxies;
    }
}