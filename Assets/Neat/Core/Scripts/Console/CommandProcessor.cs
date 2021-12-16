using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    public class CommandProcessor : IProcessor<MethodInfo>
    {
        private static CommandProcessor _instance;
        //public static CommandProcessor instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new CommandProcessor();
        //        return _instance;
        //    }
        //}
        public static CommandProcessor instance => _instance ??= new CommandProcessor();

        public List<ConsoleCommand> commands;

        public CommandProcessor()
        {
            commands = new List<ConsoleCommand>();
        }

        public void Process(Type type)
        {
            foreach (var method in type.GetMethods())
            {
                Process(method);
            }

            Debug.Log(ToString());
        }
        public void Process(MethodInfo method)
        {
            var cmdAttr = method.GetCustomAttribute<CommandAttribute>(true);
            if (cmdAttr != null)
            {
                bool valid = true;
                var _params = method.GetParameters();
                foreach (var param in _params)
                {
                    if (!IsValid(param))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    var cmd = new ConsoleCommand(method, _params);
                    commands.Add(cmd);

                    // Debug.Log($"Added command: {cmd}");
                }
            }

            
        }
        private bool IsValid(ParameterInfo p)
        {
            return p.ParameterType.Equals(typeof(string));
        }

        public override string ToString()
        {
            var x = $"Loaded {commands.Count} commands: \n" 
                + string.Join('\n', commands.Select(c => c.ToString()));

            return x;
        }
    }
}