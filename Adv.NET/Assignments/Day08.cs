using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET.Assignments
{
    [Flags]
    enum Segment
    {
        None = 0,
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64
    }

    record LogEntry(IReadOnlyList<Segment> Patterns, IReadOnlyList<Segment> Output);

    class Day08 : BaseAssignment<IReadOnlyList<LogEntry>>
    {
        protected override IReadOnlyList<LogEntry> Load(IReadOnlyList<string> input)
        {
            return input.Select(log =>
            {
                var split = log.Split(' ');
                var patterns = new List<Segment> { 
                    ParseSegments(split[0]),
                    ParseSegments(split[1]),
                    ParseSegments(split[2]),
                    ParseSegments(split[3]),
                    ParseSegments(split[4]),
                    ParseSegments(split[5]),
                    ParseSegments(split[6]),
                    ParseSegments(split[7]),
                    ParseSegments(split[8]),
                    ParseSegments(split[9]),
                };
                var output = new List<Segment> {
                    ParseSegments(split[11]),
                    ParseSegments(split[12]),
                    ParseSegments(split[13]),
                    ParseSegments(split[14]),
                };
                return new LogEntry(patterns, output);
            }).ToList();
        }

        protected override void RunPart(IReadOnlyList<LogEntry> loadedData, Part part)
        {
            if (part == Part.Two)
                return;

            var stuffs = 0;

            foreach (var entry in loadedData)
            {
                foreach (var output in entry.Output)
                {
                    var numSegments = CountActiveSegments(output);
                    if (numSegments == 3 || numSegments == 2 || numSegments == 4 || numSegments == 7)
                        stuffs++;
                }
            }

            Console.WriteLine($"Digits 1, 4, 7 and 8 appeared {stuffs} times");
        }

        private static Segment ParseSegments(string input)
        {
            var result = Segment.None;
            var trimmed = input.Trim();
            foreach (var chr in trimmed)
            {
                result |= chr switch
                {
                    'a' => Segment.A,
                    'b' => Segment.B,
                    'c' => Segment.C,
                    'd' => Segment.D,
                    'e' => Segment.E,
                    'f' => Segment.F,
                    'g' => Segment.G,
                    _ => throw new ArgumentException(),
                };
            }
            return result;
        }

        private static int CountActiveSegments(Segment segment)
        {
            return (int)System.Runtime.Intrinsics.X86.Popcnt.PopCount((uint)segment);
        }
    }
}
