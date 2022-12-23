using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem16
{
    private record struct Tunnel(string Destination, int Distance);
    
    private record struct Node(string Name, int Rate, ImmutableList<Tunnel> Tunnels);

    private record GridBase(string Position, int CurrentReleaseRate, int PressureReleased);
    private record Grid(ImmutableDictionary<string, Node> Nodes, string Position, int CurrentReleaseRate, int PressureReleased, int RemainingTime)
        : GridBase(Position, CurrentReleaseRate, PressureReleased);

    public static async Task<string> SolvePart1Async(Stream input)
    {
        var pattern =
            new Regex(
                @"^Valve (?<source>[A-Z]{2}) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<destinations>[A-Z ,]+)$");
        using StreamReader reader = new(input);

        Dictionary<string, Node> nodes = new();
        while (await reader.ReadLineAsync() is { } line)
        {
            if (pattern.Match(line) is { Success: true } match)
            {
                var tunnels = match.Groups["destinations"].Value
                    .Split(", ")
                    .Select(d => new Tunnel(d, 1))
                    .ToImmutableList();
                nodes.Add(
                    match.Groups["source"].Value,
                    new Node(
                        match.Groups["source"].Value,
                        int.Parse(match.Groups["rate"].Value),
                        tunnels));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        var start = new Grid(nodes.ToImmutableDictionary(), "AA", 0, 0, 30);
        var champion = FindChampion(start, new HashSet<GridBase>());
        
        return champion.PressureReleased.ToString();
    }

    private static Grid FindChampion(Grid grid, HashSet<GridBase> visitedNodes)
    {
        
    }
}