using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv.NET
{
    class Parser
    {
        private string _input;

        public Parser(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
        }

        public void Input(string input)
        {
            _input = input;
        }

        public int GetInt(string delimeter = "")
        {
            _input.TrimStart();
            var delimeterIndex = delimeter != string.Empty ? _input.IndexOf(delimeter) : -1;
            int num;
            if (delimeterIndex != -1)
                num = int.Parse(_input.Substring(0, delimeterIndex));
            else
                num = int.Parse(_input);
            var len = num.ToString().Length;
            if (delimeterIndex != -1)
                len = delimeterIndex + delimeter.Length;
            _input = _input.Substring(len);
            return num;
        }

        public void Get(string literal, bool trim = true)
        {
            _input = _input.TrimStart();
            if (!_input.StartsWith(literal))
                throw new ArgumentException($"'{literal}' could not be found in input '{_input}'");
            _input = _input.Substring(literal.Length);
        }
    }
}
