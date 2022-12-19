namespace Advent2022.Tests;

public class Problem09Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-09-sample-input.txt", "13")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-09-full-input.txt", "6339")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem09.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }

    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-09-sample-input.txt", "1")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-09-sample2-input.txt", "36")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-09-full-input.txt", "2541")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem09.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}