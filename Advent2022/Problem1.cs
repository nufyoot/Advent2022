namespace Advent2022;

public static class Problem1
{
    private class Elf
    {
        public List<int> Calories { get; } = new();
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var elves = await GetCaloriesPerElf(input);
        return elves.Max(e => e.Calories.Sum()).ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var elves = await GetCaloriesPerElf(input);
        return elves
            .Select(e => e.Calories.Sum())
            .OrderByDescending(e => e)
            .Take(3)
            .Sum()
            .ToString();
    }

    private static async Task<List<Elf>> GetCaloriesPerElf(Stream input)
    {
        using StreamReader reader = new(input);
        List<Elf> elves = new();
        Elf? currentElf = null;

        while (await reader.ReadLineAsync() is { } line)
        {
            // If we reach an empty line, clear the elf and go to the next line in the file.
            if (line.Length == 0)
            {
                currentElf = null;
                continue;
            }

            // If there isn't a current elf we are adding calories to, then create one.
            if (currentElf == null)
            {
                elves.Add(currentElf = new Elf());
            }

            if (int.TryParse(line, out int calories))
            {
                currentElf.Calories.Add(calories);
            }
        }

        return elves;
    }
}