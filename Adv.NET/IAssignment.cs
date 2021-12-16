using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.NET
{
    interface IAssignment
    {
        string Name { get; }
        void Run(IReadOnlyList<string> input);
    }
}
