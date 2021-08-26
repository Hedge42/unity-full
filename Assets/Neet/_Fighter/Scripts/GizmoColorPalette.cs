using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Fighter
{
    [CreateAssetMenu]
    public class GizmoColorPalette : ScriptableObject
    {
        public Color clrDefault = Color.white;

        public Color clrHitboxLow = Color.white;
        public Color clrHitboxMid = Color.white;
        public Color clrHitboxHigh = Color.white;
        public Color clrHitboxOverhead = Color.white;

        public Color clrStateAttack = Color.white;
        public Color clrStateStand = Color.white;
        public Color clrStateCrouch = Color.white;
        public Color clrStateDowned = Color.white;
        public Color clrStateAir = Color.white;
        public Color clrStateGuard = Color.white;

        public Color GetHitboxColor(AttackLevel a)
        {
            if (a == AttackLevel.High)
                return clrHitboxHigh;
            else if (a == AttackLevel.Mid)
                return clrHitboxMid;
            else if (a == AttackLevel.Low)
                return clrHitboxLow;
            else if (a == AttackLevel.Overhead)
                return clrHitboxOverhead;
            else
                return clrDefault;
        }

        public Color GetStateColor(State s)
        {
            if (s == State.Air)
                return clrStateAir;
            else if (s == State.Attack)
                return clrStateAttack;
            else if (s == State.Stand)
                return clrStateStand;
            else if (s == State.Crouch)
                return clrStateCrouch;
            else if (s == State.Down)
                return clrStateDowned;
            else if (s == State.Guard)
                return clrStateGuard;
            else
                return clrDefault;
        }
    }
}
