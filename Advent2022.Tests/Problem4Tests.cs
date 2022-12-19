namespace Advent2022.Tests;

public class Problem4Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-4-sample-input.txt", "2")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-4-full-input.txt", "532")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem4.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-4-sample-input.txt", "4")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-4-full-input.txt", "854")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem4.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}