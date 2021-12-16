using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Tools
{
    public class CommandAttribute : Attribute
    {
        public string id;
        public string desc;

        public CommandAttribute(string id, string desc)
        {
            this.id = id;
            this.desc = desc;
        }


        [Command("test1", "does a test command lol")]
        public static void TestCommand1()
        {
            UnityEngine.Debug.Log("nice");
        }

        [Command("test2", "this one has params")]
        public static void ThisIsAnotha(string one, string two)
        {
            UnityEngine.Debug.Log($"Like {one} {two}");
        }
    }
}
