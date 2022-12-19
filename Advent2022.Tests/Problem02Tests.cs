namespace Advent2022.Tests;

public class Problem02Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-02-sample-input.txt", "15")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-02-full-input.txt", "9759")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem02.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-02-sample-input.txt", "12")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-02-full-input.txt", "12429")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem02.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}