using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Neat.Music
{
    public class CutoutMask : Image
    {
        public override Material materialForRendering
        {
            get
            {
                Material mat = new Material(base.materialForRendering);
                mat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return mat;
            }
        }
    }
}
