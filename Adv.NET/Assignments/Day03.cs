using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET.Assignments
{
    class Day03 : IAssignment
    {
        enum Variable
        {
            Gamma,
            Epsilon
        }

        public string Name => nameof(Day03);

        public void Run(IReadOnlyList<string> input)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Part 2");
            Console.WriteLine("-------------------------------------------------");

            var gamma = Solve(input, Variable.Gamma);
            var epsilon = Solve(input, Variable.Epsilon);

            Console.WriteLine($"g = {gamma}, e = {epsilon}, p = {gamma * epsilon}");
        }

        private uint Solve(IReadOnlyList<string> data, Variable kind, int charIndex = 0)
        {
            if (data.Count == 1)
                return ParseBinaryString(data[0]);

            var high = new List<string>();
            var low = new List<string>();
            foreach (var entry in data)
            {
                if (entry[charIndex] == '1')
                    high.Add(entry);
                else
                    low.Add(entry);
            }

            List<string> listToSolve;

            if (kind == Variable.Gamma)
            {
                if (high.Count >= low.Count)
                {
                    Console.WriteLine($"{high.Count} >= {low.Count}, keeping {high.Count} entries with a '1'");
                    listToSolve = high;
                }
                else
                {
                    Console.WriteLine($"{high.Count} < {low.Count}, keeping {low.Count} entries with a '0'");
                    listToSolve = low;
                }
            }
            else
            {
                if (high.Count >= low.Count)
                {
                    Console.WriteLine($"{high.Count} >= {low.Count}, keeping {low.Count} entries with a '0'");
                    listToSolve = low;
                }
                else
                {
                    Console.WriteLine($"{high.Count} < {low.Count}, keeping {high.Count} entries with a '1'");
                    listToSolve = high;
                }
            }

            return Solve(listToSolve, kind, charIndex + 1);
        }


        private static uint ParseBinaryString(string input)
        {
            input = input.Trim();
            var result = 0u;

            foreach (var digit in input)
            {
                result <<= 1;
                result |= (digit == '1') ? 1u : 0u;
            }

            return result;
        }
    }
}
