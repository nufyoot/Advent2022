namespace Advent2022.Tests.Advent2023;

public class Problem03Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-03-sample-input.txt", "4361")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-03-full-input.txt", "531561")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem03.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-03-sample-input.txt", "467835")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-03-full-input.txt", "83279367")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem03.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}