namespace Advent2022.Tests.Advent2023;

public class Problem02Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-02-sample-input.txt", "8")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-02-full-input.txt", "3035")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem02.SolvePart1Async(stream, new Advent2022.Advent2023.Problem02.CubeCount(12, 13, 14));
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-02-sample-input.txt", "2286")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-02-full-input.txt", "66027")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem02.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}