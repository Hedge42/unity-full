using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public static partial class Functions
    {
        // idea for memberinfo extension methods
        // https://stackoverflow.com/questions/15921608/getting-the-type-of-a-memberinfo-with-reflection
        public static Type GetValueType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
        public static object GetValue(this MemberInfo member, object obj)
        {
            object value = null;
            if (member is FieldInfo)
                value = (member as FieldInfo).GetValue(obj);

            else if (member is PropertyInfo)
            {
                // should already be canRead to get here
                var canRead = (member as PropertyInfo).CanRead;
                value = (member as PropertyInfo).GetValue(obj);
            }

            return value;
            //else if (member is MethodInfo && member.IsEzMethod())
            //{
            //    throw new System.NotImplementedException();
            //    //value = (member as MethodInfo).
            //}
            //// Debug.Log($"{member.Name} = {value}");

            //return default;
        }
        public static void SetValue(this MemberInfo member, object obj, object value)
        {
            if (member is FieldInfo)
            {
                var info = (member as FieldInfo);
                info.SetValue(obj, value);
            }
            else if (member is PropertyInfo)
            {
                var info = (member as PropertyInfo);
                if (info.CanWrite)
                    info.SetValue(obj, value);
            }
            else
            {
                Debug.LogError($"Failed to set member");
            }

            // Debug.Log($"set {member.Name} -> {value}");
        }
        public static bool SetUnderlyingValue(this MemberInfo member, object target, object value)
        {
            if (member is PropertyInfo)
            {
                var info = (member as PropertyInfo);
                if (info.CanWrite)
                {
                    info.SetValue(target, value);
                    return true;
                }
            }

            return false;
        }
        public static bool IsEzMethod(this MemberInfo member)
        {
            // returns whether the 

            var method = member as MethodInfo;
            if (method != null)
            {
                var count = method.GetParameters().Count();
                if (count == 0)
                    return true;
                else
                    Debug.LogError($"Needed parameterless method, but found {count} parameters in {member.Name}");
            }

            return false;

        }
        public static MemberInfo[] FindMembers(this Object obj)
        {
            var _type = obj.GetType();
            var _members = _type.GetMembers();
            _members = _members.Where(m => m.IsRecognized(_type)).ToArray();
            return _members;
        }

        // GUIFunctions

        // ****
        public static bool IsRecognized(this MemberInfo member, Type _type)
        {
            return member.IsInternal(_type) && (member.IsUnitySerialized() || member.IsSerializedProperty());
        }
        public static bool IsUnitySerialized(this MemberInfo member)
        {
            // fields which are public or private-serialized 
            if (member is FieldInfo)
            {
                var fi = member as FieldInfo;
                return fi.IsPublic || fi.GetCustomAttribute<SerializeField>() != null;
            }
            return false;
        }
        public static bool IsSerializedProperty(this MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                var prop = member as PropertyInfo;
                var attr = member.GetCustomAttribute<SerializePropertyAttribute>();

                return prop.CanRead && attr != null;
            }
            return false;
        }
        public static bool IsInternal(this MemberInfo member, Type _type)
        {
            return member.DeclaringType == _type;
        }


        public static bool IsEnumerableType(this Type type)
        {
            return (type.GetInterface(nameof(System.Collections.IEnumerable)) != null);
        }

        
    }
}
