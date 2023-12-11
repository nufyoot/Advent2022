namespace Advent2022.Tests.Advent2023;

public class Problem09Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-09-sample-input.txt", "114")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-09-full-input.txt", "1708206096")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem09.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-09-sample-input.txt", "2")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-09-full-input.txt", "1050")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem09.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}