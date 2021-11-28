using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Audio.Synthesis
{
    public interface IFilter
    {
        float Filter(float f);
    }
}
