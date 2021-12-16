using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace Neat.Tools
{

    [CreateAssetMenu(menuName = "Neato/ReflectionUtility")]
    public class ReflectionUtility : ScriptableObject
    {
        // TODO use attribute

        public static string[] tags =
        {
            "Neat",
        };

        public static object[] processors =
        {
            GuiInspectorMap.instance,
            CommandProcessor.instance
        };

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod]
#endif
        public static void Reflect()
        {
            //var strat = new SelfStrategy();
            var p = Performer.Start();

            // var strat = new FilterStrategy();
            // var assemblies = strat.GetAssemblies();

            var assemblies = Filter();

            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetTypes())
                {
                    Process(type);
                    foreach (var member in type.GetMembers())
                    {
                        Process(member);

                        if (member is FieldInfo)
                            Process(member as FieldInfo);
                        else if (member is PropertyInfo)
                            Process(member as PropertyInfo);
                        else if (member is MethodInfo)
                            Process(member as MethodInfo);
                    }
                }
            }

            p.Stop();
            Debug.Log($"Reflected over {assemblies.Length} assemblies in {p.milliseconds}");

            // var test = Filter();
        }



        private static void Process<T>(T obj)
        {
            foreach (var p in processors)
                (p as IProcessor<T>)?.Process(obj);
        }
        private static Assembly[] Filter()
        {
            // the idea is you only have to walk through all assemblies once
            // maybe there is an easier way to get other assemblies?

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var filtered = new List<Assembly>();
            int count = -1;
            foreach (var asm in assemblies)
            {
                count++;

                try
                {
                    // GetIsTagged()
                    var name = asm.GetName();
                    var isTagged = false;
                    foreach (var tag in tags)
                    {
                        if (name.Name.StartsWith(tag))
                        {
                            isTagged = true;
                            break;
                        }
                    }

                    if (isTagged)
                    {
                        filtered.Add(asm);

                        // this is the kind of thing the editor needs a viewer for
                        //Debug.Log(
                        //    $"[{count}] Name={asm.FullName}" +
                        //    $"\nCodeBase={asm.CodeBase}" +
                        //    $"\nLocation={asm.Location}" +
                        //    $"\n");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"@ {asm.GetName()} {e}");
                }
            }

            var value = filtered.ToArray();
            return value;
        }

        // WHY
        private interface IAssemblyFilter
        {
            Assembly[] GetAssemblies();
        }
        private class SelfStrategy : IAssemblyFilter
        {
            public Assembly[] GetAssemblies()
            {
                return new Assembly[] { Assembly.GetExecutingAssembly() };
            }
        }
        private class FilterStrategy : IAssemblyFilter
        {
            public Assembly[] GetAssemblies()
            {
                return Filter();
            }
        }
    }
}
