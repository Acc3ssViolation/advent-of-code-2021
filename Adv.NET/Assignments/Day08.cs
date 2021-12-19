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
            if (part == Part.One)
            {
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
                return;
            }

            foreach (var entry in loadedData)
            {
                var potentialPositions = new Segment[7];

                foreach (var output in entry.Patterns)
                {
                    var numSegments = CountActiveSegments(output);
                    if (numSegments == 2)
                    {
                        // This is a 1
                        if (potentialPositions[2] == Segment.None)
                            potentialPositions[2] = output;
                        else
                            potentialPositions[2] &= output;
                        if (potentialPositions[5] == Segment.None)
                            potentialPositions[5] = output;
                        else
                            potentialPositions[5] &= output;
                    }
                    else if (numSegments == 3)
                    {
                        // This is a 7
                        if (potentialPositions[0] == Segment.None)
                            potentialPositions[0] = output;
                        else
                            potentialPositions[0] &= output;
                        if (potentialPositions[2] == Segment.None)
                            potentialPositions[2] = output;
                        else
                            potentialPositions[2] &= output;
                        if (potentialPositions[5] == Segment.None)
                            potentialPositions[5] = output;
                        else
                            potentialPositions[5] &= output;
                    }
                    else if (numSegments == 4)
                    {
                        // This is a 4
                        if (potentialPositions[1] == Segment.None)
                            potentialPositions[1] = output;
                        else
                            potentialPositions[1] &= output;
                        if (potentialPositions[2] == Segment.None)
                            potentialPositions[2] = output;
                        else
                            potentialPositions[2] &= output;
                        if (potentialPositions[3] == Segment.None)
                            potentialPositions[3] = output;
                        else
                            potentialPositions[3] &= output;
                        if (potentialPositions[5] == Segment.None)
                            potentialPositions[5] = output;
                        else
                            potentialPositions[5] &= output;
                    }
                    else if (numSegments == 7)
                    {
                        // This is an 8
                        for (int i = 0; i < potentialPositions.Length; i++)
                        {
                            if (potentialPositions[i] == Segment.None)
                                potentialPositions[i] = output;
                            else
                                potentialPositions[i] &= output;
                        }
                    }
                }

                Console.WriteLine("");
            }
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
