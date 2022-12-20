namespace Advent2022.Tests;

public class Problem14Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-14-sample-input.txt", "24")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-14-full-input.txt", "683")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem14.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-14-sample-input.txt", "93")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-14-full-input.txt", "28821")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem14.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}