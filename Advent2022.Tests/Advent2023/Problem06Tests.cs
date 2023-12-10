namespace Advent2022.Tests.Advent2023;

public class Problem06Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-06-sample-input.txt", "288")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-06-full-input.txt", "32076")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem06.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-06-sample-input.txt", "71503")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-06-full-input.txt", "34278221")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem06.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}