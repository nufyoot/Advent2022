using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem04
{
    private class Card(int cardId, List<int> winningNumbers, List<int> ourNumbers)
    {
        public int CardId { get; } = cardId;
        public List<int> WinningNumbers { get; } = winningNumbers;
        public List<int> OurNumbers { get; } = ourNumbers;
        public int Count { get; set; } = 1;
        public IEnumerable<int> Matches => ourNumbers.Intersect(winningNumbers);
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var cards = await ParseCards(input);
        return (from card in cards
                where card.Matches.Any()
                select Math.Pow(2, card.Matches.Count()-1)).Sum().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var cards = await ParseCards(input);
        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var matchCount = card.Matches.Count();
            for (var j = 1; j <= matchCount; j++)
            {
                cards[i + j].Count += card.Count;
            }
        }

        return cards.Sum(c => c.Count).ToString();
    }

    private static async Task<List<Card>> ParseCards(Stream input)
    {
        using StreamReader reader = new(input);
        var cards = new List<Card>();
        var cardPattern = new Regex(@"^Card\s+(?<cardId>\d+):(?<winningNumbers>[0-9\s]+)\|(?<ourNumbers>[0-9\s]+)$");

        while (await reader.ReadLineAsync() is { } line)
        {
            if (cardPattern.Match(line) is { Success: true } match)
            {
                cards.Add(new Card(
                    int.Parse(match.Groups["cardId"].Value), 
                    match.Groups["winningNumbers"].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList(),
                    match.Groups["ourNumbers"].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList()));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        return cards;
    }
}