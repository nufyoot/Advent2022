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

        public override string ToString()
        {
            return $"Height: {Height}, Distance: {DistanceFromStart}";
        }
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

        public override string ToString()
        {
            return $"{Start.ToString()} -> {Finish.ToString()}";
        }
    }

    private static async Task<(List<Node[]> nodes, Node start, Node finish)> BuildGrid(Stream input)
    {
        using StreamReader reader = new(input);
        List<Node[]> nodes = new();
        Node start = new('a');
        Node finish = new('z');

        start.DistanceFromStart = 0;

        while (await reader.ReadLineAsync() is { } line)
        {
            nodes.Add(line.Select(c => c switch { 
                'S' => start, 
                'E' => finish, 
                _ => new Node(c)
            }).ToArray());
        }

        // Build the graph
        for (var r = 0; r < nodes.Count; r++)
        {
            for (var c = 0; c < nodes[r].Length; c++)
            {
                var current = nodes[r][c];

                if (r > 0 && nodes[r - 1][c].Height - current.Height <= 1)
                {
                    current.Edges.Add(new Edge(current, nodes[r - 1][c]));
                }

                if (r + 1 < nodes.Count && nodes[r + 1][c].Height - current.Height <= 1)
                {
                    current.Edges.Add(new Edge(current, nodes[r + 1][c]));
                }

                if (c > 0 && nodes[r][c - 1].Height - current.Height <= 1)
                {
                    current.Edges.Add(new Edge(current, nodes[r][c - 1]));
                }

                if (c + 1 < nodes[r].Length && nodes[r][c + 1].Height - current.Height <= 1)
                {
                    current.Edges.Add(new Edge(current, nodes[r][c + 1]));
                }
            }
        }

        return (nodes, start, finish);
    }

    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (nodes, start, finish) = await BuildGrid(input);
        Queue<Node> nodesToVisit = new();
        nodesToVisit.Enqueue(start);

        while (nodesToVisit.Count > 0)
        {
            var current = nodesToVisit.Dequeue();

            foreach (var otherNode in current.Edges.Select(e => e.Finish))
            {
                if (otherNode.DistanceFromStart == null || otherNode.DistanceFromStart > current.DistanceFromStart + 1)
                {
                    otherNode.DistanceFromStart = current.DistanceFromStart + 1;
                    nodesToVisit.Enqueue(otherNode);
                }
            }
        }

        return finish.DistanceFromStart.GetValueOrDefault().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var (nodes, _, finish) = await BuildGrid(input);
        Queue<Node> nodesToVisit = new();

        foreach (var node in nodes.SelectMany(n => n).Where(n => n.Height == 'a'))
        {
            node.DistanceFromStart = 0;
            nodesToVisit.Enqueue(node);
        }

        while (nodesToVisit.Count > 0)
        {
            var current = nodesToVisit.Dequeue();

            foreach (var otherNode in current.Edges.Select(e => e.Finish))
            {
                if (otherNode.DistanceFromStart == null || otherNode.DistanceFromStart > current.DistanceFromStart + 1)
                {
                    otherNode.DistanceFromStart = current.DistanceFromStart + 1;
                    nodesToVisit.Enqueue(otherNode);
                }
            }
        }

        return finish.DistanceFromStart.GetValueOrDefault().ToString();
    }
}
