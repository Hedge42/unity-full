using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Neat.Audio.Music;
using UnityEngine;

namespace Neat.Tools
{
    public static class DebugCommandList
    {
        // maybe this would be better as a singleton?
        public static List<DebugCommandBase> commands = new List<DebugCommandBase>()
        {
            // copy paste meee
            new DebugCommand("help", "prints command list", "help",
            () =>
            {
                 Help();
            }),
            new DebugCommand<string>("log", "prints the value in this console", "log <value>", (value) =>
            {
                DebugConsole.Log(value);
            }),
            new DebugCommand("clear", "clears this debug console", "clear", () =>
            {
                DebugConsole.Clear();
            }),
            new DebugCommand<string, string>("snap", "sets snapping at <n> ticks per <m>easure or <b>eat", "snap <count> <type>", (count, type) =>
            {
                // Neat.Audio.Music.Snapping.Set(count, type);
            }),
        };
        public static void Help()
        {
            foreach (var cmd in commands)
                DebugConsole.Log(cmd.ToString());
        }
        public static void Add(List<DebugCommandBase> newCommands)
        {
            commands.AddRange(newCommands);
        }
        public static void Add(DebugCommandBase newCommand)
        {
            commands.Add(newCommand);
        }


#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#elif UNITY_STANDALONE
        [RuntimeInitializeOnLoadMethod]
#endif
        public static void FindTaggedCommands()
        {
            var list = new List<ShortCommand>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static))
                    {
                        var attr = method.GetCustomAttribute<DebugCommandAttribute>(true);
                        if (attr != null)
                        {
                            var id = string.IsNullOrEmpty(attr.id) ? method.Name : attr.id;
                            bool valid = true;
                            foreach(var p in method.GetParameters())
                            {
                                if (p.ParameterType != typeof(string))
                                {
                                    valid = false;
                                }
                            }

                            if (valid)
                            {
                                list.Add(new ShortCommand(method));
                            }
                            else
                            {
                                Debug.Log($"Could not add command {method.Name} bad params");
                            }
                        }
                    }
                }
            }

            s_commands = list;
        }

        public static List<ShortCommand> s_commands;
    }
}
