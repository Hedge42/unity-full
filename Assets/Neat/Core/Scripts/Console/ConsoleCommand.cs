using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    

    public class ConsoleCommand
    {
        public MethodInfo method;
        public ParameterInfo[] parameters;

        public Action action;

        public override string ToString()
        {
            string[] names = parameters.Select(p => p.Name).ToArray();
            string _params = string.Join(',', names);
            var value = $"{method.Name} {_params} ";

            return value;
        }

        public ConsoleCommand(MethodInfo method, ParameterInfo[] _params)
        {
            this.parameters = _params;
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
