using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Experimental;

namespace Neat.Music
{
    public class TestSerializer : Serializer<FileTesting>
    {
        public FileSelector_ _selector;
    }

    [System.Serializable]
    public class FileTesting
    {
        [Range(0, 10)]
        public float a;
        public float b;
        public float c;
    }
}
