namespace Advent2022;

public static class Problem14
{
    private enum BlockType
    {
        Cave,
        Sand,
    }
    
    private record struct Point(int X, int Y);

    private record struct LineSegment(Point Start, Point Finish);

    private record struct Bounds(int MinX, int MinY, int MaxX, int MaxY);
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (maxBounds, blocks) = await ConstructBlocks(input);

        var iterations = 0;
        while (true)
        {
            var sandPosition = new Point(500, 0);
            if (blocks.ContainsKey(sandPosition))
            {
                // This shouldn't happen when starting out...
                throw new NotImplementedException();
            }

            bool isAtRest = false;
            for (var y = 1; y <= maxBounds.MaxY; y++)
            {
                var testPoint1 = new Point(sandPosition.X, sandPosition.Y + 1);
                var testPoint2 = new Point(sandPosition.X - 1, sandPosition.Y + 1);
                var testPoint3 = new Point(sandPosition.X + 1, sandPosition.Y + 1);

                if (!blocks.ContainsKey(testPoint1))
                {
                    sandPosition = testPoint1;
                }
                else if (!blocks.ContainsKey(testPoint2))
                {
                    sandPosition = testPoint2;
                }
                else if (!blocks.ContainsKey(testPoint3))
                {
                    sandPosition = testPoint3;
                }
                else
                {
                    blocks[sandPosition] = BlockType.Sand;
                    isAtRest = true;
                    iterations++;
                    break;
                }
            }

            if (!isAtRest)
            {
                break;
            }
        }

        return iterations.ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var (maxBounds, blocks) = await ConstructBlocks(input);

        var iterations = 0;
        while (true)
        {
            var sandPosition = new Point(500, 0);
            if (blocks.ContainsKey(sandPosition))
            {
                break;
            }

            for (var y = 1; y <= maxBounds.MaxY + 1; y++)
            {
                var testPoint1 = new Point(sandPosition.X, sandPosition.Y + 1);
                var testPoint2 = new Point(sandPosition.X - 1, sandPosition.Y + 1);
                var testPoint3 = new Point(sandPosition.X + 1, sandPosition.Y + 1);

                if (!blocks.ContainsKey(testPoint1))
                {
                    sandPosition = testPoint1;
                }
                else if (!blocks.ContainsKey(testPoint2))
                {
                    sandPosition = testPoint2;
                }
                else if (!blocks.ContainsKey(testPoint3))
                {
                    sandPosition = testPoint3;
                }
                else
                {
                    break;
                }
            }

            blocks[sandPosition] = BlockType.Sand;
            iterations++;
        }

        return iterations.ToString();
    }

    private static async Task<(Bounds maxBounds, Dictionary<Point, BlockType> blocks)> ConstructBlocks(Stream input)
    {
        using StreamReader reader = new(input);
        List<LineSegment> lineSegments = new();
        Bounds maxBounds = new(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

        while (await reader.ReadLineAsync() is { } line)
        {
            List<Point> points = line
                .Split("->", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().Split(',', 2))
                .Select(parts => new Point(int.Parse(parts[0]), int.Parse(parts[1])))
                .ToList();

            // Keep track of how big this gets
            maxBounds = new Bounds(
                Math.Min(maxBounds.MinX, points.Min(p => p.X)),
                Math.Min(maxBounds.MinY, points.Min(p => p.Y)),
                Math.Max(maxBounds.MaxX, points.Max(p => p.X)),
                Math.Max(maxBounds.MaxY, points.Max(p => p.Y)));

            for (var i = 1; i < points.Count; i++)
            {
                lineSegments.Add(new LineSegment(points[i - 1], points[i]));
            }
        }

        Dictionary<Point, BlockType> blocks = new();
        foreach (var lineSegment in lineSegments)
        {
            if (lineSegment.Start.X == lineSegment.Finish.X)
            {
                var x = lineSegment.Start.X;
                var minY = Math.Min(lineSegment.Start.Y, lineSegment.Finish.Y);
                var maxY = Math.Max(lineSegment.Start.Y, lineSegment.Finish.Y);
                for (var y = minY; y <= maxY; y++)
                {
                    blocks[new Point(x, y)] = BlockType.Cave;
                }
            }
            else if (lineSegment.Start.Y == lineSegment.Finish.Y)
            {
                var y = lineSegment.Start.Y;
                var minX = Math.Min(lineSegment.Start.X, lineSegment.Finish.X);
                var maxX = Math.Max(lineSegment.Start.X, lineSegment.Finish.X);
                for (var x = minX; x <= maxX; x++)
                {
                    blocks[new Point(x, y)] = BlockType.Cave;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        return (maxBounds, blocks);
    }
}