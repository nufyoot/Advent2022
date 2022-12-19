using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem7
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
    }
    
    private static readonly Regex DirectoryListingPattern = new Regex(@"^(?<size>dir|\d+) (?<name>[a-zA-Z\.]+)$");
    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        var rootNode = new DiskNode(null, "", null);
        var currentNode = rootNode;

        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.StartsWith("$"))
            {
                if (line.StartsWith("$ cd"))
                {
                    if (line == "$ cd /")
                    {
                        currentNode = rootNode;
                    }
                    else if (line == "$ cd ..")
                    {
                        currentNode = currentNode.Parent;
                        if (currentNode == null)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        var path = line.Substring(5);
                        var childNode = currentNode.Children.FirstOrDefault(n => n.Name == path);
                        if (childNode == null)
                        {
                            throw new NotImplementedException();
                        }

                        currentNode = childNode;
                    }

                    continue;
                }
                
                if (line == "$ ls")
                {
                    continue;
                }

                throw new NotImplementedException();
            }
            else
            {
                if (DirectoryListingPattern.Match(line) is { } match)
                {
                    var sizeString = match.Groups["size"].Value;
                    int? size = null;
                    if (sizeString != "dir" && int.TryParse(sizeString, out var lineSize))
                    {
                        size = lineSize;
                    }

                    currentNode.Children.Add(new DiskNode(currentNode, match.Groups["name"].Value, size));
                }
            }
        }

        return "";
    }
}