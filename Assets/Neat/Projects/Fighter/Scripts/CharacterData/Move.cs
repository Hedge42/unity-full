using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    [System.Serializable]
    public class Move
    {
        public List<Hitbox> hitboxes;
        public List<StateChange> stateChanges;
        public List<VectorInterpolation> positionChanges;
        public List<VectorInterpolation> hurtboxChanges;

        public string name;
        public int frames;

        public string state;
        public string directions;
        public string buttons;

        public Move()
        {
            name = "Generic forward punch";
            frames = 5;
            state = "s";
            directions = "6";
            buttons = "p";

            hitboxes = new List<Hitbox>();
            stateChanges = new List<StateChange>();
            stateChanges.Add(new StateChange());
            stateChanges.Add(new StateChange() { attack = false, frame = frames });

            positionChanges = new List<VectorInterpolation>();
            hurtboxChanges = new List<VectorInterpolation>();
        }

        public void ValidateFrameBounds()
        {
            foreach (Hitbox h in hitboxes)
            {
                Fighter.ForceIntRange(ref h.startFrame, 0, frames);
                Fighter.ForceIntRange(ref h.endFrame, h.startFrame, frames);
                Fighter.ForceIntRange(ref h.disconnectFrame, h.startFrame, h.endFrame);

                foreach (var pos in h.ipPosition)
                {
                    Fighter.ForceIntRange(ref pos.startFrame, h.startFrame, frames);
                    Fighter.ForceIntRange(ref pos.endFrame, pos.startFrame, frames);
                }

                foreach (var size in h.ipSize)
                {
                    Fighter.ForceIntRange(ref size.startFrame, h.startFrame, frames);
                    Fighter.ForceIntRange(ref size.endFrame, size.startFrame, frames);
                }
            }

            foreach (StateChange s in stateChanges)
            {
                Fighter.ForceIntRange(ref s.frame, 0, frames);
            }

            foreach (var p in positionChanges)
            {
                Fighter.ForceIntRange(ref p.startFrame, 0, frames);
                Fighter.ForceIntRange(ref p.endFrame, p.startFrame, frames);
            }

            foreach (var s in hurtboxChanges)
            {
                Fighter.ForceIntRange(ref s.startFrame, 0, frames);
                Fighter.ForceIntRange(ref s.endFrame, s.startFrame, frames);
            }
        }

        public Vector2 GetPosition(int frame)
        {
            return VectorInterpolation.Calculate(frame, positionChanges);
        }

        public List<Hitbox> GetActiveHitboxes(int frame)
        {
            List<Hitbox> h = new List<Hitbox>();

            foreach (Hitbox b in hitboxes)
            {
                if (frame >= b.startFrame && frame <= b.endFrame)
                    h.Add(b);
            }

            return h;
        }

        public List<StateChange> GetActiveStates(int frame)
        {
            List<StateChange> s = new List<StateChange>();

            foreach (StateChange c in stateChanges)
            {
                ///if (frame >= b.startFrame && frame <= b.endFrame)
                    // h.Add(b);
            }

            return s;
        }
    }
}
