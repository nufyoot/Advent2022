namespace Advent2022;

public static class Problem08
{
    private class Tree
    {
        public Tree(char height)
        {
            Height = height;
        }

        public char Height { get; init; }
        public bool? Visible { get; set; }

        public override string ToString()
        {
            return $"{Height} - {Visible.ToString()}";
        }
    }

    private class GridTree : Tree
    {
        public GridTree(char height)
            : base(height)
        {
        }

        public GridTree? Left { get; set; }

        public GridTree? Right { get; set; }

        public GridTree? Up { get; set; }

        public GridTree? Down { get; set; }

        public int GetScenicScore()
        {
            int left = 0;
            int right = 0;
            int up = 0;
            int down = 0;
            GridTree? current;

            current = Left;
            while (current is not null)
            {
                left++;
                if (current.Height >= Height)
                {
                    break;
                }
                current = current.Left;
            }

            current = Right;
            while (current is not null)
            {
                right++;
                if (current.Height >= Height)
                {
                    break;
                }
                current = current.Right;
            }

            current = Up;
            while (current is not null)
            {
                up++;
                if (current.Height >= Height)
                {
                    break;
                }
                current = current.Up;
            }

            current = Down;
            while (current is not null)
            {
                down++;
                if (current.Height >= Height)
                {
                    break;
                }
                current = current.Down;
            }

            return left * right * up * down;
        }
    }

    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<Tree[]> grid = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            grid.Add(line.Select(c => new Tree(c)).ToArray());
        }

        int visibleCount = 0;

        void CheckCurrentVisibility(ref char champ, Tree current)
        {
            if (current.Height > champ)
            {
                if (current.Visible is null)
                {
                    visibleCount++;
                    current.Visible = true;
                }
                champ = current.Height;
            }
        }

        // left -> right
        foreach (var row in grid)
        {
            var champ = (char)('0' - 1);
            foreach (var cell in row)
            {
                CheckCurrentVisibility(ref champ, cell);
            }
        }

        // right -> left
        foreach (var row in grid)
        {
            var champ = (char)('0' - 1);
            for (var c = row.Length - 1; c >= 0; c--)
            {
                CheckCurrentVisibility(ref champ, row[c]);
            }
        }

        // top -> bottom
        for (var c = 0; c < grid[0].Length; c++)
        {
            var champ = (char)('0' - 1);
            foreach (var row in grid)
            {
                CheckCurrentVisibility(ref champ, row[c]);
            }
        }

        // bottom -> top
        for (var c = 0; c < grid[0].Length; c++)
        {
            var champ = (char)('0' - 1);
            for (var r = grid.Count - 1; r >= 0; r--)
            {
                CheckCurrentVisibility(ref champ, grid[r][c]);
            }
        }

        return visibleCount.ToString();
    }

    public static async Task<string> SolvePart2Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<GridTree[]> grid = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            grid.Add(line.Select(c => new GridTree(c)).ToArray());
        }

        // Create a graph
        for (int r = 0; r < grid.Count; r++)
        {
            for (int c = 0; c < grid[r].Length; c++)
            {
                var node = grid[r][c];

                if (c > 0)
                {
                    node.Left = grid[r][c - 1];
                }
                if (c + 1 < grid[r].Length)
                {
                    node.Right = grid[r][c + 1];
                }
                if (r > 0)
                {
                    node.Up = grid[r - 1][c];
                }
                if (r+1 < grid.Count)
                {
                    node.Down = grid[r + 1][c];
                }
            }
        }

        return grid.SelectMany(n => n).Max(n => n.GetScenicScore()).ToString();
    }
}
