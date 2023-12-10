namespace Advent2022.Advent2023;

public static class Problem06
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var timesAndDistances = await ParseRacesPart1(input);
        var recordBreakers = new List<int>();
        foreach (var (time, distance) in timesAndDistances)
        {
            recordBreakers.Add((
                from acceleration in Enumerable.Range(0, time)
                let potentialDistance = acceleration * (time - acceleration)
                where potentialDistance > distance
                select potentialDistance).Count());
        }

        return recordBreakers.Aggregate(1L, (currentProduct, value) => currentProduct * value).ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var (time, distance) = await ParseRacesPart2(input);
        
        // Find the min time that sets a record
        var minTime = 0L;
        var maxTime = time;
        while (maxTime - minTime != 0)
        {
            var candidateTime = ((maxTime - minTime) / 2) + minTime;
            var potentialDistance = (time - candidateTime) * candidateTime;
            if (potentialDistance > distance)
                maxTime = Math.Max(minTime, candidateTime - 1);
            else if (potentialDistance <= distance)
                minTime = Math.Min(maxTime, candidateTime + 1);
        }
        
        var startTime = minTime;
        
        // Find the max time that sets a record
        minTime = 0L;
        maxTime = time;
        while (maxTime - minTime != 0)
        {
            var candidateTime = ((maxTime - minTime) / 2) + minTime;
            var potentialDistance = (time - candidateTime) * candidateTime;
            if (potentialDistance > distance)
                minTime = Math.Min(maxTime, candidateTime + 1);
            else if (potentialDistance <= distance)
                maxTime = Math.Max(minTime, candidateTime - 1);
        }

        var endTime = maxTime;

        return (endTime - startTime + 1).ToString();
    }

    private static async Task<List<(int time, int distance)>> ParseRacesPart1(Stream input)
    {
        using StreamReader reader = new(input);
        var times = (await reader.ReadLineAsync())![9..].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var distances = (await reader.ReadLineAsync())![9..].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        return times.Zip(distances).ToList();
    }
    
    private static async Task<(long time, long distance)> ParseRacesPart2(Stream input)
    {
        using StreamReader reader = new(input);
        var time = long.Parse((await reader.ReadLineAsync())![9..].Replace(" ", ""));
        var distance = long.Parse((await reader.ReadLineAsync())![9..].Replace(" ", ""));
        return (time, distance);
    }
}