namespace Advent2022.Tests;

public class Problem12Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-12-sample-input.txt", "10605")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem12.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
}