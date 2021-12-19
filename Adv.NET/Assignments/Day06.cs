using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adv.NET.Assignments
{
    class Day06 : IAssignment
    {
        public string Name => nameof(Day06);

        private int _fishCount = 0;

        public void Run(IReadOnlyList<string> input)
        {
            var fish = LoadFish(input);
            Part(fish, 1);
            fish = LoadFish(input);
            Part(fish, 2);
        }

        private void Part(List<int> fish, int part)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Part {part}");
            Console.WriteLine("-------------------------------------------------");

            Interlocked.Exchange(ref _fishCount, fish.Count);
            int maxDays = part == 1 ? 80 : 256;

            var result = Simulate(fish, maxDays);

            Console.WriteLine($"Number of fish after {maxDays} days: {result}");
        }

        private long Simulate(List<int> fish, int days)
        {
            long[] fishPerDay = new long[9];

            foreach (var age in fish)
            {
                fishPerDay[age]++;
            }

            for (int day = 0; day < days; day++)
            {
                long newFish = fishPerDay[0];
                for (int i = 1; i < fishPerDay.Length; i++)
                {
                    fishPerDay[i - 1] = fishPerDay[i];
                }
                fishPerDay[8] = newFish;
                fishPerDay[6] += newFish;
            }

            return fishPerDay.Sum();
        }

        private List<int> LoadFish(IReadOnlyList<string> input)
        {
            IEnumerable<int> nums = Array.Empty<int>();
            var numnums = input.Select(i => i.Split(',').Select(t => int.Parse(t)));
            foreach (var nm in numnums)
            {
                nums = nums.Concat(nm);
            }
            return nums.ToList();
        }
    }
}
