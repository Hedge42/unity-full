using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System.Reflection;

namespace Neat.Tools
{
    // bunch of experimental BS

    [AttributeUsage(AttributeTargets.Class)]
    public class VirtualInspectorAttribute : Attribute
    {
        public Type type;

        // type inherits from virtual inspector
        public bool isValidType => typeof(VirtualInspector).IsAssignableFrom(type);

        public VirtualInspectorAttribute(Type type)
        {
            this.type = type;
        }

        // [InitializeOnLoadMethod]
        public static void OK()
        {
            var p = PerformanceElement.Start("Finding virtual inspectors...");

            // find my virtual inspector classes...
            var vInspectors = new List<VirtualInspector>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    var attr = type.GetCustomAttribute<VirtualInspectorAttribute>(true);
                    if (attr != null)
                    {
                        if (attr.isValidType)
                        {
                            var s = ScriptableObject.CreateInstance(attr.type) as VirtualInspector;
                            vInspectors.Add(s);
                        }
                        else
                        {
                            Debug.LogError("Virtual Inspector must inherit from abstract VirtualInspector class");
                        }
                    }
                }
            }

            Debug.Log("Found " + vInspectors.Count + " virtual inspectors!");

            p.Stop();
        }
    }

    public abstract class VirtualInspector : ScriptableObject
    {
        public Object target;
        public abstract void OnGUI();
    }
    public class TestClass { }

    // [CustomGUIDrawer(typeof(TestClass))]
    public class SomeClassDrawer : GuiPropertyDrawer
    {
        public static Dictionary<Type, Type> dic;

        // [InitializeOnLoadMethod]
        public static void GetGUITypeMap()
        {
            dic = new Dictionary<Type, Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    var attr = type.GetCustomAttribute<CustomGUIDrawer>(true);

                    if (attr != null)
                    {
                        dic.Add(attr.GetType(), type);
                    }
                }
            }
        }
    }

    public abstract class CustomGuiDrawer : ScriptableObject
    {
        public abstract void DrawPropertyLayout(object value);
    }

    // [CustomGUIDrawer(typeof(TestClass))]
    public class TestClassDrawer : CustomGuiDrawer
    {
        public override void DrawPropertyLayout(object value)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomGuiModifierAttribute : Attribute
    {
        public Type type;
        public CustomGuiModifierAttribute(Type type)
        {
            this.type = type;
        }
    }

    public abstract class CustomAttributeDrawer
    {
        public virtual void BeforeGUI() { }
        public virtual void OnGUI() { }
        public virtual void AfterGUI() { }
    }

    //[CustomGuiModifier(typeof(DisabledAttribute))]
    public class DisabledDrawer : CustomAttributeDrawer
    {
        public override void BeforeGUI()
        {
            GUI.enabled = false;
        }
        public override void AfterGUI()
        {
            GUI.enabled = true;
        }

        public void Drawe(MemberInfo member)
        {
            member.GetCustomAttributes();
        }
    }
}
