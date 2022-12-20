using System.Text.RegularExpressions;

namespace Advent2022;

public static class Problem15
{
    private record struct Sensor(Point Position, int Radius);

    private record struct Point(int X, int Y);
    
    private record struct Bounds(int MinX, int MinY, int MaxX, int MaxY);

    private readonly struct SensorRange
    {
        public readonly int MinX;
        public readonly int MaxX;

        public SensorRange(int minX, int maxX)
        {
            MinX = minX;
            MaxX = maxX;
        }

        public bool Overlaps(SensorRange other)
        {
            return MinX <= other.MaxX && MaxX >= other.MinX;
        }

        public bool Contains(int x)
        {
            return x >= MinX && x <= MaxX;
        }

        public SensorRange Union(SensorRange other)
        {
            return new SensorRange(
                Math.Min(MinX, other.MinX),
                Math.Max(MaxX, other.MaxX));
        }

        public SensorRange Intersect(SensorRange other)
        {
            return new SensorRange(
                Math.Max(MinX, other.MinX),
                Math.Min(MaxX, other.MaxX));
        }

        public int Length => MaxX - MinX + 1;

        public override string ToString()
        {
            return $"{MinX} -> {MaxX}";
        }
    }
    
    public static async Task<string> SolvePart1Async(Stream input, int rowToCheck)
    {
        using StreamReader reader = new(input);
        List<Sensor> sensors = new();
        HashSet<Point> beacons = new();
        Bounds bounds = new Bounds(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
        var pattern =
            new Regex(
                @"^Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)$");

        while (await reader.ReadLineAsync() is { } line)
        {
            if (pattern.Match(line) is { Success: true } match)
            {
                var beacon = new Point(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value));
                var sensorPosition = new Point(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value));
                var radius = ManhattanDistance(beacon, sensorPosition);
                var sensor = new Sensor(sensorPosition, radius);

                bounds = ExpandTo(bounds, sensorPosition);
                bounds = ExpandTo(bounds, beacon);

                sensors.Add(sensor);
                beacons.Add(beacon);
            }
        }
        
        var overlaps = GetSensorRanges(sensors, rowToCheck);
        var overlapCount = overlaps.Sum(o => o.Length);
        var beaconOverlapCount = beacons
            .Where(b => b.Y == rowToCheck)
            .Count(b => overlaps.Any(o => o.Contains(b.X)));
        return (overlapCount - beaconOverlapCount).ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input, int minX, int minY, int maxX, int maxY)
    {
        using StreamReader reader = new(input);
        List<Sensor> sensors = new();
        HashSet<Point> beacons = new();
        Bounds bounds = new Bounds(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
        var pattern =
            new Regex(
                @"^Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)$");

        while (await reader.ReadLineAsync() is { } line)
        {
            if (pattern.Match(line) is { Success: true } match)
            {
                var beacon = new Point(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value));
                var sensorPosition = new Point(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value));
                var radius = ManhattanDistance(beacon, sensorPosition);
                var sensor = new Sensor(sensorPosition, radius);

                bounds = ExpandTo(bounds, sensorPosition);
                bounds = ExpandTo(bounds, beacon);

                sensors.Add(sensor);
                beacons.Add(beacon);
            }
        }

        var distressSignal = FindDistressSignal(minY, maxY, sensors);
        
        return "";
    }

    private static Point FindDistressSignal(int minY, int maxY, List<Sensor> sensors)
    {
        for (var rowToCheck = minY; rowToCheck <= maxY; rowToCheck++)
        {
            var overlaps = GetSensorRanges(sensors, rowToCheck);
            switch (overlaps.Count)
            {
                case 0:
                case > 2:
                    throw new NotImplementedException();
                case 1:
                    continue;
                case 2:
                    return new Point(
                        Math.Max(overlaps[0].MinX, overlaps[1].MinX),
                        rowToCheck);
                    break;
            }
        }

        throw new NotImplementedException();
    }

    private static List<SensorRange> GetSensorRanges(List<Sensor> sensors, int y)
    {
        List<SensorRange> ranges = new();
        foreach (var sensor in sensors)
        {
            var dx = sensor.Radius - Math.Abs(sensor.Position.Y - y);
            if (dx >= 0)
            {
                var newRange = new SensorRange(sensor.Position.X - dx, sensor.Position.X + dx);
                for (int i = 0; i < ranges.Count; i++)
                {
                    if (ranges[i].Overlaps(newRange))
                    {
                        newRange = newRange.Intersect(ranges[i]);
                        ranges.RemoveAt(i--);
                    }
                }
                ranges.Add(newRange);
            }
        }

        return ranges;
    }

    private static int ManhattanDistance(Point p1, Point p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }

    private static Bounds ExpandTo(Bounds bounds, Point point)
    {
        return new Bounds(
            Math.Min(bounds.MinX, point.X),
            Math.Min(bounds.MinY, point.Y),
            Math.Max(bounds.MaxX, point.X),
            Math.Max(bounds.MaxY, point.Y));
    }
}