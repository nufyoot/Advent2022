using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem16
{
    private record Tunnel(string Destination, int Distance);
    
    private record Node(string Name, int Rate, ImmutableList<Tunnel> Tunnels);

    private record Grid(ImmutableDictionary<string, Node> Nodes, string Position, int CurrentReleaseRate, int PressureReleased, int RemainingTime);
    
    
    private static int GetPotentialRelease(Grid grid)
    {
        var released = grid.PressureReleased;
        var toBeReleased = grid.CurrentReleaseRate * grid.RemainingTime;
        var couldBeReleasedAllAtOnce = grid.Nodes.Values.OrderByDescending(n => n.Rate).Take(grid.RemainingTime).Sum(n => n.Rate) * grid.RemainingTime;

        return released + toBeReleased + couldBeReleasedAllAtOnce;
    }
    
    private static int GetProjectedRelease(Grid grid)
    {
        var released = grid.PressureReleased;
        var toBeReleased = grid.CurrentReleaseRate * grid.RemainingTime;
        var additonalNodeRelease = grid.Nodes[grid.Position].Rate * (grid.RemainingTime - 1);

        return released + toBeReleased + additonalNodeRelease;
    }

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

        Grid champion = SimplifyGrid(new Grid(nodes.ToImmutableDictionary(), "AA", 0, 0, 30));
        PriorityQueue<Grid, int> gridsToCheck = new();
        gridsToCheck.Enqueue(champion, champion.RemainingTime);
        int totalChecks = 0;

        while (gridsToCheck.Count > 0)
        {
            totalChecks++;
            var grid = gridsToCheck.Dequeue();
            
            // Release some pressure and take one minute off the clock.
            var newGrid = grid with
            {
                PressureReleased = grid.PressureReleased + grid.CurrentReleaseRate,
                RemainingTime = grid.RemainingTime - 1,
            };

            if (GetPotentialRelease(newGrid) < champion.PressureReleased)
            {
                continue;
            }

            if (newGrid.RemainingTime == 0)
            {
                if (newGrid.PressureReleased > champion.PressureReleased)
                {
                    champion = newGrid;
                }
                continue;
            }
            
            // First, determine if we can open the valve in the current area
            var position = newGrid.Nodes[newGrid.Position];
            if (position.Rate > 0)
            {
                // Open the value here, and try out that scenario.
                var newPosition = position with { Rate = 0 };
                var openCurrentValve = newGrid with
                {
                    CurrentReleaseRate = newGrid.CurrentReleaseRate + position.Rate,
                    Nodes = newGrid.Nodes.SetItem(position.Name, newPosition),
                };
                openCurrentValve = SimplifyGrid(openCurrentValve);
                gridsToCheck.Enqueue(openCurrentValve, -GetProjectedRelease(openCurrentValve));
            }
            
            // We could try moving...
            if (newGrid.Nodes[newGrid.Position].Tunnels is { Count: >0 } tunnels)
            {
                foreach (var tunnel in tunnels)
                {
                    var dt = Math.Min(newGrid.RemainingTime, tunnel.Distance) - 1;
                    var gridMove = newGrid with
                    {
                        Position = tunnel.Destination,
                        RemainingTime = newGrid.RemainingTime - dt,
                        PressureReleased = newGrid.PressureReleased + (newGrid.CurrentReleaseRate * dt),
                    };
                    gridMove = SimplifyGrid(gridMove);
                    gridsToCheck.Enqueue(gridMove, -GetProjectedRelease(gridMove));
                }
            }
            else
            {
                // There's nowhere to move, so just cycle back though for another ride
                gridsToCheck.Enqueue(newGrid, -GetProjectedRelease(newGrid));
            }
        }
        
        return champion.PressureReleased.ToString();
    }

    private static Grid SimplifyGrid(Grid current)
    {
        var simplifiedNodes = current.Nodes;

        while (true)
        {
            var toBeSimplified = simplifiedNodes.Values
                .FirstOrDefault(n => n.Rate == 0 && n.Name != current.Position);

            if (toBeSimplified is { } nodeToBeRemoved)
            {
                // Replace anything coming to this node with a new tunnel and distance to the current node's tunnels
                var nodesLeadingHere =
                    from node in simplifiedNodes.Values
                    from tunnel in node.Tunnels
                    where tunnel.Destination == nodeToBeRemoved.Name
                    select (node, tunnel);
                foreach (var nodeLeadingHere in nodesLeadingHere)
                {
                    // Remove the current tunnel to the node being removed...
                    var oldTunnel = nodeLeadingHere.tunnel;
                    var newTunnels = nodeLeadingHere.node.Tunnels.Remove(oldTunnel);

                    // Now add tunnels to all the other tunnels that were previously accessible by the nod to be removed
                    foreach (var tunnelToBeRemoved in nodeToBeRemoved.Tunnels.Where(t =>
                                 t.Destination != nodeLeadingHere.node.Name))
                    {
                        var newTunnel = tunnelToBeRemoved with
                        {
                            Distance = tunnelToBeRemoved.Distance + oldTunnel.Distance
                        };
                        if (newTunnels.FirstOrDefault(t => t.Destination == newTunnel.Destination) is { } otherTunnel)
                        {
                            if (newTunnel.Distance < otherTunnel.Distance)
                            {
                                newTunnels = newTunnels.Replace(otherTunnel, newTunnel);
                            }
                        }
                        else
                        {
                            newTunnels = newTunnels.Add(newTunnel);
                        }
                    }

                    simplifiedNodes = simplifiedNodes.SetItem(
                        nodeLeadingHere.node.Name,
                        nodeLeadingHere.node with
                        {
                            Tunnels = newTunnels,
                        });
                }

                simplifiedNodes = simplifiedNodes.Remove(nodeToBeRemoved.Name);
            }
            else
            {
                break;
            }
        }

        return current with { Nodes = simplifiedNodes };
    }
}