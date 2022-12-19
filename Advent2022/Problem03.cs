namespace Advent2022;

public static class Problem03
{
    private class Rucksack
    {
        public Rucksack(string contents)
        {
            Contents = contents;
        }
        public string Contents { get; init; }

        public static int CalculatePriority(char item) => item switch
        {
            >= 'a' and <= 'z' => item - 'a' + 1,
            >= 'A' and <= 'Z' => item - 'A' + 27,
            _ => throw new NotImplementedException(),
        };

        public int CalculateProblem1Priority()
        {
            var halfLength = Contents.Length / 2;
            HashSet<char> compartment1Items = new(Contents[0..halfLength]);
            HashSet<char> compartment2Items = new(Contents[halfLength..]);
            HashSet<char> intersection = IntersectHashSets(compartment1Items, compartment2Items);

            return CalculatePriority(intersection.Single());
        }

        private static HashSet<T> IntersectHashSets<T>(HashSet<T> set1, HashSet<T> set2)
        {
            HashSet<T> result = new(set1);
            result.IntersectWith(set2);
            return result;
        }
    }

    private class RucksackGroup
    {
        public RucksackGroup(Rucksack rucksack1, Rucksack rucksack2, Rucksack rucksack3)
        {
            Rucksack1 = rucksack1;
            Rucksack2 = rucksack2;
            Rucksack3 = rucksack3;
        }
        
        public Rucksack Rucksack1 { get; init; }
        public Rucksack Rucksack2 { get; init; }
        public Rucksack Rucksack3 { get; init; }

        public int CalculatePart2Priority()
        {
            HashSet<char> rucksack1Contents = new(Rucksack1.Contents);
            HashSet<char> rucksack2Contents = new(Rucksack2.Contents);
            HashSet<char> rucksack3Contents = new(Rucksack3.Contents);
            HashSet<char> result = new(rucksack1Contents);

            result.IntersectWith(rucksack2Contents);
            result.IntersectWith(rucksack3Contents);

            return Rucksack.CalculatePriority(result.Single());
        }
    }

    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<Rucksack> rucksacks = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            rucksacks.Add(new Rucksack(line));
        }

        return rucksacks.Sum(r => r.CalculateProblem1Priority()).ToString();
    }

    public static async Task<string> SolvePart2Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<RucksackGroup> rucksackGroups = new();

        while (await reader.ReadLineAsync() is { } line1 &&
               await reader.ReadLineAsync() is { } line2 &&
               await reader.ReadLineAsync() is { } line3)
        {
            rucksackGroups.Add(
                new RucksackGroup(
                    new Rucksack(line1), 
                    new Rucksack(line2),
                    new Rucksack(line3)));
        }

        return rucksackGroups.Sum(r => r.CalculatePart2Priority()).ToString();
    }
}