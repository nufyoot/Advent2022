using System.Text.RegularExpressions;

namespace Advent2022.Advent2023;

public static class Problem03
{
    private class Symbol(char value, int x, int y )
    {
        public char Value { get; set; } = value;
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }

    private class Number(int value, int left, int right, int top, int bottom)
    {
        public int Value { get; set; } = value;
        public int Left { get; set; } = left;
        public int Right { get; set; } = right;
        public int Top { get; set; } = top;
        public int Bottom { get; set; } = bottom;

        public bool Overlaps(Symbol symbol)
        {
            return symbol.X >= Left && symbol.X <= Right && symbol.Y >= Top && symbol.Y <= Bottom;
        }
    }

    private class Grid
    {
        public Grid(List<Symbol> symbols, List<Number> numbers)
        {
            Symbols = symbols;
            Numbers = numbers;
        }
        
        public List<Symbol> Symbols { get; set; }
        public List<Number> Numbers { get; set; }
    }
    
    public static async Task<string> SolvePart1Async(Stream input)
    {
        var grid = await ParseGrid(input);
        var numbersTouchingSymbols = grid.Numbers.Where(n => grid.Symbols.Any(n.Overlaps));
        return numbersTouchingSymbols.Select(n => n.Value).Sum().ToString();
    }
    
    public static async Task<string> SolvePart2Async(Stream input)
    {
        var grid = await ParseGrid(input);
        var starSymbolsTouching = grid.Symbols.Select(s => new { Symbol = s, Numbers = grid.Numbers.Where(n => n.Overlaps(s)) });
        return starSymbolsTouching.Where(s => s.Numbers.Count() == 2).Select(s => s.Numbers.ElementAt(0).Value * s.Numbers.ElementAt(1).Value).Sum().ToString();
    }

    private static async Task<Grid> ParseGrid(Stream input)
    {
        using StreamReader reader = new(input);
        var grid = new List<string>();

        while (await reader.ReadLineAsync() is { } line)
        {
            grid.Add(line);
        }

        return ParseGrid(grid);
    }

    private static Grid ParseGrid(IReadOnlyList<string> grid)
    {
        var symbols = new List<Symbol>();
        var numbers = new List<Number>();
        
        for (var row = 0; row < grid.Count; row++)
        {
            Number? currentNumber = null;
            
            for (var col = 0; col < grid[row].Length; col++)
            {
                var c = grid[row][col];
                
                // Ignore dots
                if (c == '.')
                {
                    currentNumber = null;
                }
                else if (char.IsDigit(c))
                {
                    var digit = c - '0';
                    if (currentNumber is null)
                    {
                        currentNumber = new Number(digit, col - 1, col + 1, row - 1, row + 1);
                        numbers.Add(currentNumber);
                    }
                    else
                    {
                        currentNumber.Value = (currentNumber.Value * 10) + digit;
                        currentNumber.Right++;
                    }
                }
                else
                {
                    symbols.Add(new Symbol(c, col, row));
                    currentNumber = null;
                }
            }
        }

        return new Grid(symbols, numbers);
    }
}