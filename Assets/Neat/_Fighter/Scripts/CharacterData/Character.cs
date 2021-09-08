using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Fighter
{
    [System.Serializable]
    public class Character
    {
        public string name;
        public float walkSpeed;
        public int health;
        public List<Move> moves;

        public float sizeX;
        public float sizeY;

        public Vector2 size
        {
            get
            {
                return new Vector2(sizeX, sizeY);
            }
            set
            {
                sizeX = value.x;
                sizeY = value.y;
            }
        }

        public string[] GetMoveNames()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < moves.Count; i++)
                names.Add(i + ": " + moves[i].name);

            return names.ToArray();
        }

        public Character()
        {
            moves = new List<Move>();
            name = "EX name";
            health = Fighter.DEFAULT_HEALTH;
            size = new Vector2(Fighter.DEFAULT_SIZE_X, Fighter.DEFAULT_SIZE_Y);
            walkSpeed = Fighter.DEFAULT_WALKSPEED;
        }
    }
}
