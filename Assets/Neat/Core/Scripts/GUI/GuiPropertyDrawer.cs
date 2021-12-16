using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public class GuiPropertyDrawerAttribute : Attribute
    {
        public Type type;
        public GuiPropertyDrawerAttribute(Type type)
        {
            this.type = type;
        }
    }

    public abstract class GuiPropertyDrawer : MonoBehaviour
    {
        public static GuiPropertyDrawer Create(object o)
        {
            var type = o.GetType();
            var attribute = type.GetCustomAttribute<GuiPropertyDrawerAttribute>();
            if (attribute != null)
            {
                if (type.IsSubclassOf(typeof(GuiPropertyDrawer)))
                {
                    // get or create guidrawer...
                }
            }

            throw new NotImplementedException();
        }
    }
}