using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neet.Fighter
{
    // ground or air
    // stand or crouch
    // command or passive
    // guard or not
    // stunned or not
    // down or active

    public enum xState
    {
        Ground, // or air
        Stand , // or crouch
        Attack, // or...not
        Stun, // or...not
        Guard, // or...not
        Knockdown, // or active
    }
    
    public enum State
    {
        Stand,
        Crouch,
        Air,
        Down,
        Attack,
        Stun,
        Guard
    }

    public enum Button
    {
        Punch,
        Kick,
        Special,
        Guard
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum AttackLevel
    {
        Low, // ignores stand block
        Mid, // always blockable
        High, // duckable
        Overhead // ignores low block
    }

    /// <summary>
    /// Contains base data and methods for fighting game stuff
    /// </summary>
    public static class Fighter
    {
        public const int INPUT_STREAM_LENGTH = 60;
        public const int BTN_BUFFER = 3;

        public const int DEFAULT_HEALTH = 1000;
        public const int DEFAULT_SIZE_X = 50;
        public const int DEFAULT_SIZE_Y = 100;
        public const int DEFAULT_WALKSPEED = 50;

        public const int P_BIT = (int)Button.Punch;
        public const int K_BIT = (int)Button.Kick;
        public const int S_BIT = (int)Button.Special;
        public const int G_BIT = (int)Button.Guard;

        public const int P_VALUE = 1 << P_BIT;
        public const int K_VALUE = 1 << K_BIT;
        public const int S_VALUE = 1 << S_BIT;
        public const int G_VALUE = 1 << G_BIT;

        // directional bit values used in command parsing
        public const int UP_BIT = (int)Direction.Up;
        public const int DOWN_BIT = (int)Direction.Down;
        public const int LEFT_BIT = (int)Direction.Left;
        public const int RIGHT_BIT = (int)Direction.Right;

        public const int UP_VALUE = 1 << UP_BIT;
        public const int DOWN_VALUE = 1 << DOWN_BIT;
        public const int LEFT_VALUE = 1 << LEFT_BIT;
        public const int RIGHT_VALUE = 1 << RIGHT_BIT;

        // states
        public const int STAND_BIT = (int)State.Stand;
        public const int CROUCH_BIT = (int)State.Crouch;
        public const int AIR_BIT = (int)State.Air;
        public const int DOWNED_BIT = (int)State.Down;
        public const int ATTACKING_BIT = (int)State.Attack;
        public const int STUN_BIT = (int)State.Stun;
        public const int GUARD_BIT = (int)State.Guard;

        public const int STAND_VALUE = 1 << STAND_BIT;
        public const int CROUCH_VALUE = 1 << CROUCH_BIT;
        public const int AIR_VALUE = 1 << AIR_BIT;
        public const int DOWNED_VALUE = 1 << DOWNED_BIT;
        public const int ATTACKING_VALUE = 1 << ATTACKING_BIT;
        public const int STUN_VALUE = 1 << STUN_BIT;
        public const int GUARD_VALUE = 1 << GUARD_BIT;

        public const int xSTAND_BIT = (int)xState.Stand;

        /// <summary>
        /// Converts numerical [1-9] notation to bit value
        /// </summary>
        public static int DirToBitValue(int dir)
        {
            int value = 0;

            if (dir >= 7) // 7,8,9
                value += UP_VALUE;
            else if (dir <= 3) // 1,2,3
                value += DOWN_VALUE;

            if (dir % 3 == 1) // 1,4,7
                value += LEFT_VALUE;
            else if (dir % 3 == 0) // 3,6,9
                value += RIGHT_VALUE;

            return value;
        }

        /// <summary>
        /// Converts bit value to numerical [1-9] notation
        /// </summary>
        public static int BitValueToDir(int value)
        {
            int dir = 5;

            if (value.IsBitOn(UP_BIT))
                dir += 3;
            if (value.IsBitOn(DOWN_BIT))
                dir -= 3;
            if (value.IsBitOn(LEFT_BIT))
                dir -= 1;
            if (value.IsBitOn(RIGHT_BIT))
                dir += 1;

            return dir;
        }

        public static string BitToButtonString(int bit)
        {
            if (bit == P_BIT)
                return "p";
            else if (bit == K_BIT)
                return "k";
            else if (bit == S_BIT)
                return "s";
            else if (bit == G_BIT)
                return "g";
            else
                return "?";
        }

        /// <summary>
        /// Converts button character (p,k,s,g) to bit value
        /// </summary>
        public static int BtnToBitValue(char c)
        {
            if (c == 'k')
                return K_VALUE;
            else if (c == 'p')
                return P_VALUE;
            else if (c == 's')
                return S_VALUE;
            else if (c == 'g')
                return G_VALUE;
            else
                return 0;
        }

        /// <summary>
        /// Converts bit value to button string "pksg"
        /// </summary>
        public static string BitValueToBtn(int value)
        {
            string s = "";
            if (value.IsBitOn(P_BIT))
                s += "p";
            if (value.IsBitOn(K_BIT))
                s += "k";
            if (value.IsBitOn(S_BIT))
                s += "s";
            if (value.IsBitOn(G_BIT))
                s += "g";
            return s;
        }

        /// <summary>
        /// Calculates 2^bit
        /// </summary>
        public static int BitToBitValue(int bit)
        {
            return 1 << bit;
        }

        /// <summary>
        /// True if a contains all active bits of b
        /// </summary>
        public static bool ContainsBits(this int a, int b)
        {
            int mismatch = a ^ b;
            return (mismatch & b) == 0;
        }

        /// <summary>
        /// Determines if the value has an active bit at the given position
        /// </summary>
        public static bool IsBitOn(this int value, int pos)
        {
            return ((value >> pos) & 1) == 1;
        }

        public static T Clone<T>(this T obj)
        {
            using (MemoryStream memory_stream = new MemoryStream())
            {
                // Serialize the object into the memory stream.
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memory_stream, obj);

                // Rewind the stream and use it to create a new object.
                memory_stream.Position = 0;
                return (T)formatter.Deserialize(memory_stream);
            }
        }

        public static void ForceIntRange(ref int i, int min, int max)
        {
            if (i < min)
                i = min;
            else if (i > max)
                i = max;
        }
    }
}
