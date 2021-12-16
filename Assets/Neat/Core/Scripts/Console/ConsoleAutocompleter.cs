using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    public class ConsoleAutocompleter : ScriptableObject
    {
        public DebugConsole console;

        public static ConsoleAutocompleter Instantiate(DebugConsole console)
        {
            var obj = ScriptableObject.CreateInstance<ConsoleAutocompleter>();
            obj.console = console;

            return obj;
        }

        public bool isUpdated;
        private string[] _ids;
        public string[] commandids
        {
            get
            {
                if (!isUpdated)
                    _ids = Process();
                return _ids;
            }
        }

        public void OnGUI()
        {
            foreach (string id in commandids)
                GUILayout.Label(id);
        }

        private string[] Process()
        {
            List<string> list = new List<string>();
            foreach (var command in DebugCommandList.commands)
            {
                if (command.id.Contains(console.input))
                    list.Add(command.ToString());
            }
            return list.ToArray();
        }

    }
}
