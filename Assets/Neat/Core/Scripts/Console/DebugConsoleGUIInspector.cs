using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public class DebugConsoleGUIInspector : GUIInspector
    {
        public DebugConsole console => target as DebugConsole;


        protected override MemberInfo[] FindMembers()
        {
            var p = PerformanceElement.Start("Finding [DebugCommand] ...");

            var members = Functions.FindAttributeMembers<DebugCommandAttribute>(flags)
                .Where(m => m is MethodInfo && (m as MethodInfo).IsStatic)
                .ToArray();

            p.Stop();

            return members;
        }

        private void ViewCommands(MemberInfo[] members)
        {
            Debug.Log($"Found {members.Length} tagged commands: ");
            foreach (var member in members)
            {
                if (member is MethodInfo)
                {
                    var method = member as MethodInfo;
                    var _params = method.GetParameters().Select(p => p.ToString());
                    var args = $"({String.Join(',', _params)})";

                    Debug.Log($"cmd: {method.Name} {args}");
                }
            }
        }

        public override void OnGUI()
        {
            var controlName = "neato.console.input";
            GUI.SetNextControlName(controlName);
            console.input = GUILayout.TextField(console.input);

            if (GUI.changed)
                console.autocomplete.isUpdated = false;

            console.autocomplete.OnGUI();
        }
    }
}
