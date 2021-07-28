using System.Collections.Generic;
using Neet.Input;
using UnityEngine;

namespace Neet.Input.Exp
{

    public class Cursor : MonoBehaviour
    {
        [Range(.1f, 20f)]
        public float sensitivity;
        float minimumDistanceToSnap;
        public List<Vector2> positions;

        private RectTransform cursorRt;
        private PlayerController pc;

        private void Awake()
        {
            cursorRt = GetComponent<RectTransform>();
            pc = GetComponent<PlayerController>();
        }
        private void Start()
        {
            if (positions != null && positions.Count > 0)
                cursorRt.localPosition = positions[0];
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (pc == null)
                return;

            if (pc.GetButtonDown(GamepadControl.DpadLeft))
                MoveLeft();
            if (pc.GetButtonDown(GamepadControl.DpadRight))
                MoveRight();
            if (pc.GetButtonDown(GamepadControl.DpadUp))
                MoveUp();
            if (pc.GetButtonDown(GamepadControl.DpadDown))
                MoveDown();

            float mainX = pc.GetAxis(GamepadControl.StickLeftX);
            float mainY = pc.GetAxis(GamepadControl.StickLeftY);
            float altX = pc.GetAxis(GamepadControl.StickRightX);
            float altY = pc.GetAxis(GamepadControl.StickRightY);

            Vector2 main = new Vector2(mainX, mainY);
            Vector2 alt = new Vector2(altX, altY);

            Translate(main + alt);
        }

        private void MoveDown()
        {
            // find next position immediately down

            // get all points below current position
            List<Vector2> positionsLower = new List<Vector2>();
            foreach (Vector2 p in positions)
                if (p.y < cursorRt.localPosition.y)
                    positionsLower.Add(p);

            // if there are multiple, get the closest one
            Vector2 nearest = GetNearest(positionsLower);

            MoveTo(nearest);
        }
        private void MoveUp()
        {
            // find next position immediately up

            // get all points above current cursor position
            List<Vector2> positionsHigher = new List<Vector2>();
            foreach (Vector2 p in positions)
                if (p.y > cursorRt.localPosition.y)
                    positionsHigher.Add(p);

            Vector2 nearest = GetNearest(positionsHigher);

            MoveTo(nearest);
        }
        private void MoveRight()
        {
            // find next position immediately to the right

            // get all points to the right of  current cursor position
            List<Vector2> positionsRight = new List<Vector2>();
            foreach (Vector2 p in positions)
                if (p.x > cursorRt.localPosition.x)
                    positionsRight.Add(p);

            Vector2 nearest = GetNearest(positionsRight);

            MoveTo(nearest);
        }
        private void MoveLeft()
        {
            // find next position immediately to the left

            // get all points to the right of current cursor position
            List<Vector2> positionsLeft = new List<Vector2>();
            foreach (Vector2 p in positions)
                if (p.x < cursorRt.localPosition.x)
                    positionsLeft.Add(p);

            Vector2 nearest = GetNearest(positionsLeft);

            MoveTo(nearest);
        }

        private Vector2 GetNearest(List<Vector2> items)
        {
            if (items.Count == 0)
                // travel to nearest object
                return cursorRt.localPosition; // but for now don't move

            Vector2 nearest = items[0];
            if (items.Count > 1)
            {
                float shortestDistance = Vector2.Distance(cursorRt.localPosition, nearest);
                for (int i = 1; i < items.Count; i++)
                {
                    float distance = Vector2.Distance(cursorRt.localPosition, items[i]);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearest = items[i];
                    }
                }
            }
            return nearest;
        }

        private void Translate(Vector2 distance)
        {
            cursorRt.localPosition += (Vector3)distance * sensitivity;
        }
        private void MoveTo(Vector2 location)
        {
            cursorRt.localPosition = location;

            print("Moving to " + location);
        }
    }
}
