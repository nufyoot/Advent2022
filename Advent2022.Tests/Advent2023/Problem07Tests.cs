namespace Advent2022.Tests.Advent2023;

public class Problem07Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-07-sample-input.txt", "6440")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-07-full-input.txt", "248396258")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem07.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-07-sample-input.txt", "5905")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-07-full-input.txt", "248396258")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem07.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}