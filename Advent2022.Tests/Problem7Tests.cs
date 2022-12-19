namespace Advent2022.Tests;

public class Problem7Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-7-sample-input.txt", "95437")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem7.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
}