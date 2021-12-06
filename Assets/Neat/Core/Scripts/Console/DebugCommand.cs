using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    [Serializable]
    public class DebugCommandBase // : SerializedObject
    {
        private string _id;
        private string _description;
        private string _format;

        public string id => _id;
        public string description => _description;
        public string format => _format;

        protected readonly Regex rx_id = new Regex(@"^[^\s]+");
        protected readonly Regex rx_format = new Regex(@"<[^\s]+>");



        public DebugCommandBase(string id, string description, string format)
        {
            _id = id;
            _description = description;
            _format = format;
        }

        public override string ToString()
        {
            // [^\s]+ // basically string.split(' ')
            return $"{format} | {description}";
        }

        public virtual bool Valid(string input)
        {
            bool id_match = rx_id.Match(input).ToString().Equals(this.id);
            if (id_match)
            {
                var formatCount = rx_format.Matches(format).Count;
                var inputCount = input.Split(' ').Count() - 1;
                bool arg_match = inputCount == formatCount;

                if (!arg_match)
                {
                    DebugConsole.Log($"Expected Arge: {this.format}\nReceived: {input}");
                }

                return arg_match;
            }
            else
            {
                // Debug.Log($"Expected ID: {this.id}\nReceived: {input}");

                return false;
            }
        }
    }
    public class DebugCommand : DebugCommandBase
    {
        private Action command;
        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
        {
            this.command = command;
        }
        public void Invoke()
        {
            DebugConsole.Log($"Executing command: {id}");
            command.Invoke();
        }
    }
    public class DebugCommand<T> : DebugCommandBase
    {
        private Action<T> command;
        public DebugCommand(string id, string description, string format, Action<T> command) : base(id, description, format)
        {
            this.command = command;
        }
        public void Invoke(T value)
        {
            DebugConsole.Log($"Executing command: {id}");
            command.Invoke(value);
        }
    }
    public class DebugCommand<T1, T2> : DebugCommandBase
    {
        private Action<T1, T2> command;

        public DebugCommand(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
        {
            this.command = command;
        }
        public void Invoke(T1 p1, T2 p2)
        {
            DebugConsole.Log($"Executing command: {id}");
            command.Invoke(p1, p2);
        }
    }

    

    // starting to think this was a bad idea?
    [CreateAssetMenu(menuName = "Neat/CheatList")]
    public class DebugCommandObject : ScriptableObject
    {
        public List<DebugCommandBase> commands;

        public DebugCommandBase this[int index] => commands[index];

        public static DebugCommandObject Instantiate(List<DebugCommandBase> commands)
        {
            var x = ScriptableObject.CreateInstance<DebugCommandObject>();
            x.commands = new List<DebugCommandBase>(commands);
            return x;
        }

        public override string ToString()
        {
            string s = "";
            foreach (var cmd in commands)
            {
                s += $"{cmd}\n";
            }

            return s;
        }
    }
}
