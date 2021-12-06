using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    public class DebugCommandAttributeHandler
    {
        [DebugCommand]
        public static void TestCommand() { }

        /// <summary>
        /// Valid methods: static void, no params
        /// </summary>
        // [RuntimeInitializeOnLoadMethod]
        public static List<DebugCommandBase> FindCommands()
        {
            var list = new List<DebugCommandBase>();
            var x = Functions.FindMembersAndAttributes<DebugCommandAttribute>();
            Debug.Log($"Filtering {x.Count} pairs...");

            foreach (var pair in x)
            {
                if (pair.member.IsEzMethod())
                {
                    var method = pair.member as MethodInfo;
                    Action action = () => method.Invoke(null, null);
                    var command = new DebugCommand(pair.member.Name, "", pair.member.Name, action);
                    list.Add(command);
                }
            }

            Debug.Log($"Found {x.Count}: ");
            foreach (var m in x)
            {
                Debug.Log(m.member.Name);
            }

            return list;
        }
    }
}
