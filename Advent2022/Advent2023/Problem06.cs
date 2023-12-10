namespace Advent2022.Advent2023;

public static class Problem06
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (times, distances) = await ParseRaces(input);
        throw new NotImplementedException();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        throw new NotImplementedException();
    }

    private static async Task<(List<int> times, List<int> distances)> ParseRaces(Stream input)
    {
        using StreamReader reader = new(input);
        var times = (await reader.ReadLineAsync())![9..].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var distances = (await reader.ReadLineAsync())![9..].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        return (times, distances);
    }
}