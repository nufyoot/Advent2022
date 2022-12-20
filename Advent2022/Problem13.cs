namespace Advent2022;

public static class Problem13
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<int> correctIndexes = new();
        var index = 0;

        while (true)
        {
            var packet1 = await reader.ReadLineAsync();
            var packet2 = await reader.ReadLineAsync();
            await reader.ReadLineAsync();

            index++;

            if (packet1 is null || packet2 is null)
            {
                break;
            }

            var input1 = Parse(packet1);
            var input2 = Parse(packet2);

            var packetComparison = ComparePackets(input1, input2);
            if (packetComparison < 1)
            {
                correctIndexes.Add(index);
            }
        }

        return correctIndexes.Sum().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<List<object>> packets = new();

        while (true)
        {
            var packet1 = await reader.ReadLineAsync();
            var packet2 = await reader.ReadLineAsync();
            await reader.ReadLineAsync();

            if (packet1 is null || packet2 is null)
            {
                break;
            }
            
            packets.Add(Parse(packet1));
            packets.Add(Parse(packet2));
        }

        var distressPackets = new List<List<object>>
        {
            Parse("[[2]]"),
            Parse("[[6]]"),
        };
        packets.AddRange(distressPackets);
        
        packets.Sort(ComparePackets);

        return ((packets.IndexOf(distressPackets[0]) + 1) * (packets.IndexOf(distressPackets[1]) + 1)).ToString();
    }

    private static int ComparePackets(object? left, object? right)
    {
        while (true)
        {
            if (left is null && right is not null)
            {
                return -1;
            }

            if (left is not null && right is null)
            {
                return 1;
            }

            var leftList = left as List<object>;
            var rightList = right as List<object>;
            var leftInt = left as int?;
            var rightInt = right as int?;

            if (leftList is not null && rightList is not null)
            {
                var count = Math.Max(leftList.Count, rightList.Count);
                for (var i = 0; i < count; i++)
                {
                    var leftElement = leftList.ElementAtOrDefault(i);
                    var rightElement = rightList.ElementAtOrDefault(i);

                    var packetCompare = ComparePackets(leftElement, rightElement);
                    if (packetCompare != 0)
                    {
                        return packetCompare;
                    }
                }
            }
            else if (leftInt is not null && rightInt is not null)
            {
                return leftInt?.CompareTo(rightInt) ?? throw new NotImplementedException();
            }
            else if (leftInt is not null && rightList is not null)
            {
                left = new List<object> { leftInt };
                right = rightList;
                continue;
            }
            else if (leftList is not null && rightInt is not null)
            {
                left = leftList;
                right = new List<object> { rightInt };
                continue;
            }
            else
            {
                throw new NotImplementedException();
            }

            return 0;
        }
    }

    public static List<object> Parse(ReadOnlySpan<char> input)
    {
        List<object>? current = new();
        Stack<List<object>> stack = new();
        stack.Push(current);
        
        while (input.Length > 0)
        {
            if (input[0] == '[')
            {
                input = input[1..];
                List<object> newList = new();
                current?.Add(newList);
                stack.Push(current ?? throw new NotImplementedException());
                current = newList;
            }
            else if (input[0] == ']')
            {
                input = input[1..];
                current = stack.Pop();
            }
            else if (input[0] == ',')
            {
                input = input[1..];
            }
            else
            {
                var commaPos = input.IndexOfAny(',', ']');
                var number = int.Parse(input[..commaPos]);
                input = input.Slice(commaPos);
                current?.Add(number);
            }
        }

        return (List<object>?)current?.FirstOrDefault() ?? throw new NotImplementedException();
    }
}
