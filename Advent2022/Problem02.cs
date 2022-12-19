namespace Advent2022;

public static class Problem02
{
    private enum Choice
    {
        Rock,
        Paper,
        Scissors,
    }

    private class Round
    {
        public Choice OpponentChoice { get; init; }
        public Choice OurChoice { get; init; }

        public int OurChoiceScore => OurChoice switch
        {
            Choice.Rock => 1,
            Choice.Paper => 2,
            Choice.Scissors => 3,
            _ => throw new NotImplementedException(),
        };

        public int OurOutcomeScore => (OurChoice, OpponentChoice) switch
        {
            /* Loss */
            (Choice.Rock, Choice.Paper) => 0,
            (Choice.Paper, Choice.Scissors) => 0,
            (Choice.Scissors, Choice.Rock) => 0,

            /* Draw */
            (Choice.Rock, Choice.Rock) => 3,
            (Choice.Paper, Choice.Paper) => 3,
            (Choice.Scissors, Choice.Scissors) => 3,

            /* Win */
            (Choice.Rock, Choice.Scissors) => 6,
            (Choice.Paper, Choice.Rock) => 6,
            (Choice.Scissors, Choice.Paper) => 6,

            _ => throw new NotImplementedException(),
        };

        public int OurScore => OurChoiceScore + OurOutcomeScore;

        public static Round ParsePart1(string input)
        {
            var opponentChoice = input[0] switch
            {
                'A' => Choice.Rock,
                'B' => Choice.Paper,
                'C' => Choice.Scissors,
                _ => throw new NotImplementedException(),
            };

            var ourChoice = input[2] switch
            {
                'X' => Choice.Rock,
                'Y' => Choice.Paper,
                'Z' => Choice.Scissors,
                _ => throw new NotImplementedException(),
            };

            return new Round { OurChoice = ourChoice, OpponentChoice = opponentChoice };
        }
        
        public static Round ParsePart2(string input)
        {
            var opponentChoice = input[0] switch
            {
                'A' => Choice.Rock,
                'B' => Choice.Paper,
                'C' => Choice.Scissors,
                _ => throw new NotImplementedException(),
            };

            var ourChoice = input[2] switch
            {
                'X' => opponentChoice switch
                {
                    Choice.Rock => Choice.Scissors,
                    Choice.Paper => Choice.Rock,
                    Choice.Scissors => Choice.Paper,
                    _ => throw new NotImplementedException(),
                },
                'Y' => opponentChoice,
                'Z' => opponentChoice switch
                {
                    Choice.Rock => Choice.Paper,
                    Choice.Paper => Choice.Scissors,
                    Choice.Scissors => Choice.Rock,
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };

            return new Round { OurChoice = ourChoice, OpponentChoice = opponentChoice };
        }
    }

    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<Round> rounds = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            rounds.Add(Round.ParsePart1(line));
        }

        return rounds.Sum(r => r.OurScore).ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        using StreamReader reader = new(input);
        List<Round> rounds = new();

        while (await reader.ReadLineAsync() is { } line)
        {
            rounds.Add(Round.ParsePart2(line));
        }

        return rounds.Sum(r => r.OurScore).ToString();
    }
}