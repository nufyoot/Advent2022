namespace Advent2022;

public static class Problem04
{
    private class SectionRange
    {
        public int Start { get; init; }
        public int Finish { get; init; }

        public bool FullyContains(SectionRange otherRange) =>
            this.Start <= otherRange.Start && this.Finish >= otherRange.Finish;

        public bool Overlaps(SectionRange otherRange) =>
            this.Start <= otherRange.Finish && this.Finish >= otherRange.Start;

        public static SectionRange Parse(string range)
        {
            var parts = range.Split('-', 2);
            if (int.TryParse(parts[0], out var start) && int.TryParse(parts[1], out var finish))
            {
                return new SectionRange { Start = start, Finish = finish };
            }

            throw new NotImplementedException();
        }
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        using StreamReader reader = new(input);
        int totalOverlaps = 0;

        while (await reader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(',', 2);
            var sectionRange1 = SectionRange.Parse(parts[0]);
            var sectionRange2 = SectionRange.Parse(parts[1]);

            if (sectionRange1.FullyContains(sectionRange2) || sectionRange2.FullyContains(sectionRange1))
            {
                totalOverlaps++;
            }
        }

        return totalOverlaps.ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        using StreamReader reader = new(input);
        int totalOverlaps = 0;

        while (await reader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(',', 2);
            var sectionRange1 = SectionRange.Parse(parts[0]);
            var sectionRange2 = SectionRange.Parse(parts[1]);

            if (sectionRange1.Overlaps(sectionRange2))
            {
                totalOverlaps++;
            }
        }

        return totalOverlaps.ToString();
    }
}