using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    [CreateAssetMenu]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Character> _characters;
        public List<Character> characters
        {
            get
            {
                if (_characters == null)
                    _characters = new List<Character>();
                return _characters;
            }
            set
            {
                _characters = value;
            }
        }

        public string[] GetCharacterNames()
        {
            List<string> names = new List<string>();

            for (int i = 0; i < characters.Count; i++)
                names.Add(i + ": " + characters[i].name);

            return names.ToArray();
        }

        public bool GetCharacterNames(out string[] names)
        {
            names = null;
            try
            {
                names = GetCharacterNames();

                return names.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
