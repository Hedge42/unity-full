using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neat.Music;
using UnityEngine;

namespace Neat.Experimental
{
    public class ColorPaletteSerializer : Serializer<ColorPalette>
    {
        [SerializeField] public ColorPalette palette;

        [SerializeField] private FileSelector<ColorPalette> _file;
        public FileSelector<ColorPalette> file
        {
            get
            {
                if (_file == null || _file.ser != this)
                    _file = new FileSelector<ColorPalette>(this);
                return _file;
            }
        }
    }
}