using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Neet.File;

namespace Neet.Fighter
{
    [Serializable]
    public class CharacterDatabase
    {
        public static string path
        {
            get
            {
                return Application.dataPath
                    + "/Neet/_Demo/FighterDemo/CharacterData/characterData.sav";
            }
        }

        public List<Character> characters;

        public CharacterDatabase()
        {
            characters = new List<Character>();
        }

        public static CharacterDatabase Load()
        {
            FileManager.instance.DeserializeBinary(out CharacterDatabase cd, path);

            if (cd == default(CharacterDatabase))
                cd = new CharacterDatabase();

            return cd;
        }

        public void Save()
        {
            FileManager.instance.SerializeBinary(this, path);
        }
    }
}