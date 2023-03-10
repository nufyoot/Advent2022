namespace Advent2022.Tests;

public class Problem03Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-03-sample-input.txt", "157")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-03-full-input.txt", "7967")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem03.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-03-sample-input.txt", "70")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-03-full-input.txt", "2716")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem03.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}