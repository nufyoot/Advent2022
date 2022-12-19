namespace Advent2022;

public static class Problem12
{
    private class Node
    {
        public Node(char height)
        {
            Height = height;
        }

        public char Height { get; init; }

        public int? DistanceFromStart { get; set; }

        public List<Edge> Edges { get; } = new();
    }

    private class Edge
    {
        public Edge(Node start, Node finish)
        {
            Start = start;
            Finish = finish;
        }

        public Node Start { get; }

        public Node Finish { get; }
    }

    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<Node[]> nodes = new();
        Node start = new Node('a');
        Node finish = new Node('z');

        while (await reader.ReadLineAsync() is { } line)
        {
            nodes.Add(line.Select(c => c switch { 
                'S' => start, 
                'E' => finish, 
                _ => new Node(c)
            }).ToArray());
        }

        for (int r = 0; r < nodes.Count; r++)
        {
            for (int c = 0; c < nodes[r].Length; c++)
            {
                var current = nodes[r][c];

                if (r > 0 && nodes[r - 1][c].Height <= current.Height)
                {
                    current.Edges.Add(new Edge(current, nodes[r - 1][c]));
                }

                if (r + 1 < nodes.Count && nodes[r + 1][c].Height <= current.Height)
                {
                    current.Edges.Add(new Edge(current, nodes[r + 1][c]));
                }

                if (c > 0 && nodes[r][c - 1].Height <= current.Height)
                {
                    current.Edges.Add(new Edge(current, nodes[r][c - 1]));
                }

                if (c + 1 < nodes[r].Length && nodes[r][c + 1].Height <= current.Height)
                {
                    current.Edges.Add(new Edge(current, nodes[r][c + 1]));
                }
            }
        }

        return "";
    }
}
