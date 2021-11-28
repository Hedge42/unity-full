using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neat.Audio.Music;

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
                Neat.Audio.Music.Snapping.Set(count, type);
            }),

            FretboardConsoleCommands.SetScaleCommand(),
            FretboardConsoleCommands.SetFlatsCommand(),
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
    }
}
