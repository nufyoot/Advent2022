namespace Advent2022.Tests;

public class Problem12Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-12-sample-input.txt", "31")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-12-full-input.txt", "361")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem12.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-12-sample-input.txt", "29")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-12-full-input.txt", "354")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem12.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}