namespace Advent2022.Tests;

public class Problem15Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-15-sample-input.txt", "26", 10)]
    [EmbeddedResource("Advent2022.Tests.Data.problem-15-full-input.txt", "5181556", 2_000_000)]
    public async Task TestPart1(Stream stream, string expected, int rowToCheck)
    {
        var solution = await Problem15.SolvePart1Async(stream, rowToCheck);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-15-sample-input.txt", "26", 0, 0, 20, 20)]
    //[EmbeddedResource("Advent2022.Tests.Data.problem-15-full-input.txt", "5181556", 0, 0, 4_000_000, 4_000_000)]
    public async Task TestPart2(Stream stream, string expected, int minX, int minY, int maxX, int maxY)
    {
        var solution = await Problem15.SolvePart2Async(stream, minX, minY, maxX, maxY);
        Assert.Equal(expected, solution);
    }
}