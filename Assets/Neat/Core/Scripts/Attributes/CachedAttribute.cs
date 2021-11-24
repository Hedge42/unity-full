using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Attributes
{
    public class CachedAttribute : System.Attribute
    {
        public CachedAttribute attribute;

        // find the field using reflection
        // if it's null, set it using it's default constructor
    }
}
