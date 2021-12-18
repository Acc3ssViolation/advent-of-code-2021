using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET.Assignments
{
    record Line(int X1, int Y1, int X2, int Y2);

    class Area
    {
        public int Width { get; }
        public int Height { get; }

        private int[] _surface;

        public Area(int width, int height)
        {
            Width = width;
            Height = height;

            _surface = new int[width * height];
        }

        public void Draw(Line line, bool includeDiagonal)
        {
            if (line.X1 == line.X2)
            {
                // Vertical
                var yMin = Math.Min(line.Y1, line.Y2);
                var yMax = Math.Max(line.Y1, line.Y2);
                var x = line.X1;

                for (int y = yMin; y <= yMax; y++)
                {
                    _surface[x + y * Width]++;
                }
            }
            else if (line.Y1 == line.Y2)
            {
                // Horizontal
                var xMin = Math.Min(line.X1, line.X2);
                var xMax = Math.Max(line.X1, line.X2);
                var y = line.Y1;

                for (int x = xMin; x <= xMax; x++)
                {
                    _surface[x + y * Width]++;
                }
            }
            else if (includeDiagonal)
            {
                // Diagonal
                var xMin = Math.Min(line.X1, line.X2);
                var xMax = Math.Max(line.X1, line.X2);
                var yMin = Math.Min(line.Y1, line.Y2);
                var yMax = Math.Max(line.Y1, line.Y2);

                if (yMax - yMin != xMax - xMin)
                    throw new ArgumentException("Line is not diaogonal!");

                var startAt1 = xMin == line.X1;

                var y = startAt1 ? line.Y1 : line.Y2;
                var yEnd = startAt1 ? line.Y2 : line.Y1;
                var yDelta = Math.Sign(yEnd - y);

                for (int x = xMin; x <= xMax; x++)
                {
                    _surface[x + y * Width]++;
                    y += yDelta;
                }
            }
        }

        public int CountOverlaps(int minimum)
        {
            var sum = 0;

            for (int i = 0; i < _surface.Length; i++)
            {
                if (_surface[i] >= minimum)
                    sum++;
            }

            return sum;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var value = _surface[x + y * Width];
                    if (value > 0)
                        sb.Append(value);
                    else
                        sb.Append('.');
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    class Day05 : IAssignment
    {
        public string Name => nameof(Day05);

        public void Run(IReadOnlyList<string> input)
        {
            var lines = LoadLines(input);
            Part(lines, 1);
            Part(lines, 2);

        }

        private void Part(List<Line> lines, int part)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Part {part}");
            Console.WriteLine("-------------------------------------------------");

            var area = new Area(1000, 1000);

            foreach (var line in lines)
            {
                area.Draw(line, part == 1 ? false : true);

                //Console.WriteLine("-------------------------------------------------");
                //Console.WriteLine($"{line.X1},{line.Y1} -> {line.X2},{line.Y2}");
                //Console.WriteLine("-------------------------------------------------");
                //Console.WriteLine(area.ToString());
            }

            var threshold = 2;
            var overlaps = area.CountOverlaps(threshold);

            Console.WriteLine($"Overlap of at least {threshold} lines: {overlaps}");
        }

        private List<Line> LoadLines(IReadOnlyList<string> input)
        {
            var parser = new Parser(string.Empty);
            var lines = new List<Line>();

            foreach (var line in input)
            {
                parser.Input(line);
                var x1 = parser.GetInt(",");
                var y1 = parser.GetInt("->");
                var x2 = parser.GetInt(",");
                var y2 = parser.GetInt();

                lines.Add(new Line(x1, y1, x2, y2));
            }

            return lines;
        }
    }
}
