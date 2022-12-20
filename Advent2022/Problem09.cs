namespace Advent2022;

public static class Problem09
{
    private record struct Position(int X, int Y);

    public static async Task<string> SolvePart1Async(Stream input)
    {
        return await SolveForKnots(input, 2);
    }

    public static async Task<string> SolvePart2Async(Stream input)
    {
        return await SolveForKnots(input, 10);
    }

    private static async Task<string> SolveForKnots(Stream input, int knotCount)
    {
        using StreamReader reader = new(input);
        Position[] knots = new Position[knotCount];
        HashSet<Position> visitedTailPositions = new();

        // Clearly we visited the tail since this is where we started...
        visitedTailPositions.Add(knots.Last());

        while (await reader.ReadLineAsync() is { } line)
        {
            char direction = line[0];
            int magnitude = int.Parse(line[2..]);

            for (int m = 0; m < magnitude; m++)
            {
                knots[0] = direction switch
                {
                    'R' => knots[0] with { X = knots[0].X + 1 },
                    'L' => knots[0] with { X = knots[0].X - 1 },
                    'U' => knots[0] with { Y = knots[0].Y - 1 },
                    'D' => knots[0] with { Y = knots[0].Y + 1 },
                    _ => throw new NotImplementedException(),
                };

                for (int k = 1; k < knotCount; k++)
                {
                    int dx = knots[k - 1].X - knots[k].X;
                    int dy = knots[k - 1].Y - knots[k].Y;

                    if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
                    {
                        // Overlapping or touching - do nothing.
                        continue;
                    }

                    if (dx == 0)
                    {
                        // At this point, we know that dx is 0 and Abs(dy) is > 1. We need to move and
                        // dy will either be 2 or -2 and we only need to move 1 position closer
                        knots[k] = knots[k] with { Y = knots[k].Y + (dy / 2) };
                    }
                    else if (dy == 0)
                    {
                        // At this point, we know that dy is 0 and Abs(dx) is > 1. We need to move and
                        // dx will either be 2 or -2 and we only need to move 1 position closer
                        knots[k] = knots[k] with { X = knots[k].X + (dx / 2) };
                    }
                    else
                    {
                        // Time for a diagonal move
                        dx = Math.Clamp(dx, -1, 1);
                        dy = Math.Clamp(dy, -1, 1);
                        knots[k] = new Position(X: knots[k].X + dx, Y: knots[k].Y + dy);
                    }
                }
                visitedTailPositions.Add(knots.Last());
            }
        }

        return visitedTailPositions.Count.ToString();
    }
}
