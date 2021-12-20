using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    public class FighterMotor : MonoBehaviour
    {
        public float TransformSpeed => TransformVelocity.magnitude;
        public Vector3 TransformVelocity = Vector3.zero;

        private Vector3 lastPosition;
        public Vector3 DeltaPosition { get; private set; }
        public float DeltaDistance { get; private set; }

        public void Walk(Character c, InputEvent last)
        {
            TransformVelocity = GetDirectionVector(last) * c.walkSpeed;

            transform.position += TransformVelocity * Time.fixedDeltaTime;

            UpdateDelta();
        }

        public void HandleGravity(Character c)
        {

        }

        private Vector2 GetDirectionVector(InputEvent e)
        {
            if (e.dirBit.IsBitOn(Fighter.LEFT_BIT))
                return Vector2.left;
            else if (e.dirBit.IsBitOn(Fighter.RIGHT_BIT))
                return Vector2.right;
            else
                return Vector2.zero;
        }

        private void UpdateDelta()
        {
            DeltaPosition = transform.position - lastPosition;
            DeltaDistance = DeltaPosition.magnitude;
            lastPosition = transform.position;
        }
    }
}
