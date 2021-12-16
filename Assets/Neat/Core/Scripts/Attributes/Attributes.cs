using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    // attributes only need declarations with optional constructors
    // so putting them in the same file makes sense


    public static class Attributes
    {
        public static AttributeTargets Member(this AttributeTargets at) //where T : typeof(AttributeTargets)
        {
            return AttributeTargets.Field;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxAttribute : PropertyAttribute
    {
        public Vector2 range;
        public MinMaxAttribute(float min, float max)
        {
            range.x = min;
            range.y = max;
        }
    }

    public class BeginDisabledGroupAttribute : PropertyAttribute { }
    public class EndDisabledGroupAttribute : PropertyAttribute { }

    // [AttributeUsage(AttributeTargets.)]
    public class DisabledAttribute : Attribute { }

    // [AttributeUsage(AttributeTargets.Field)]
    public class DisabledIfAttribute : Attribute
    {
        public string boolName { get; private set; }
        public DisabledIfAttribute(string boolName)
        {
            this.boolName = boolName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        // public Action action;
        public string methodName;

        public ButtonAttribute(string methodName)
        {
            // could use reflection here to find the method...
            this.methodName = methodName;
        }
        public ButtonAttribute()
        {
        }
    }

    public class HideIfAttribute : Attribute
    {
        public string fieldName;
        public HideIfAttribute(string boolFieldName)
        {
            this.fieldName = boolFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SerializePropertyAttribute : Attribute { }

    public class InlineAttribute : PropertyAttribute
    {
        // for serialized classes, removes the dropdown and shows properties by default
        // for scriptableObjects and components, display the reference field
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SidebarAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class ExtendAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class ListAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToolbarAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class CachedFieldAttribute : Attribute { }

    // [ExtendedInspector]
    /// <summary>
    /// Uses custom inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExtendedInspectorAttribute : Attribute { }

    // [MultiInspector]
    /// <summary>
    /// * Draws a toolbar to select between each inspector type. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiInspectorAttribute : Attribute { }

    // [ShowEditorScript]
    /// <summary>
    /// Draws a clickable reference to the editor script being used for this class, 
    /// Draws a script header for the current editor script
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ShowEditorScriptAttribute : Attribute { }

    // [GUIInspector]
    /// <summary>
    /// Changes the inspector to view 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GUIInspectorAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomGUIInspectorAttribute : Attribute
    {
        public Type type;
        public CustomGUIInspectorAttribute(Type type)
        {
            this.type = type;
        }
    }

    // [GUI]
    /// <summary>
    /// Try to draw runtime GUI version of tagged member
    /// If class, automatically tag all members
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Enum)] // ????
    public class GUIAttribute : Attribute { }

    // [EditorWindow]
    /// <summary>
    /// Adds a button to open the current inspector in a new editor window
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorWindowAttribute : Attribute { }

    // [GUIWindow]
    /// <summary>
    /// Adds a button to open the current inspector in an OnGUI window
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OnGUIWindowAttribute : Attribute { }

    // [ConsoleCommand]
    /// <summary>
    /// Tags a method to add to Console Commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCommandAttribute : Attribute { }

    // [CustomGUIDrawer(typeof(object))]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CustomGUIDrawer : Attribute
    {
        public CustomGUIDrawer(Type type) { }
    }

    // [GUI, NoPrefix]
    /// <summary>
    /// Remove the property's prefix in the Editor on OnGUI
    /// </summary>
    public class NoPrefixAttribute : Attribute { }


    // [Changed]
    /// <summary>
    /// Trigger an event when the value changes
    /// </summary>
    public class ChangedAttribute : Attribute { }

    // [DebugCommand]
    /// <summary>
    /// Adds this method to the list of debug commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DebugCommandAttribute : Attribute
    {
        public string description;
        public string id; // optional?
    }

    
}

