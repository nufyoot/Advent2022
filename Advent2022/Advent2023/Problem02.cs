using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem02
{
    public record CubeCount(int Red, int Green, int Blue);

    private record Game(int Id, List<CubeCount> Revealed);
    
    public static async Task<string> SolvePart1Async(Stream input, CubeCount maxCubes)
    {
        var games = await ParseGames(input);
        return games
            .Where(g => g.Revealed.All(r => r.Red <= maxCubes.Red && r.Green <= maxCubes.Green && r.Blue <= maxCubes.Blue))
            .Select(g => g.Id)
            .Sum()
            .ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var games = await ParseGames(input);
        return games
            .Select(g => g.Revealed.Max(r => r.Red) * g.Revealed.Max(r => r.Green) * g.Revealed.Max(r => r.Blue))
            .Sum()
            .ToString();
    }

    private static async Task<List<Game>> ParseGames(Stream input)
    {
        using StreamReader reader = new(input);
        var result = new List<Game>();

        while (await reader.ReadLineAsync() is { } line)
        {
            var gamePattern = new Regex(@"^Game (?<id>\d+): (?<cubes>.+)$");
            if (gamePattern.Match(line) is { Success: true } match)
            {
                var matchId = int.Parse(match.Groups["id"].Value);
                var cubeCounts = new List<CubeCount>();
                foreach (var cubes in match.Groups["cubes"].Value.Split(";").Select(s => s.Trim()))
                {
                    var cubeCount = new CubeCount(0, 0, 0);
                    foreach (var colorRevealed in cubes.Split(",").Select(s => s.Trim()))
                    {
                        var parts = colorRevealed.Split(" ", 2);
                        var (count, color) = (int.Parse(parts[0]), parts[1]);
                        cubeCount = color switch
                        {
                            "red" => cubeCount with { Red = count },
                            "green" => cubeCount with { Green = count },
                            "blue" => cubeCount with { Blue = count },
                            _ => throw new NotImplementedException(),
                        };
                    }
                    cubeCounts.Add(cubeCount);
                }
                result.Add(new Game(matchId, cubeCounts));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        return result;
    }
}