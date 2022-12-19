namespace Advent2022.Tests;

public class Problem11Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-11-sample-input.txt", "10605")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-11-full-input.txt", "54752")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem11.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }

    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-11-sample-input.txt", "2713310158")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-11-full-input.txt", "13606755504")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem11.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}