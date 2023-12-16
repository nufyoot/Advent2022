namespace Advent2022.Tests.Advent2023;

public class Problem10Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-10-sample-input.txt", "4")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-10-sample2-input.txt", "8")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-10-full-input.txt", "6757")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem10.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-10-sample3-input.txt", "4")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-10-full-input.txt", "523")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem10.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}