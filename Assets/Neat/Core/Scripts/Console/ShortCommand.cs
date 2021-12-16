using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    public class ShortCommand
    {
        public MethodInfo method;

        public ShortCommand(MethodInfo method)
        {
            this.method = method;
        }

        public void TryInvoke(params string[] p)
        {
            try
            {
                method.Invoke(null, p);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed in invoke {method.Name}, {e.Message}");
            }
        }
    }
}
