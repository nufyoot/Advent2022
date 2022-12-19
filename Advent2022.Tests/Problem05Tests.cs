namespace Advent2022.Tests;

public class Problem05Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-05-sample-input.txt", "CMZ")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-05-full-input.txt", "MQTPGLLDN")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem05.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-05-sample-input.txt", "MCD")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-05-full-input.txt", "LVZPSTTCZ")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem05.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}