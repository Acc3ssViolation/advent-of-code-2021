using System.Collections.Generic;

namespace Adv.NET.Assignments
{
    enum Part
    {
        One = 1,
        Two = 2,
    }

    abstract class BaseAssignment<T> : IAssignment
    {
        public string Name => GetType().Name;

        public void Run(IReadOnlyList<string> input)
        {
            var data = Load(input);
            AssignmentHelper.PrintSeparator((int)Part.One);
            RunPart(data, Part.One);
            data = Load(input);
            AssignmentHelper.PrintSeparator((int)Part.Two);
            RunPart(data, Part.Two);
        }

        protected abstract T Load(IReadOnlyList<string> input);
        protected abstract void RunPart(T loadedData, Part part);
    }
}
