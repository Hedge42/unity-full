using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
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
            positionChanges = new List<VectorInterpolation>();
            hurtboxChanges = new List<VectorInterpolation>();
        }

        public void ValidateFrameBounds()
        {
            foreach (Hitbox h in hitboxes)
            {
                if (h.startFrame > frames)
                    h.startFrame = frames;
                if (h.endFrame > frames)
                    h.endFrame = frames;

                if (h.disconnectFrame < h.startFrame)
                    h.disconnectFrame = h.startFrame;
                else if (h.disconnectFrame > h.endFrame)
                    h.disconnectFrame = h.endFrame;

                foreach (var pos in h.ipPosition)
                {
                    if (pos.startFrame < h.startFrame)
                        pos.startFrame = h.startFrame;
                    if (pos.endFrame > h.endFrame)
                        pos.endFrame = h.endFrame;
                }

                foreach (var size in h.ipSize)
                {
                    if (size.startFrame < h.startFrame)
                        size.startFrame = h.startFrame;
                    if (size.endFrame > h.endFrame)
                        size.endFrame = h.endFrame;
                }
            }

            foreach (StateChange s in stateChanges)
            {
                if (s.frame < 0)
                    s.frame = 0;
                else if (s.frame > frames)
                    s.frame = frames;
            }

            foreach (var p in positionChanges)
            {
                if (p.startFrame < 0)
                    p.startFrame = 0;
                else if (p.startFrame > frames)
                    p.startFrame = frames;

                if (p.endFrame < p.startFrame)
                    p.endFrame = p.startFrame;
                if (p.endFrame > frames)
                    p.endFrame = frames;
            }

            foreach (var s in hurtboxChanges)
            {
                if (s.startFrame < 0)
                    s.startFrame = 0;
                else if (s.startFrame > frames)
                    s.startFrame = frames;

                if (s.endFrame < s.startFrame)
                    s.endFrame = s.startFrame;
                else if (s.endFrame > frames)
                    s.endFrame = frames;
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
    }
}
