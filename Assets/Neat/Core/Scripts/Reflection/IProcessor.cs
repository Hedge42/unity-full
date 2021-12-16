using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Neat.Tools
{
    public interface IProcessor<T>
    {
        void Process(T info);
    }
    public class _MemberProcessor : IProcessor<MemberInfo>
    {
        public void Process(MemberInfo info)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMemberProcessor
    {
        void Process(MemberInfo value);
    }
    public interface ITypeProcessor
    {
        void Process(Type type);
    }
}
