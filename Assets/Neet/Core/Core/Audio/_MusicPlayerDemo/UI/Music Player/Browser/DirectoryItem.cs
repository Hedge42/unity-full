using System;
using Neet.Collections;

namespace Neet.Scroller
{
    public class DirectoryItem : IComparable
    {
        const char PATH_SEPARATOR = '\\';

        public string path { get; private set; }
        public int level { get; private set; }
        public bool isOpen { get; set; }
        public IList<DirectoryItem> children { get; private set; }

        public DirectoryItem(string path, int level, bool isOpen)
        {
            this.path = path;
            this.level = level;
            this.isOpen = isOpen;
            children = new List<DirectoryItem>();
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 0;

            // parse the parameter
            DirectoryItem param;
            if (obj.GetType() == typeof(DirectoryItem))
                param = (DirectoryItem)obj;
            else
                return 0;


            // https://docs.microsoft.com/en-us/dotnet/api/system.icomparable?view=netframework-4.8
            // LESS than zero if the parameter comes BEFORE
            // MORE than zero if the parameter comes AFTER

            // returns >0 if the parameter is a child of this but not the same
            if (param.ToString().Contains(path) && path.Length != param.ToString().Length)
                return 1;

            // returns <0 if the parameter is a parent of this but not the same
            if (path.Contains(param.ToString()) && path.Length != param.ToString().Length)
                return -1;

            // returns 0 in all other circumstances
            return 0;
        }
        public override string ToString()
        {
            return path;
        }
    }
}
