namespace Advent2022.Tests.Advent2023;

public class Problem11Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-11-sample-input.txt", "374")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-11-full-input.txt", "9681886")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem11.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-11-sample-input.txt", "1030", 10)]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-11-sample-input.txt", "8410", 100)]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-11-full-input.txt", "791134099634", 1_000_000)]
    public async Task TestPart2(Stream stream, string expected, int expansionRate)
    {
        var solution = await Advent2022.Advent2023.Problem11.SolvePart2Async(stream, expansionRate);
        Assert.Equal(expected, solution);
    }
}