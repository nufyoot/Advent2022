namespace Advent2022.Tests;

public class Problem1Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-1-sample-input.txt", "24000")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-1-full-input.txt", "68467")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem1.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-1-sample-input.txt", "45000")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-1-full-input.txt", "203420")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem1.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}