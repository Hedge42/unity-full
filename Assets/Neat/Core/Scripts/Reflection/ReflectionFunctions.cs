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
    public static class ReflectionFunctions
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
                var prop = (member as PropertyInfo);
                value = (member as PropertyInfo).GetValue(obj);
            }

            Debug.Log($"{member.Name} type: {value.GetType()}. Object? {value is Object}. Object[]? {value is Object[]}");

            return value;
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
                    Debug.LogError($"Needed parameterless method, but found {count} parameters");
            }

            return false;

        }
    }
}
