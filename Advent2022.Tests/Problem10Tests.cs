namespace Advent2022.Tests;

public class Problem10Tests
{
    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-10-sample-input.txt", "13140")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-10-full-input.txt", "17020")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem10.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }

    [Theory]
    [EmbeddedResource("Advent2022.Tests.Data.problem-10-sample-input.txt", "##..##..##..##..##..##..##..##..##..##..###...###...###...###...###...###...###.####....####....####....####....####....#####.....#####.....#####.....#####.....######......######......######......###########.......#######.......#######.....")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-10-full-input.txt", "###..#....####.####.####.#.....##..####.#..#.#....#.......#.#....#....#..#.#....#..#.#....###....#..###..#....#....###..###..#....#.....#...#....#....#.##.#....#.#..#....#....#....#....#....#..#.#....#..#.####.####.####.#....####..###.####.")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem10.SolvePart2Async(stream);
        Assert.Equal(expected, solution.Replace("\r", "").Replace("\n", ""));
    }
}