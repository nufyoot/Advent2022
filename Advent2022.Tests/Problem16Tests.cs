namespace Advent2022.Tests;

public class Problem16Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-16-sample-input.txt", "1651")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-16-full-input.txt", "1653")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem16.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
}