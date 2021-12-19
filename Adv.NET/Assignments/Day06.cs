using System;
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

            var manager = new SimulationManager(Environment.ProcessorCount, 6 * 1024);

            Console.WriteLine($"Starting fish sim 2022");

            manager.Start(fish.ToArray(), 18);

            Thread.Sleep(5000);

            manager.Stop();

            Console.WriteLine($"Number of fish after {maxDays} days: {manager.Result}");
        }

        record Settings(int ChunkSize, int Days, IReadOnlyList<int> InitialFish);

        private Thread DispatchSimulationThread(Settings settings)
        {
            Console.WriteLine($"Creating thread with settings {settings}");
            var thread = new Thread(new ParameterizedThreadStart(SimulateFish));
            thread.Start(settings);
            return thread;
        }

        class SimulationManager
        {
            private int _fishCount;
            private Thread[] _threads;
            private SemaphoreSlim _takeFishFileGuard = new SemaphoreSlim(0);
            private bool _isRunning;
            private string _fishFileDirectory;
            private bool _stopFlag;
            private int _perThreadArraySize;
            //private SemaphoreSlim _busyTasks;

            public int Result => _fishCount;

            public SimulationManager(int threadCount, int memoryCapMegabytes)
            {
                _threads = new Thread[threadCount];
                _fishFileDirectory = string.Empty;

                const int integersPerMegabyte = 262144;
                long memoryPerThread = (memoryCapMegabytes / threadCount);
                var tempPerThreadArraySize = (memoryPerThread * integersPerMegabyte);
                if (tempPerThreadArraySize > int.MaxValue)
                    _perThreadArraySize = int.MaxValue;
                else
                    _perThreadArraySize = (int)tempPerThreadArraySize;
            }

            private void ThreadFunction()
            {
                int[] fish = new int[_perThreadArraySize];
                int fishCount;
                var threshold = fish.Length / 2;

                while (!_stopFlag)
                {
                    var data = LoadFish();
                    if (data is null)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    var (loadedFish, daysLeft) = data.Value;
                    loadedFish.CopyTo(fish, 0);
                    fishCount = loadedFish.Length;

                    for (; daysLeft > 0; daysLeft--)
                    {
                        for (int i = fishCount - 1; i > 0; i--)
                        {
                            if (fish[i] == 0)
                            {
                                fish[fishCount++] = 8;
                                Interlocked.Increment(ref _fishCount);
                                fish[i] = 6;
                            }
                            else
                            {
                                fish[i]--;
                            }
                        }

                        if (fishCount > threshold)
                        {
                            // Dump the first half of the fish array in a file for another worker to pick up later
                            SaveFish(fish.AsSpan(0, threshold), daysLeft - 1);
                            fishCount -= threshold;
                            Array.Copy(fish, threshold, fish, 0, fishCount);
                        }
                    }
                }
            }

            private (int[] fish, int daysLeft)? LoadFish()
            {
                _takeFishFileGuard.Wait();

                try
                {
                    // Find a file
                    var files = Directory.GetFiles(_fishFileDirectory);
                    if (files.Length == 0)
                        return null;
                    var fileToLoad = files[0];
                    var result = LoadFish(files[0]);
                    File.Delete(fileToLoad);
                    Console.WriteLine($"Loaded fish from '{fileToLoad}'");
                    return result;
                }
                finally
                {
                    _takeFishFileGuard.Release();
                }
            }

            private void SaveFish(Span<int> fish, int daysLeft)
            {
                using var outFile = File.Create(Path.Join(_fishFileDirectory, Path.GetRandomFileName()));
                using var writer = new BinaryWriter(outFile, Encoding.ASCII);

                writer.Write(daysLeft);
                writer.Write(fish.Length);
                for (int i = 0; i < fish.Length; i++)
                {
                    writer.Write(fish[i]);
                }

                Console.WriteLine($"Saved fish to '{outFile.Name}'");
            }

            private (int[] fish, int daysLeft) LoadFish(string filePath)
            {
                using var inFile = File.OpenRead(filePath);
                using var reader = new BinaryReader(inFile, Encoding.ASCII);

                var daysLeft = reader.ReadInt32();
                var fishCount = reader.ReadInt32();
                var fish = new int[fishCount];
                for (int i = 0; i < fish.Length; i++)
                {
                    fish[i] = reader.ReadInt32();
                }

                return (fish, daysLeft);
            }

            public void Start(Span<int> fish, int days)
            {
                if (_isRunning)
                    throw new InvalidOperationException();

                _isRunning = true;
                _stopFlag = false;

                _fishFileDirectory = Path.Join(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(_fishFileDirectory);

                SaveFish(fish, days);

                _takeFishFileGuard.Release();

                for (int i = 0; i < _threads.Length; i++)
                {
                    _threads[i] = new Thread(new ThreadStart(ThreadFunction));
                    _threads[i].Start();
                }
            }

            public void Stop()
            {
                if (!_isRunning)
                    return;

                _stopFlag = true;

                for (int i = 0; i < _threads.Length; i++)
                {
                    _threads[i].Join();
                }
            }
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
