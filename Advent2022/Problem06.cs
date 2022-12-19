namespace Advent2022;

public static class Problem06
{
    public static async Task<string> SolvePart1Async(Stream input)
    {
        return await GetFirstUniquePacket(input, 4);
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        return await GetFirstUniquePacket(input, 14);
    }

    public static async Task<string> GetFirstUniquePacket(Stream input, int packetSize)
    {
        using StreamReader reader = new(input);
        var value = await reader.ReadToEndAsync();
        var seen = new HashSet<char>();
        
        for (var i = 0; i <= value.Length - packetSize; i++)
        {
            if (value.Substring(i, packetSize).All(v => seen.Add(v)))
            {
                return (i + packetSize).ToString();
            }
            
            seen.Clear();
        }

        throw new NotImplementedException();
    }
}