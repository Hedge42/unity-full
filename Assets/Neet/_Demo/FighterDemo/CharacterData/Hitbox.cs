using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    [System.Serializable]
    public class Hitbox
    {
        public int startFrame;
        public int endFrame;

        public int damage;
        public AttackLevel level;

        public float startSizeX;
        public float startSizeY;

        public float startPosX;
        public float startPosY;

        public float deltaSizeX;
        public float deltaSizeY;

        public float deltaPosX;
        public float deltaPosY;

        public bool disconnect;
        public int disconnectFrame;

        public Vector2 startSize
        {
            get
            {
                return new Vector2(startSizeX, startSizeY);
            }
            set
            {
                startSizeX = value.x;
                startSizeY = value.y;
            }
        }
        public Vector2 startPos
        {
            get
            {
                return new Vector2(startPosX, startPosY);
            }
            set
            {
                startPosX = value.x;
                startPosY = value.y;
            }
        }
        public Vector2 deltaSize
        {
            get
            {
                return new Vector2(deltaSizeX, deltaSizeY);
            }
            set
            {
                deltaSizeX = value.x;
                deltaSizeY = value.y;
            }
        }
        public Vector2 deltaPos
        {
            get
            {
                return new Vector2(deltaPosX, deltaPosY);
            }
            set
            {
                deltaPosX = value.x;
                deltaPosY = value.y;
            }
        }
       

        public List<VectorInterpolation> ipPosition;
        public List<VectorInterpolation> ipSize;

        public Hitbox()
        {
            ipSize = new List<VectorInterpolation>();
            ipPosition = new List<VectorInterpolation>();

            startSize = new Vector2(50, 100);
            startPos = new Vector2(70, 0);

            level = AttackLevel.High;
        }

        public Vector2 GetPosition(int frame)
        {
            return startPos + VectorInterpolation.Calculate(frame, ipPosition);
        }
        public Vector2 GetSize(int frame)
        {
            return startSize + VectorInterpolation.Calculate(frame, ipSize);
        }
    }
}
