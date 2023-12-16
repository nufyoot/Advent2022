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
        
        // Replace the starting position with the correct letter
        var start = grid.GetStartingPosition();
        var (up, down, left, right) = (start.Up, start.Down, start.Left, start.Right);
        
        if (up?.CanGoDown == true && down?.CanGoUp == true)
            start.SetChar('|');
        else if (up?.CanGoDown == true && left?.CanGoRight == true)
            start.SetChar('J');
        else if (up?.CanGoDown == true && right?.CanGoLeft == true)
            start.SetChar('L');
        else if (down?.CanGoUp == true && left?.CanGoRight == true)
            start.SetChar('7');
        else if (down?.CanGoUp == true && right?.CanGoLeft == true)
            start.SetChar('F');
        else
            throw new NotImplementedException();

        int insideLoopCount = 0;
        for (var x = 0; x < grid.Width; x++)
        {
            var walkStatus = LoopWalkStatus.Outside;
            for (var y = 0; y < grid.Height; y++)
            {
                var c = new Coordinate(grid, x, y);
                if (!c.Distance.HasValue)
                {
                    // We stepped on a tile not on the loop. If we are inside the loop, count this tile.
                    if (walkStatus == LoopWalkStatus.Inside)
                        insideLoopCount++;
                }
                else
                {
                    // Ok, we stepped on a loop tile - we need to figure out how to move forward from here.
                    if (c.CanGoLeft && c.CanGoRight)
                    {
                        // We stepped on a '-' tile, which means we just flip inside/outside status
                        walkStatus = walkStatus.FlipInsideOutside();
                    }
                    else if (c.CanGoUp && c.CanGoDown)
                    {
                        // If we can go up and down, then we already captured being on the loop, and we're just walking along. Change nothing.
                        continue;
                    }
                    else if (c.CanGoUp)
                    {
                        if (c.CanGoLeft)
                        {
                            if ((walkStatus & LoopWalkStatus.Right) == LoopWalkStatus.Right)
                                // We entered from the right and just went left. Flip inside/outside
                                walkStatus = walkStatus.FlipInsideOutside();
                        }
                        else if (c.CanGoRight)
                        {
                            if ((walkStatus & LoopWalkStatus.Left) == LoopWalkStatus.Left)
                                // We entered from the right and just went left. Flip inside/outside
                                walkStatus = walkStatus.FlipInsideOutside();
                        }
                        else
                            throw new NotImplementedException();
                        
                        // Remove the fact that we entered from a direction earlier
                        walkStatus &= ~LoopWalkStatus.EntranceDirection;
                        // Remove the fact we are on the loop
                        walkStatus &= ~LoopWalkStatus.OnLoop;
                    }
                    else if (c.CanGoDown)
                    {
                        if (c.CanGoLeft)
                            walkStatus |= LoopWalkStatus.OnLoopLeftEntrance;
                        else if (c.CanGoRight)
                            walkStatus |= LoopWalkStatus.OnLoopRightEntrance;
                        else
                            throw new NotImplementedException();
                    }
                }
            }
        }

        return insideLoopCount.ToString();
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

    [Flags]
    private enum LoopWalkStatus
    {
        Outside = 1,
        Inside = 2,
        OnLoop = 4,
        Left = 8,
        Right = 16,
        OnLoopRightEntrance = OnLoop | Right,
        OnLoopLeftEntrance = OnLoop | Left,
        EntranceDirection = Left | Right,
    }

    private static bool IsOnLoop(this LoopWalkStatus status)
    {
        return (status & LoopWalkStatus.OnLoop) == LoopWalkStatus.OnLoop;
    }

    private static LoopWalkStatus FlipInsideOutside(this LoopWalkStatus status)
    {
        return status ^ (LoopWalkStatus.Inside | LoopWalkStatus.Outside);
    }
    
    private static bool IsInside(this LoopWalkStatus status)
    {
        return (status & LoopWalkStatus.Inside) == LoopWalkStatus.Inside;
    }
    
    private static bool IsOutside(this LoopWalkStatus status)
    {
        return (status & LoopWalkStatus.Outside) == LoopWalkStatus.Outside;
    }
}