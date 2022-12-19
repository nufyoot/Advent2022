using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem05
{
    private class CraneInstruction
    {
        private readonly static Regex Pattern =
            new Regex(@"^move (?<quantity>\d+) from (?<source>\d+) to (?<destination>\d+)$");

        public int Quantity { get; init; }
        public int Source { get; init; }
        public int Destination { get; init; }

        public static CraneInstruction Parse(string input)
        {
            if (Pattern.Match(input) is { Success: true } match)
            {
                return new CraneInstruction
                {
                    Quantity = int.Parse(match.Groups["quantity"].Value),
                    Source = int.Parse(match.Groups["source"].Value),
                    Destination = int.Parse(match.Groups["destination"].Value),
                };
            }

            throw new NotImplementedException();
        }
    }

    public static async Task<string> SolvePart1Async(Stream input)
    {
        var (instructions, stacks) = await CreateCraneInstructions(input);

        // Perform the instructions
        foreach (var instruction in instructions)
        {
            for (int i = 0; i < instruction.Quantity; i++)
            {
                var val = stacks[instruction.Source - 1].Pop();
                stacks[instruction.Destination - 1].Push(val);
            }
        }

        return new string(stacks.Select(s => s.Pop()).ToArray());
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var (instructions, stacks) = await CreateCraneInstructions(input);

        // Perform the instructions
        foreach (var instruction in instructions)
        {
            Stack<char> tempStack = new();
            for (int i = 0; i < instruction.Quantity; i++)
            {
                var val = stacks[instruction.Source - 1].Pop();
                tempStack.Push(val);
            }

            while (tempStack.Count > 0)
            {
                stacks[instruction.Destination -1].Push(tempStack.Pop());
            }
        }

        return new string(stacks.Select(s => s.Pop()).ToArray());
    }

    private static async Task<(List<CraneInstruction> instructions, Stack<char>[] stacks)> CreateCraneInstructions(Stream input)
    {
        using StreamReader reader = new(input);
        List<string> rows = new();
        List<CraneInstruction> instructions = new();

        // Start by reading the head section
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.Length == 0)
            {
                break;
            }

            rows.Add(line);
        }

        // Parse the instructions
        while (await reader.ReadLineAsync() is { } line)
        {
            instructions.Add(CraneInstruction.Parse(line));
        }

        // Initial setup of the piles.
        var indexes = rows.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var stacks = Enumerable.Range(0, indexes.Length).Select(_ => new Stack<char>()).ToArray();
        foreach (var row in rows.AsEnumerable().Reverse().Skip(1))
        {
            for (int col = 0; col < stacks.Length; col++)
            {
                var val = row[(col * 4) + 1];
                if (val != ' ')
                {
                    stacks[col].Push(val);
                }
            }
        }

        return (instructions, stacks);
    }
}