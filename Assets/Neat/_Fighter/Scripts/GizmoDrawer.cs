using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    public class GizmoDrawer : MonoBehaviour
    {
        public GizmoColorPalette colors;

        [HideInInspector]
        public Vector3 localPreviewOrigin;

        [HideInInspector]
        public bool useTransformOrigin;

        [HideInInspector]
        public Character c;
        [HideInInspector]
        public Move m;
        [HideInInspector]
        public int frame;
        [HideInInspector]
        public bool active;

        public bool drawInPlayMode;

        private void OnDrawGizmos()
        {
            UpdateGizmosEditor();
        }

        public void SetPreviewState(Character c, Move m, int frame)
        {
            this.c = c;
            this.m = m;
            this.frame = frame;
        }

        private void UpdateGizmosEditor()
        {
            if (active || (Application.isPlaying && drawInPlayMode))
            {
                Vector3 origin = transform.TransformPoint(localPreviewOrigin);

                if (c != null)
                {
                    // just draw the character
                    if (m == null)
                    {
                        Gizmos.color = colors.clrDefault;
                        Gizmos.DrawWireCube(origin, c.size);
                    }

                    // draw using move data
                    else
                    {
                        var characterPos = origin + (Vector3)m.GetPosition(frame);

                        Gizmos.color = colors.clrDefault;
                        Gizmos.DrawWireCube(characterPos, c.size);
                        Gizmos.DrawLine(origin, characterPos);

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

                            Gizmos.color = colors.GetHitboxColor(h.level);
                            Gizmos.DrawWireCube(pos, size);
                        }
                    }
                }
            }
        }
    }
}