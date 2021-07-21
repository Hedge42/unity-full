using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;

namespace Neet.Fighter
{
    public class CharacterEditorComponent : MonoBehaviour
    {
        public Vector3 localPreviewOrigin;
        public Color startColor;
        public Color movementColor;
        public Color hitboxColor;

        [HideInInspector]
        public Character c;
        [HideInInspector]
        public Move m;
        [HideInInspector]
        public int frame;
        [HideInInspector]
        public bool active;

        private void OnDrawGizmos()
        {
            if (active)
            {
                Vector3 origin = transform.TransformPoint(localPreviewOrigin);

                if (c != null)
                {
                    Gizmos.color = startColor;
                    Gizmos.DrawWireCube(origin, c.size);

                    if (m != null)
                    {
                        var characterPos = origin + (Vector3)m.GetPosition(frame);

                        Gizmos.color = movementColor;
                        Gizmos.DrawWireCube(characterPos, c.size);

                        Gizmos.color = hitboxColor;
                        foreach (var h in m.GetActiveHitboxes(frame))
                        {
                            Vector3 pos = Vector3.zero;
                            if (h.disconnect && h.disconnectFrame <= frame)
                            {
                                // get position at disconnect frame

                                var localPosAtDisconnect = 
                                    (Vector3)h.GetPosition(h.disconnectFrame);
                                var localDeltaPosAfterDisconnect = 
                                    (Vector3)h.GetPosition(frame) - localPosAtDisconnect;

                                var characterPosAtDisconnect = 
                                    origin + (Vector3)m.GetPosition(h.disconnectFrame);

                                var posAtDisconnect = characterPosAtDisconnect +
                                    localPosAtDisconnect;

                                pos = posAtDisconnect + localDeltaPosAfterDisconnect;
                            }
                            else
                            {
                                pos = characterPos + (Vector3)h.GetPosition(frame);
                            }

                            var size = h.GetSize(frame);

                            Gizmos.DrawWireCube(pos, size);
                        }
                    }
                }
            }
        }


        public void SetState(Character c, Move m, int frame)
        {
            this.c = c;
            this.m = m;
            this.frame = frame;
        }
    }
}
