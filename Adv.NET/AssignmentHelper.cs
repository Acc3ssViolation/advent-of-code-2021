using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adv.NET
{
    class AssignmentHelper
    {
        public string InputFolder { get; set; } = "input";
        public Encoding FileEncoding { get; set; } = new UTF8Encoding(false);

        public IReadOnlyList<string>? LoadData(IAssignment assignment)
        {
            try
            {
                return File.ReadAllLines(Path.Combine(InputFolder, $"{assignment.Name.ToLowerInvariant()}.txt"), FileEncoding);
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine($"Exception when trying to read file for assignment {assignment.Name}:");
                Console.Error.WriteLine(exc);
                return null;
            }
        }

        public static void PrintSeparator(int part)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Part {part}");
            Console.WriteLine("-------------------------------------------------");
        }
    }
}
