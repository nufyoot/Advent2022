namespace Advent2022.Tests;

public class Problem08Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-08-sample-input.txt", "21")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-08-full-input.txt", "1705")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem08.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }

    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-08-sample-input.txt", "8")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-08-full-input.txt", "371200")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem08.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}