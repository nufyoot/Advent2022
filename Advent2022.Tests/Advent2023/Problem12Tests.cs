namespace Advent2022.Tests.Advent2023;

public class Problem12Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-12-sample-input.txt", "21")]
    [EmbeddedResource("Advent2022.Tests.Advent2023.Data.problem-12-full-input.txt", "<7669")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Advent2022.Advent2023.Problem12.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
}