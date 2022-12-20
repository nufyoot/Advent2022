using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem07
{
    private class DiskNode
    {
        public DiskNode(DiskNode? parent, string name, int? size)
        {
            Parent = parent;
            Name = name;
            Size = size;
        }
        public DiskNode? Parent { get; init; }

        public List<DiskNode> Children { get; } = new();

        public string Name { get; init; }
        
        public int? Size { get; init; }

        public bool IsDirectory => Size is null;

        public int GetTotalSize() => Size ?? Children.Sum(n => n.GetTotalSize());
    }

    private class Process
    {
        public Process(string command, string? argument)
        {
            Command = command;
            Argument = argument;
        }

        public string Command { get; init; }

        public string? Argument { get; init; }

        public List<string> ResponseLines { get; } = new();

        public static Process ParseCommand(string input)
        {
            if (!input.StartsWith('$'))
            {
                throw new ArgumentException($"Expected argument to start with '$' but instead was '{input}'", nameof(input));
            }

            var parts = input[2..].Split(' ', 2);
            return new Process(parts[0], parts.Length >= 2 ? parts[1] : null);
        }
    }
    
    private static readonly Regex DirectoryListingPattern = new Regex(@"^(?<size>dir|\d+) (?<name>[a-zA-Z\.]+)$");

    public static async Task<string> SolvePart1Async(Stream input)
    {
        var rootNode = await ParseInputIntoRootNode(input);

        // Find the entries 
        const int maxSize = 100_000;
        Queue<DiskNode> pendingVisits = new();
        int total = 0;

        pendingVisits.Enqueue(rootNode);
        

        while (pendingVisits.Count > 0)
        {
            var node = pendingVisits.Dequeue();
            var totalSize = node.GetTotalSize();

            if (totalSize <= maxSize)
            {
                total += totalSize;
            }

            foreach (var childNode in node.Children.Where(d => d.IsDirectory))
            {
                pendingVisits.Enqueue(childNode);
            }
        }

        return total.ToString();
    }

    public static async Task<string> SolvePart2Async(Stream input)
    {
        var rootNode = await ParseInputIntoRootNode(input);

        // Find the entries 
        Queue<DiskNode> pendingVisits = new();
        int candidateSize = int.MaxValue;
        int used = rootNode.GetTotalSize();
        int minToFree = used - 40_000_000;

        pendingVisits.Enqueue(rootNode);

        while (pendingVisits.Count > 0)
        {
            var node = pendingVisits.Dequeue();
            var totalSize = node.GetTotalSize();

            if (totalSize >= minToFree && totalSize < candidateSize)
            {
                candidateSize = totalSize;
            }

            foreach (var childNode in node.Children.Where(d => d.IsDirectory))
            {
                pendingVisits.Enqueue(childNode);
            }
        }

        return candidateSize.ToString();
    }

    private static async Task<DiskNode> ParseInputIntoRootNode(Stream input)
    {
        using StreamReader reader = new(input);
        List<Process> processes = new();

        // Process the input
        Process? currentCommand = null;
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.StartsWith('$'))
            {
                processes.Add(currentCommand = Process.ParseCommand(line));
            }
            else if (currentCommand is not null)
            {
                currentCommand.ResponseLines.Add(line);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Solve the problem
        DiskNode rootNode = new DiskNode(null, "", null);
        DiskNode? currentNode = null;
        foreach (var process in processes)
        {
            if (process.Command == "cd")
            {
                if (process.Argument == "/")
                {
                    currentNode = rootNode;
                }
                else if (process.Argument == "..")
                {
                    currentNode = currentNode?.Parent;
                    if (currentNode is null)
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    if (currentNode is null)
                    {
                        throw new NotImplementedException();
                    }

                    currentNode = currentNode.Children.Single(n => n.Name == process.Argument);
                }
            }
            else if (process.Command == "ls")
            {
                if (currentNode is null)
                {
                    throw new NotImplementedException();
                }

                foreach (var responseLine in process.ResponseLines)
                {
                    if (DirectoryListingPattern.Match(responseLine) is { Success: true } match)
                    {
                        int? size = null;
                        if (match.Groups["size"].Value == "dir")
                        {
                            size = null;
                        }
                        else if (int.TryParse(match.Groups["size"].Value, out int lineSize))
                        {
                            size = lineSize;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        currentNode.Children.Add(new DiskNode(currentNode, match.Groups["name"].Value, size));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        return rootNode;
    }
}