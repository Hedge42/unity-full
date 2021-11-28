using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    [CreateAssetMenu]
    public class MoveDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Move> _moves;
        public List<Move> moves
        {
            get
            {
                if (_moves == null)
                    _moves = new List<Move>();
                return _moves;
            }
            set
            {
                _moves = value;
            }
        }

        public string[] GetMoveNames()
        {
            List<string> names = new List<string>();

            for (int i = 0; i < moves.Count; i++)
                names.Add(i + ": " + moves[i].name);

            return names.ToArray();
        }

        public bool GetMoveNames(out string[] names)
        {
            names = null;
            try
            {
                names = GetMoveNames();

                return names.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}