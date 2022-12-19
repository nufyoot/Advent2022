namespace Advent2022.Tests;

public class Problem06Tests
{
    [Theory]
    [StringResource("mjqjpqmgbljsphdztnvjfqwrcgsmlb", "7")]
    [StringResource("bvwbjplbgvbhsrlpgdmjqwftvncz", "5")]
    [StringResource("nppdvjthqldpwncqszvftbrmjlhg", "6")]
    [StringResource("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", "10")]
    [StringResource("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", "11")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-06-full-input.txt", "1855")]
    public async Task TestPart1(Stream stream, string expected)
    {
        var solution = await Problem06.SolvePart1Async(stream);
        Assert.Equal(expected, solution);
    }
    
    [Theory]
    [StringResource("mjqjpqmgbljsphdztnvjfqwrcgsmlb", "19")]
    [StringResource("bvwbjplbgvbhsrlpgdmjqwftvncz", "23")]
    [StringResource("nppdvjthqldpwncqszvftbrmjlhg", "23")]
    [StringResource("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", "29")]
    [StringResource("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", "26")]
    [EmbeddedResource("Advent2022.Tests.Data.problem-06-full-input.txt", "3256")]
    public async Task TestPart2(Stream stream, string expected)
    {
        var solution = await Problem06.SolvePart2Async(stream);
        Assert.Equal(expected, solution);
    }
}