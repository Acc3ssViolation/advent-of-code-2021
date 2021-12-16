using Adv.NET.Assignments;
using System;

namespace Adv.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var helper = new AssignmentHelper();

            var days = new IAssignment[]
            {
                new Day03(),
                new Day04()
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

                assignment.Run(input);
            }
        }
    }
}
