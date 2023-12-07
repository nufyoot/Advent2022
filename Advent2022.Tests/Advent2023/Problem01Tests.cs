namespace Advent2022.Tests.Advent2023;

public class Problem01Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-01-sample-input.txt", "142")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-01-full-input.txt", "56049")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem01.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-01-sample2-input.txt", "281")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-01-full-input.txt", "54530")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem01.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}