namespace Advent2022.Advent2023;

public static class Problem07
{
    private enum HandStrength
    {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeKind = 3,
        FullHouse = 4,
        FourKind = 5,
        FiveKind = 6,
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var hands = await ReadCards(input);
        return (
            from hand in hands
            let handStrength = GetStrengthPart1(hand.cards)
            orderby handStrength,
                GetCardStrength(hand.cards[0], false),
                GetCardStrength(hand.cards[1], false),
                GetCardStrength(hand.cards[2], false),
                GetCardStrength(hand.cards[3], false),
                GetCardStrength(hand.cards[4], false)
            select hand.bid
        ).Select((b, i) => b * (i + 1)).Sum().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var hands = await ReadCards(input);
        return (
            from hand in hands
            let handStrength = GetStrengthPart2(hand.cards)
            orderby handStrength,
                GetCardStrength(hand.cards[0], true),
                GetCardStrength(hand.cards[1], true),
                GetCardStrength(hand.cards[2], true),
                GetCardStrength(hand.cards[3], true),
                GetCardStrength(hand.cards[4], true)
            select hand.bid
        ).Select((b, i) => b * (i + 1)).Sum().ToString();
    }

    private static async Task<List<(string cards, int bid)>> ReadCards(Stream input)
    {
        using StreamReader reader = new(input);
        var result = new List<(string cards, int bid)>();
        while ((await reader.ReadLineAsync()) is { } line)
        {
            var parts = line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            result.Add((parts[0], int.Parse(parts[1])));
        }

        return result;
    }

    private static HandStrength GetStrengthPart1(string cards)
    {
        var groups = cards.GroupBy(c => c).ToList();

        if (groups.Any(g => g.Count() == 5))
            return HandStrength.FiveKind;
        if (groups.Any(g => g.Count() == 4))
            return HandStrength.FourKind;
        if (groups.Any(g => g.Count() == 3) && groups.Any(g => g.Count() == 2))
            return HandStrength.FullHouse;
        if (groups.Any(g => g.Count() == 3))
            return HandStrength.ThreeKind;
        if (groups.Count(g => g.Count() == 2) == 2)
            return HandStrength.TwoPair;
        if (groups.Any(g => g.Count() == 2))
            return HandStrength.OnePair;

        return HandStrength.HighCard;
    }
    
    private static HandStrength GetStrengthPart2(string cards)
    {
        var distinctCards = cards.Distinct().Except(['J']);
        var champion = HandStrength.HighCard;
        
        foreach (var distinctCard in distinctCards)
        {
            var replacedCards = cards.Replace('J', distinctCard);
            var groups = replacedCards.GroupBy(c => c).ToList();

            if (groups.Any(g => g.Count() == 5))
            {
                if (champion < HandStrength.FiveKind)
                    champion = HandStrength.FiveKind;
            }
            else if (groups.Any(g => g.Count() == 4))
            {
                if (champion < HandStrength.FourKind)
                    champion = HandStrength.FourKind;
            }
            else if (groups.Any(g => g.Count() == 3) && groups.Any(g => g.Count() == 2))
            {
                if (champion < HandStrength.FullHouse)
                    champion = HandStrength.FullHouse;
            }
            else if (groups.Any(g => g.Count() == 3))
            {
                if (champion < HandStrength.ThreeKind)
                    champion = HandStrength.ThreeKind;
            }
            else if (groups.Count(g => g.Count() == 2) == 2)
            {
                if (champion < HandStrength.TwoPair)
                    champion = HandStrength.TwoPair;
            }
            else if (groups.Any(g => g.Count() == 2))
            {
                if (champion < HandStrength.OnePair)
                    champion = HandStrength.OnePair;
            }
        }

        return champion;
    }

    private static int GetCardStrength(char card, bool useJoker)
    {
        return card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => useJoker ? 1 : 11,
            'T' => 10,
            '9' => 9,
            '8' => 8,
            '7' => 7,
            '6' => 6,
            '5' => 5,
            '4' => 4,
            '3' => 3,
            '2' => 2,
            _ => throw new NotImplementedException(),
        };
    }
}