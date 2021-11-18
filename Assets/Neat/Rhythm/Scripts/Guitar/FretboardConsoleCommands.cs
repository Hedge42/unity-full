using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Music;

namespace Neat.Console
{
    public static class FretboardConsoleCommands
    {
        static readonly Regex split = new Regex(@"[^\s]+");
        static readonly Regex num = new Regex(@"\d+");

        // ??????
        public static DebugCommandBase A(string regex, string description)
        {
            // format: name arg1<int> arg2 arg3+
            // examples: snap 2 b


            return new DebugCommandBase(regex, description, "");
        }
        public static bool MatchesType(this string arg, out Type t)
        {
            throw new System.NotImplementedException();

            if (int.TryParse(arg, out int _int))
            {

            }
            else if (float.TryParse(arg, out float _float))
            {

            }
        }
        // '''''''

        public static DebugCommand<string> SetScaleCommand()
        {
            return new DebugCommand<string>("scale", "sets the fretboard scale to the <value>", "scale <value>",
                SetScale);
        }
        public static void SetScale(string arg)
        {
            var fretboard = GameObject.FindObjectOfType<Fretboard>();
            if (fretboard == null)
            {
                Debug.LogError("No Fretboard component found");
                return;
            }

            // valid arg?
            arg = arg.ToLower();
            int key = -1;
            var flats = fretboard.scale.preferFlats;

            if (int.TryParse(arg, out int result))
            {
                // either parsable as an int
                result %= 12;
                key = result;
            }
            else
            {
                // or matches any note name
                var noteNames = MusicScale.AllNoteNames(flats);

                for (int i = 0; i < noteNames.Length; i++)
                {
                    if (noteNames[i].ToLower().Equals(arg))
                    {
                        key = i;
                        break;
                    }
                }
            }

            if (key >= 0)
            {

                fretboard.scale = new MusicScale(key, fretboard.scale.mode, flats);
                Debug.Log("Scale set to " + fretboard.scale.ToString());
            }

        }

        public static DebugCommand<string> SetFlatsCommand()
        {
            return new DebugCommand<string>("flats", "flats <value> is (t)rue or (false)", "flats <value>",
                SetFlats);
        }
        public static void SetFlats(string arg)
        {
            arg = arg.ToLower();
            var fretboard = GameObject.FindObjectOfType<Fretboard>();

            if (arg.Equals("true") || arg.Equals("t"))
            {
                fretboard.scale.preferFlats = true;
                Debug.Log("flats set to " + fretboard.scale.preferFlats);
            }
            else if (arg.Equals("false") || arg.Equals("f"))
            {
                fretboard.scale.preferFlats = false;
                Debug.Log($"flats set to {fretboard.scale.preferFlats}");
            }
            else
            {
                Debug.Log("Invaliid input");
            }
        }

        public static DebugCommand TestCommand()
        {
            throw new System.NotImplementedException();
            return new DebugCommand("test", "does a thing", "test",
                () =>
                {

                });
        }
    }
}
