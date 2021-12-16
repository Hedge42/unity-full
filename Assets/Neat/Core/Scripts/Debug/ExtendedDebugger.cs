using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Debug = Neat.Debugger.Debug;


using Object = UnityEngine.Object;

namespace Neat.Debugger
{
    public static class Debug
    {
        public static void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
        public static void Log(string message, Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }
        public static void Log(this object o)
        {
            Log(o.ToString());
        }
        public static void Log(this Object context)
        {
            Log(context.ToString(), context);
        }
    }
}
