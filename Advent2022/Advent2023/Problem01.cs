namespace Advent2022.Advent2023;

public static class Problem01
{
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var integers = await GetIntegerList(input, false);
        return integers.Sum().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var integers = await GetIntegerList(input, true);
        return integers.Sum().ToString();
    }

    private static async Task<List<int>> GetIntegerList(Stream input, bool replaceNumberWords)
    {
        using StreamReader reader = new(input);
        var result = new List<int>();

        while (await reader.ReadLineAsync() is { } line)
        {
            var num = 0;
            for (var i = 0; i < line.Length; i++)
            {
                var potential = ToInt(line.AsSpan(i), replaceNumberWords);
                if (potential == 0) continue;
                num += (potential * 10);
                break;
            }
            
            for (var i = line.Length - 1; i >= 0; i--)
            {
                var potential = ToInt(line.AsSpan(i), replaceNumberWords);
                if (potential == 0) continue;
                num += potential;
                break;
            }
            
            result.Add(num);
        }

        return result;
    }

    private static int ToInt(ReadOnlySpan<char> span, bool replaceNumberWords)
    {
        if (span[0] == '1' || (replaceNumberWords && span.StartsWith("one")))
            return 1;
        if (span[0] == '2' || (replaceNumberWords && span.StartsWith("two")))
            return 2;
        if (span[0] == '3' || (replaceNumberWords && span.StartsWith("three")))
            return 3;
        if (span[0] == '4' || (replaceNumberWords && span.StartsWith("four")))
            return 4;
        if (span[0] == '5' || (replaceNumberWords && span.StartsWith("five")))
            return 5;
        if (span[0] == '6' || (replaceNumberWords && span.StartsWith("six")))
            return 6;
        if (span[0] == '7' || (replaceNumberWords && span.StartsWith("seven")))
            return 7;
        if (span[0] == '8' || (replaceNumberWords && span.StartsWith("eight")))
            return 8;
        if (span[0] == '9' || (replaceNumberWords && span.StartsWith("nine")))
            return 9;

        return 0;
    }
}