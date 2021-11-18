using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Console
{
    public static class CommandParser
    {
        private static Regex rx_id = new Regex(@"^[^\s]+");
        private static Regex rx_format = new Regex(@"<[^\s]+>");

        public static bool Attempt(this DebugCommandBase cmd, string input)
        {
            bool id_match = rx_id.Match(input).ToString().Equals(cmd.id);
            if (id_match)
            {
                var inputParams = GetParams(input);
                var inputParamCount = inputParams.Length;
                var cmdParamCount = rx_format.Matches(cmd.format).Count;

                if (inputParamCount.Equals(cmdParamCount))
                {
                    // validated
                    Invoke(cmd, inputParams);
                    return true;
                }
                else
                {
                    DebugConsole.Log($"Expected Args: {cmd.format}\nReceived: {input}");
                }
            }

            return false;
        }
        public static void Invoke(DebugCommandBase _cmd, string[] _params)
        {
            if (_cmd is DebugCommand)
                (_cmd as DebugCommand).Invoke();
            else if (_cmd is DebugCommand<string>)
                (_cmd as DebugCommand<string>).Invoke(_params[0]);
            else if (_cmd is DebugCommand<string, string>)
                (_cmd as DebugCommand<string, string>).Invoke(_params[0], _params[1]);
            else
                DebugConsole.Log("Bad command types, use strings");
        }

        private static string[] GetParams(string input)
        {
            var words = input.Split(' ');

            var _params = new string[words.Length - 1];

            for (int i = 1; i < words.Length; i++)
                _params[i - 1] = words[i];

            return _params;
        }

    }
}
