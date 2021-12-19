using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET
{
    interface IAssignment
    {
        string Name { get; }
        void Run(IReadOnlyList<string> input);
    }

    interface IAsyncAssignment : IAssignment
    {
        Task RunAsync(IReadOnlyList<string> input);
    }
}
