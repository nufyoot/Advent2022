namespace Advent2022.Tests.Advent2023;

public class Problem08Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-08-sample-input.txt", "2")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-08-sample2-input.txt", "6")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-08-full-input.txt", "16343")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem08.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-08-sample3-input.txt", "6")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-08-full-input.txt", "15299095336639")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem08.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}