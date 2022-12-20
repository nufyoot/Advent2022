namespace Advent2022.Tests;

public class Problem13Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-13-sample-input.txt", "13")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-13-full-input.txt", "6235")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem13.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-13-sample-input.txt", "140")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-13-full-input.txt", "22866")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem13.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}