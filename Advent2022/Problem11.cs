using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem11
{
    private class Monkey
    {
        public Queue<long> Items { get; } = new();

        public Func<long, long> InspectOperation { get; set; } = _ => throw new NotImplementedException();

        public long TestDivisor { get; set; } = 0;

        public long TestTrueMonkeyIndex { get; set; } = 0;

        public long TestFalseMonkeyIndex { get; set; } = 0;

        public long InspectionCount { get; set; } = 0;
    }


    private static readonly Regex TestPattern = new(@"^  Test: divisible by (?<divisor>.+)$");
    private static readonly Regex ThrowToMonkeyPattern = new(@"^    If (?:true|false): throw to monkey (?<monkeyIndex>\d+)$");

    public static async Task<string> SolvePart1Async(Stream input)
    {
        return await PlayMonkeyGame(input, 3, 20);
    }

    public static async Task<string> SolvePart2Async(Stream input)
    {
        return await PlayMonkeyGame(input, 1, 10_000);
    }

    public static async Task<string> PlayMonkeyGame(Stream input, long worryReducer, int roundCount)
    {
        var monkeys = await ParseMonkeysAsync(input);

        // Keep the monkey numbers sane.
        var monkeyDivisor = monkeys.Select(m => m.TestDivisor).Aggregate((a, b) => a * b);

        for (int round = 0; round < roundCount; round++)
        {
            for (int m = 0; m < monkeys.Length; m++)
            {
                var monkey = monkeys[m];

                while (monkey.Items.Count > 0)
                {
                    var item = monkey.Items.Dequeue();
                    item = (monkey.InspectOperation(item) / worryReducer) % monkeyDivisor;

                    var isDivisable = item % monkey.TestDivisor == 0;
                    monkeys[isDivisable ? monkey.TestTrueMonkeyIndex : monkey.TestFalseMonkeyIndex].Items.Enqueue(item);
                    monkey.InspectionCount++;
                }
            }
        }

        return monkeys
            .OrderByDescending(m => m.InspectionCount)
            .Take(2)
            .Select(m => m.InspectionCount)
            .Aggregate((a, b) => a * b)
            .ToString();
    }

    private static async Task<Monkey[]> ParseMonkeysAsync(Stream input)
    {

        using StreamReader reader = new(input);
        List<Monkey> monkeys = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            if (line == String.Empty)
            {
                line = await reader.ReadLineAsync() ?? throw new NotImplementedException();
            }

            var monkey = new Monkey();

            var startingItemsLine = await reader.ReadLineAsync() ?? throw new NotImplementedException();
            var startingItems = startingItemsLine.Substring("  Starting items: ".Length).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in startingItems)
            {
                monkey.Items.Enqueue(long.Parse(item));
            }

            var operationLine = await reader.ReadLineAsync() ?? throw new NotImplementedException();
            monkey.InspectOperation = CompileInspectionOperation(operationLine);

            var testLine = await reader.ReadLineAsync() ?? throw new NotImplementedException();
            if (TestPattern.Match(testLine) is { Success: true } match)
            {
                monkey.TestDivisor = long.Parse(match.Groups["divisor"].Value);
            }
            else
            {
                throw new NotImplementedException();
            }

            var trueMonkeyLine = await reader.ReadLineAsync() ?? throw new NotImplementedException();
            if (ThrowToMonkeyPattern.Match(trueMonkeyLine) is { Success: true } trueMonkeyMatch)
            {
                monkey.TestTrueMonkeyIndex = long.Parse(trueMonkeyMatch.Groups["monkeyIndex"].Value);
            }
            else
            {
                throw new NotImplementedException();
            }

            var falseMonkeyLine = await reader.ReadLineAsync() ?? throw new NotImplementedException();
            if (ThrowToMonkeyPattern.Match(falseMonkeyLine) is { Success: true } falseMonkeyMatch)
            {
                monkey.TestFalseMonkeyIndex = long.Parse(falseMonkeyMatch.Groups["monkeyIndex"].Value);
            }
            else
            {
                throw new NotImplementedException();
            }

            monkeys.Add(monkey);
        }

        return monkeys.ToArray();
    }

    private static Func<long, long> CompileInspectionOperation(string input)
    {
        var operationPattern = new Regex(@"^  Operation: new = (?<p1>.+) (?<op>[\+\*]) (?<p2>.+)$");
        if (operationPattern.Match(input) is { Success: true } match)
        {
            var p1 = match.Groups["p1"].Value;
            var p2 = match.Groups["p2"].Value;
            var op = match.Groups["op"].Value;

            var old = Expression.Parameter(typeof(long));
            Expression p1Expression = p1 == "old" ? old : Expression.Constant(long.Parse(p1));
            Expression p2Expression = p2 == "old" ? old : Expression.Constant(long.Parse(p2));

            var expression =
                Expression.Lambda<Func<long, long>>(
                    Expression.MakeBinary(
                        op switch
                        {
                            "*" => ExpressionType.Multiply,
                            "+" => ExpressionType.Add,
                            _ => throw new NotImplementedException()
                        },
                        p1Expression,
                        p2Expression),
                    old);
            return expression.Compile();
        }

        throw new NotImplementedException();
    }
}
