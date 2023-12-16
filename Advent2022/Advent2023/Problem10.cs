using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem10
{
    private class Grid(List<List<char>> map)
    {
        public int?[,] PathValues { get; } = new int?[map.Count, map[0].Count];

        public char this[int x, int y] => map[y][x];
        public int Height => map.Count;
        public int Width => map[0].Count;
        public List<List<char>> Map => map;

        public Coordinate GetStartingPosition()
        {
            for (var row = 0; row < map.Count; row++)
            {
                var x = map[row].IndexOf('S');
                if (x < 0) continue;
                
                var start = new Coordinate(this, x, row);
                start.SetDistance(0);
                return start;
            }

            throw new NotImplementedException();
        }

        public void WalkMainLoop()
        {
            var start = GetStartingPosition();
            var firstPath = start.NextUnvisited();
            var secondPath = start.NextUnvisited();

            while (true)
            {
                firstPath = firstPath?.NextUnvisited();
                secondPath = secondPath?.NextUnvisited();

                if (firstPath is null || secondPath is null)
                    break;
            }
            start.SetDistance(-1);
        }
    }

    private readonly struct Coordinate(Grid grid, int x, int y)
    {
        public int? Distance => grid.PathValues[y, x];
        private char Value => grid[x, y];
        
        public static implicit operator char(Coordinate c) => c.Value;

        public Coordinate? Up => y <= 0 ? null : new Coordinate(grid, x, y - 1);
        public Coordinate? Down => y >= grid.Height ? null : new Coordinate(grid, x, y + 1);
        public Coordinate? Left => x <= 0 ? null : new Coordinate(grid, x - 1, y);
        public Coordinate? Right => x >= grid.Width ? null : new Coordinate(grid, x + 1, y);
        public bool CanGoUp => Value is '|' or 'J' or 'L';
        public bool CanGoDown => Value is '|' or '7' or 'F';
        public bool CanGoLeft => Value is '-' or 'J' or '7';
        public bool CanGoRight => Value is '-' or 'L' or 'F';
        public bool IsNotGround => Value is not '.';

        public Coordinate? NextUnvisited()
        {
            Coordinate? next;
            if (CanGoUp && Up is { Distance: null, IsNotGround: true } up)
                next = up;
            else if (CanGoDown && Down is { Distance: null, IsNotGround: true } down)
                next = down;
            else if (CanGoLeft && Left is { Distance: null, IsNotGround: true } left)
                next = left;
            else if (CanGoRight && Right is { Distance: null, IsNotGround: true } right)
                next = right;
            else
                next = Value switch
                {
                    'S' when Up is { Distance: null, CanGoDown: true } up2 => up2,
                    'S' when Down is { Distance: null, CanGoUp: true } down2 => down2,
                    'S' when Left is { Distance: null, CanGoRight: true } left2 => left2,
                    'S' when Right is { Distance: null, CanGoLeft: true } right2 => right2,
                    _ => null
                };

            next?.SetDistance(Distance.GetValueOrDefault() + 1);
            return next;
        }

        public void SetDistance(int distance)
        {
            grid.PathValues[y, x] = distance;
        }

        public void SetChar(char c)
        {
            grid.Map[y][x] = c;
        }

        public override string ToString()
        {
            return $"'{Value}' ({x}, {y}) [{Distance}]";
        }
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var grid = await ParseGrid(input);
        grid.WalkMainLoop();
        return grid.PathValues.Cast<int?>().Where(i => i.HasValue).Cast<int>().Max().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var grid = await ParseGrid(input);
        grid.WalkMainLoop();
        
        // Replace the 'S' as the starting point 
        var start = grid.GetStartingPosition();
        var (up, down, left, right) = (start.Up, start.Down, start.Left, start.Right);
        
        
        // Now find the bits that are inside the main loop
        // Start with north -> south
        var insideLoopGroundCount = 0;
        for (var x = 0; x < grid.Width; x++)
        {
            var loopStatus = LoopWalkStatus.Outside;
            var potential = 0;
            for (var y = 0; y < grid.Height; y++)
            {
                var c = new Coordinate(grid, x, y);
                if (c.Distance.HasValue)
                {
                    switch (loopStatus)
                    {
                        case LoopWalkStatus.OnLoopPreviousOutside or LoopWalkStatus.OnLoopPreviousInside when c.CanGoUp:
                            continue;
                        case LoopWalkStatus.OnLoopPreviousOutside when !c.CanGoUp:
                            loopStatus = LoopWalkStatus.OnLoopPreviousInside;
                            continue;
                        case LoopWalkStatus.OnLoopPreviousInside when !c.CanGoUp:
                            loopStatus = LoopWalkStatus.OnLoopPreviousOutside;
                            continue;
                        case LoopWalkStatus.Outside:
                            loopStatus = LoopWalkStatus.OnLoopPreviousOutside;
                            break;
                        case LoopWalkStatus.Inside:
                            insideLoopGroundCount += potential;
                            potential = 0;
                            loopStatus = LoopWalkStatus.OnLoopPreviousInside;
                            break;
                    }
                }
                else if (!c.Distance.HasValue)
                {
                    switch (loopStatus)
                    {
                        case LoopWalkStatus.OnLoopPreviousOutside:
                            loopStatus = LoopWalkStatus.Inside;
                            potential = 1;
                            break;
                        case LoopWalkStatus.Inside:
                            potential++;
                            break;
                        case LoopWalkStatus.OnLoopPreviousInside:
                            loopStatus = LoopWalkStatus.Outside;
                            break;
                        case LoopWalkStatus.Outside:
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        return insideLoopGroundCount.ToString();
    }

    private static async Task<Grid> ParseGrid(Stream input)
    {
        using StreamReader reader = new(input);
        var map = new List<List<char>>();
        
        while (await reader.ReadLineAsync() is { } line)
        {
            map.Add(line.ToCharArray().ToList());
        }

        return new Grid(map);
    }

    private enum LoopWalkStatus
    {
        Outside,
        Inside,
        OnLoopPreviousOutside,
        OnLoopPreviousInside,
    }
}