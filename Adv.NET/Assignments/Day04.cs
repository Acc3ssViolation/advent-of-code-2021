using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET.Assignments
{
    class Board
    {
        public int Size { get; }
        private int[] _numbers;
        private int[] _markedRowCount;
        private int[] _markedColumnCount;
        private int _markedDiagCount1;
        private int _markedDiagCount2;

        public Board(IReadOnlyList<int> numbers, int size)
        {
            Size = size;
            if (numbers.Count != size * size)
                throw new ArgumentOutOfRangeException(nameof(numbers));
            _numbers = numbers.ToArray();
            _markedRowCount = new int[Size];
            _markedColumnCount = new int[Size];
        }

        public bool Mark(int value)
        {
            for (int i = 0; i < _numbers.Length; i++)
            {
                if (_numbers[i] == value)
                {
                    _numbers[i] = -1;
                    var row = i / Size;
                    var col = i % Size;
                    if (++_markedRowCount[row] == Size)
                        return true;
                    if (++_markedColumnCount[col] == Size)
                        return true;

                    if (row == col && (++_markedDiagCount1 == Size))
                        return true;
                    if (row == (Size - 1 - col) && (++_markedDiagCount2 == Size))
                        return true;
                }
            }

            return false;
        }

        public int GetScore()
        {
            int sum = 0;
            for (int i = 0; i < _numbers.Length; i++)
            {
                if (_numbers[i] != -1)
                    sum += _numbers[i];
            }
            return sum;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _numbers.Length; i++)
            {
                sb.Append($"{_numbers[i],2} ");
                if ((i + 1) % 5 == 0)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    class Day04 : IAssignment
    {
        public string Name => nameof(Day04);

        public void Run(IReadOnlyList<string> input)
        {
            var bingoNumbers = input[0].Split(',').Select(s => int.Parse(s)).ToList();
            var boards = new List<Board>();

            for (int i = 1; i < input.Count; i++)
            {
                var numberStrings = input[i].Split();
                if (numberStrings.Length == 1)
                    continue;
                var boardLine = numberStrings.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s)).ToList();
                var size = boardLine.Count;
                for (int k = 1; k < size; k++)
                {
                    numberStrings = input[i + k].Split();
                    boardLine.AddRange(numberStrings.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s)));
                }
                boards.Add(new Board(boardLine, size));
                i += size;
            }

            foreach (var number in bingoNumbers)
            {
                Console.WriteLine($"Number: {number}");
                foreach (var board in boards)
                {
                    if (board.Mark(number))
                    {
                        var score = board.GetScore();
                        Console.WriteLine($"First board score: {score}");
                        Console.WriteLine($"Final result: {score} * {number} = {score * number}");
                        return;
                    }

                    Console.WriteLine(board);
                }
            }
        }
    }
}
