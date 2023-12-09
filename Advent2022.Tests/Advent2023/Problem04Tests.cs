namespace Advent2022.Tests.Advent2023;

public class Problem04Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-04-sample-input.txt", "13")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-04-full-input.txt", "23235")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem04.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-04-sample-input.txt", "30")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-04-full-input.txt", "5920640")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem04.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}