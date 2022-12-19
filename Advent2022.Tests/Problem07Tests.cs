namespace Advent2022.Tests;

public class Problem07Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-07-sample-input.txt", "95437")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-07-full-input.txt", "1581595")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem07.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }

    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-07-sample-input.txt", "24933642")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-07-full-input.txt", "1544176")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem07.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}