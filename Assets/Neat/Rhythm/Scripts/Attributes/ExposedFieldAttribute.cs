using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Attributes
{
    // https://www.youtube.com/watch?v=vLKeqS1PeTU

    [AttributeUsage(AttributeTargets.Field)]

    public class ExposedFieldAttribute : System.Attribute
    {
        public string instance; 
        public ExposedFieldAttribute()
        {
        }

        // in editor script
        public static MemberInfo[] GetMembers<T>(BindingFlags flags)
        {
            return typeof(T).GetMembers(flags);
        }
        public void hi(BindingFlags flags)
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
                    MemberInfo[] members = type.GetMembers(flags);
                    foreach (MemberInfo member in members)
                    {
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            ExposedFieldAttribute attribute 
                                = member.GetCustomAttribute<ExposedFieldAttribute>();

                            if (attribute != null)
                            {
                                // do stuff
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
