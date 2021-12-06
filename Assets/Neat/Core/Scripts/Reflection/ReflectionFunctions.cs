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
                     $"Input MemberInfo {member.Name} must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo, but was type {member.GetType()}"
                    );
            }
        }
        public static bool ValidMember(this MemberInfo member, Object target)
        {
            return member.ValidInfoType() && member.IsDeclared(target);
        }
        private static bool ValidInfoType(this MemberInfo member)
        {
            var valid = member is PropertyInfo || member is FieldInfo || member is MethodInfo || member is EventInfo;

            if (!valid) { }
            return valid;
        }
        private static bool IsDeclared(this MemberInfo member, Object obj)
        {
            var _type = obj.GetType();
            return member.DeclaringType == _type;
        }

        private static bool GetConditional(string memberName, MemberInfo[] members, Object target)
        {
            var member = members.FirstOrDefault(m => m.Name.Equals(memberName));
            if (member == null)
            {
                Debug.LogError($"No member \"{memberName}\" found");
                return false;
            }
            else
            {
                if (member is PropertyInfo)
                {
                    var info = (member as PropertyInfo);
                    if (info.PropertyType == typeof(bool))
                        return (bool)info.GetValue(target);
                    else
                        Debug.LogError($"Property \"{info.Name}\" " +
                            $"type returned {info.PropertyType}" +
                            $"but needed bool");
                }
                else if (member is FieldInfo)
                {
                    var info = (member as FieldInfo);
                    if (info.FieldType == typeof(bool))
                        return (bool)info.GetValue(target);
                    else
                        Debug.LogError($"Field \"{info.Name}\" " +
                            $"type returned {info.FieldType}" +
                            $"but needed bool");
                }
                else if (member is MethodInfo && member.IsEzMethod())
                {
                    var info = (member as MethodInfo);
                    if (info.ReturnType == typeof(bool))
                        return (bool)info.Invoke(target, null);
                    else
                        Debug.LogError($"Method \"{info.Name}\" " +
                            $"type returned {info.ReturnType}" +
                            $"but needed bool");

                    return (bool)(member as MethodInfo).Invoke(target, null);
                }
                else
                    Debug.LogError("???");
            }

            return false;
        }
        private static bool IsHidden(IEnumerable<Attribute> attributes, MemberInfo[] members, Object target)
        {
            // TODO optimize me
            var attr = attributes.FirstOrDefault(a => a is HideIfAttribute) as HideIfAttribute;
            if (attr != null)
                return GetConditional(attr.fieldName, members, target);

            return false;
        }
        private static bool IsDisabled(IEnumerable<Attribute> attributes, MemberInfo[] members, Object target)
        {
            var attr = attributes.FirstOrDefault(a => a is DisabledAttribute) as DisabledAttribute;
            if (attr != null)
                return true;

            var ifAttr = attributes.FirstOrDefault(a => a is DisabledIfAttribute) as DisabledIfAttribute;
            if (ifAttr != null)
                return GetConditional(ifAttr.boolName, members, target);

            return false;
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
                obj = info.IsStatic ? null : obj;

                info.SetValue(obj, value);
            }
            else if (member is PropertyInfo)
            {
                var info = (member as PropertyInfo);
                obj = info.GetAccessors(true)[0].IsStatic ? null : obj;

                if (info.CanWrite)
                    info.SetValue(obj, value);
            }
            else
            {
                Debug.LogError($"Failed to set member");
            }

            // Debug.Log($"set {member.Name} -> {value}");
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

        // ???????π™
        public struct MemberPair
        {
            public MemberInfo member;
            public Attribute attribute;

            public MemberPair(MemberInfo member, Attribute attribute)
            {
                this.member = member;
                this.attribute = attribute;
            }
        }
        public struct TypePair
        {
            public Type type;
            public Attribute attribute;

            public TypePair(Type type, Attribute attribute)
            {
                this.type = type;
                this.attribute = attribute;
            }
        }

        public static List<MemberInfo> FindAttributeMembers<T>(BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<MemberInfo> exposedMembers = new List<MemberInfo>();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    MemberInfo[] members = type.GetMembers(flags);
                    foreach (MemberInfo member in members)
                    {
                        // TESTME
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            T attribute = member.GetCustomAttribute<T>();
                            if (attribute != null)
                            {
                                exposedMembers.Add(member);
                                // do stuff
                                // fields.Add((FieldInfo)member);
                                // fieldNames.Add($"{member.ReflectedType}/{attribute.instance}");
                            }
                        }
                    }
                }
            }
            return exposedMembers;
        }

        public static List<TypePair> FindTypePairs<AttributeType>(BindingFlags flags = BindingFlags.Default) where AttributeType : Attribute
        {
            List<TypePair> found = new List<TypePair>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                found.AddRange(FindTypePairs<AttributeType>(assembly, flags));

            return found;
        }
        public static List<TypePair> FindTypePairs<AttributeType>(Assembly assembly, BindingFlags flags = BindingFlags.Default) where AttributeType : Attribute
        {
            List<TypePair> found = new List<TypePair>();

            foreach (var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<AttributeType>();
                if (attribute != null)
                    found.Add(new TypePair(type, attribute));
            }
            return found;
        }

        public static List<Type> FindTypes<AttributeType>(Assembly assembly) where AttributeType : Attribute
        {
            List<Type> found = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<AttributeType>();
                if (attribute != null)
                    found.Add(type);
            }
            return found;
        }
        public static List<Type> FindTypes<AttributeType>() where AttributeType : Attribute
        {
            List<Type> found = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                found.AddRange(FindTypes<AttributeType>(assembly));

            return found;
        }

        public static List<MemberPair> FindMembersAndAttributes<T>(BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            List<MemberPair> found = new List<MemberPair>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                found.AddRange(FindMembersAndAttributes<T>(assembly, flags));

            return found;
        }
        public static List<MemberPair> FindMembersAndAttributes<T>(Assembly assembly, BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            List<MemberPair> found = new List<MemberPair>();
            foreach (var type in assembly.GetTypes())
                found.AddRange(FindMembersAndAttributes<T>(type, flags));

            return found;
        }
        public static List<MemberPair> FindMembersAndAttributes<T>(Type type, BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            List<MemberPair> found = new List<MemberPair>();
            var members = type.GetMembers();
            foreach (MemberInfo member in members)
            {
                var attribute = member.GetCustomAttribute<T>();
                var validType = member.GetValueType() != null;
                if (attribute != null)
                    found.Add(new MemberPair(member, attribute));
            }
            return found;
        }
        public static List<MemberInfo> FindValidMembers(Type type)
        {
            var list = new List<MemberInfo>();
            var members = type.GetMembers();

            //list.AddRange(type.GetMethods());
            //list.AddRange(type.GetProperties());
            //list.AddRange(type.GetFields());
            //list.AddRange(type.GetEvents)();

            foreach (MemberInfo member in members)
            {
                if (member is PropertyInfo || member is MethodInfo || member is FieldInfo || member is EventInfo)
                {
                    list.Add(member);
                }

                try
                {

                    var x = member.GetValueType();


                    // list.Add(member);
                }
                catch
                {
                    Debug.Log($"Could not add member {member.Name}");
                }
            }
            return list;
        }

        /// <summary> Guessing which members are serialized by default by Unity </summary>
        public static List<MemberPair> FindUnityMembersAndAttributes<T>(Type type, BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            var list = new List<MemberPair>();
            var members = type.GetMembers();
            foreach (var member in members)
            {
                if (member is FieldInfo)
                {
                    var field = member as FieldInfo;
                    var attr = member.GetCustomAttribute<SerializeField>();
                    var valid = field.IsPublic || attr != null;

                    if (valid)
                        list.Add(new MemberPair(member, attr));
                }
            }
            return list;
        }
        public static List<MemberPair> FindAttributePairs<T>(Type type)
        {
            var list = new List<MemberPair>();
            var members = type.GetMembers();

            foreach (var member in members)
            {
                var attributes = member.GetCustomAttributes();
                foreach (var attr in attributes)
                {
                    // more validation ?
                    // isExtendedAttribute ?
                    list.Add(new MemberPair(member, attr));
                }
            }
            return list;
        }

        public static List<MemberInfo> FindMembers<AttributeType>(BindingFlags flags = BindingFlags.Default) where AttributeType : Attribute
        {
            List<MemberInfo> found = new List<MemberInfo>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                found.AddRange(FindMembers<AttributeType>(assembly, flags));

            return found;
        }
        public static List<MemberInfo> FindMembers<T>(Assembly assembly, BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            List<MemberInfo> found = new List<MemberInfo>();
            foreach (var type in assembly.GetTypes())
                found.AddRange(FindMembers<T>(type, flags));

            return found;
        }
        public static List<MemberInfo> FindMembers<T>(Type type, BindingFlags flags = BindingFlags.Default) where T : Attribute
        {
            List<MemberInfo> found = new List<MemberInfo>();
            var members = type.GetMembers(flags);
            foreach (MemberInfo member in members)
            {
                var attribute = member.GetCustomAttribute<T>();
                if (attribute != null)
                    found.Add(member);
            }
            return found;
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
