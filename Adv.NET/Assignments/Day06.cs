using System;
using System.Collections.Generic;
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

            Console.WriteLine($"Starting fish sim 2022");

            var simulationThread = DispatchSimulationThread(new Settings(1 << 24, maxDays, fish));

            simulationThread.Join();

            Console.WriteLine($"Number of fish after {maxDays} days: {Interlocked.Exchange(ref _fishCount, 0)}");
        }

        record Settings(int ChunkSize, int Days, IReadOnlyList<int> InitialFish);

        private Thread DispatchSimulationThread(Settings settings)
        {
            Console.WriteLine($"Creating thread with settings {settings}");
            var thread = new Thread(new ParameterizedThreadStart(SimulateFish));
            thread.Start(settings);
            return thread;
        }

        private void SimulateFish(object? param)
        {
            var settings = param as Settings ?? throw new ArgumentException();

            if (settings.InitialFish.Count > settings.ChunkSize / 2)
                throw new ArgumentOutOfRangeException(nameof(settings));

            int[] fish = new int[settings.ChunkSize];
            int fishInChunk = settings.InitialFish.Count;

            var childThreads = new List<Thread>();

            for (int i = 0; i < settings.InitialFish.Count; i++)
                fish[i] = settings.InitialFish[i];

            for (int days = 0; days < settings.Days; days++)
            {
                for (int i = fishInChunk - 1; i >= 0; i--)
                {
                    if (fish[i] == 0)
                    {
                        fish[fishInChunk++] = 8;
                        Interlocked.Increment(ref _fishCount);
                        fish[i] = 6;
                    }
                    else
                    {
                        fish[i]--;
                    }
                }

                if (fishInChunk >= settings.ChunkSize / 2)
                {
                    childThreads.Add(DispatchSimulationThread(settings with
                    {
                        Days = settings.Days - days,
                        InitialFish = fish.Skip(settings.ChunkSize / 2).ToList(),
                    }));
                    fishInChunk = settings.ChunkSize / 2;
                }
            }

            foreach (var childThread in childThreads)
            {
                childThread.Join();
            }
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
