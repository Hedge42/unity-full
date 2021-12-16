using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Neat.Tools
{

    public class GuiInspectorMap : IProcessor<Type>
    {
        private static GuiInspectorMap _instance;
        // public static GuiInspectorMap instance => GameUtility.Cache(ref _instance, () => new GuiInspectorMap());
        public static GuiInspectorMap instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GuiInspectorMap()
                    {
                        typeMap = new Dictionary<Type, GuiInspector>(),
                    };
                return _instance;
            }
        }

        public readonly Type baseType = typeof(object);
        public Dictionary<Type, GuiInspector> typeMap;


        //private static Dictionary<Type, GuiInspector> _map;
        //public static Dictionary<Type, GuiInspector> _typeMap => GameUtility.Cache(ref _map, () => new Dictionary<Type, GuiInspector>());

        public GuiInspector this[Type type]
        {
            get
            {
                if (typeMap.ContainsKey(type))
                {
                    return typeMap[type];
                }
                else
                {
                    return typeMap[baseType];
                }
            }
        }

        public void Process(Type type)
        {
            var attr = type.GetCustomAttribute<GuiPropertyDrawerAttribute>(true);
            if (attr != null)
            {
                if (!typeMap.ContainsKey(type))
                {
                    if (!typeof(GuiInspector).IsAssignableFrom(type))
                        Debug.LogError($"{type.Name} must inherit from base GuiDrawer");

                    else
                    {
                        try
                        {
                            var drawer = ScriptableObject.CreateInstance(type) as GuiInspector;
                            typeMap.Add(type, drawer);
                            Debug.Log($"Added drawer: {drawer.GetType().Name}");
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning($"Failed to add GuiDrawer {type.Name} \n{e}");
                        }
                    }
                }
            }
        }
    }
}
