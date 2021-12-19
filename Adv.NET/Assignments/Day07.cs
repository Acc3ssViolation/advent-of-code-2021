using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET.Assignments
{
    class Day07 : BaseAssignment<IReadOnlyList<int>>
    {
        protected override IReadOnlyList<int> Load(IReadOnlyList<string> input)
        {
            return input[0].Split(',').Select(num => int.Parse(num)).ToList();
        }

        protected override void RunPart(IReadOnlyList<int> loadedData, Part part)
        {
            var from = loadedData.Min();
            var to = loadedData.Max();

            var min = int.MaxValue;
            var crabCount = loadedData.Count;

            for (int i = from; i <= to; i++)
            {
                var totalDistance = 0;

                for (int k = 0; k < crabCount; k++)
                {
                    var delta = Math.Abs(i - loadedData[k]);
                    if (part == Part.One)
                        totalDistance += delta;
                    else
                        totalDistance += FuelCost(delta);
                }

                if (totalDistance < min)
                    min = totalDistance;
            }

            Console.WriteLine($"Optimal fuel: {min}");
        }

        private int FuelCost(int distance)
        {
            var sum = 0;
            for (int i = 0; i <= distance; i++)
            {
                sum += i;
            }
            return sum;
        }
    }
}
