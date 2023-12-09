namespace Advent2022.Tests.Advent2023;

public class Problem05Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-05-sample-input.txt", "35")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-05-full-input.txt", "579439039")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem05.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-05-sample-input.txt", "46")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-05-full-input.txt", "7873084")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem05.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}