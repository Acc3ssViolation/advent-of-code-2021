using Adv.NET.Assignments;
using System;
using System.Threading.Tasks;

namespace Adv.NET
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var helper = new AssignmentHelper();

            var days = new IAssignment[]
            {
                new Day03(),
                new Day04(),
                new Day05(),
                new Day06(),
            };

            foreach (var assignment in days)
            {
                Console.WriteLine("=================================================");
                Console.WriteLine(assignment.Name);
                Console.WriteLine("=================================================");

                var input = helper.LoadData(assignment);

                if (input is null)
                {
                    Console.WriteLine("INPUT ERROR");
                    continue;
                }

                if (assignment is IAsyncAssignment asyncAssignment)
                    await asyncAssignment.RunAsync(input).ConfigureAwait(false);
                else
                    assignment.Run(input);
            }
        }
    }
}
