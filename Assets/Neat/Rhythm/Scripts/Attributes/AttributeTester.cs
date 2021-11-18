using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

namespace Neat.Attributes
{
    [CreateAssetMenu(menuName="Neat/AttributeTester")]
    public class AttributeTester : ScriptableObject
    {
        [ExposedField]
        public static float s;
        
        void GetMembers()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<MemberInfo> exposedMembers = new List<MemberInfo>();

            var fields = new List<FieldInfo>();
            var fieldNames = new List<string>();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    BindingFlags flags = BindingFlags.Public;

                    MemberInfo[] members = type.GetMembers(flags);

                    foreach (MemberInfo member in members)
                    {
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            ExposedFieldAttribute attribute
                                = member.GetCustomAttribute<ExposedFieldAttribute>();

                            if (attribute != null)
                            {
                                fields.Add((FieldInfo)member);
                                fieldNames.Add($"{member.ReflectedType}/{attribute.instance}");
                            }
                        }
                    }
                }
            }
        }
    }
}
