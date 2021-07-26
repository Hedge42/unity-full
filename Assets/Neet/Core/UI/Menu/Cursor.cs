using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Input;
using UnityEngine.UI;

namespace Neet.Input
{
    /// <summary>
    /// While there is only 1 cursor, the master device is used.
    /// But there may be 2+ in a place like a character selector
    /// </summary>
    /// 

    public class Player
    {
        public struct Data
        {
            public Color color;
        }


        public Cursor cursor;
        public Device device;
        public Data data;
    }

    public class Cursor : MonoBehaviour
    {
        [HideInInspector]
        public Player player;

        public float speed;

        public Image sprite;

        // should be used to transfer the cursor to a new menu
        public Menu menu;

        private MenuItem highlighted;

        private void Awake()
        {
        }
        private void Start()
        {

            Player p = GetComponentInParent<Player>();
            if (p != null)
                player = p;

            UpdateColor();
            IgnoreCursorCollisions();
        }
        private void Update()
        {
            GetInput();
        }
        private void IgnoreCursorCollisions()
        {
            // automated collision ignoring
            foreach (Player p in PlayerManager.instance.players)
            {
                if (p == null)
                    continue;

                Physics2D.IgnoreCollision(
                GetComponentInChildren<Collider2D>(),
                p.cursor.GetComponentInChildren<Collider2D>());
            }
        }
        private void GetInput()
        {
            if (player.device == null || menu == null)
                return;

            if (player.device.GetButtonDown(GamepadControl.DpadUp))
                SnapInDirection(0);
            if (player.device.GetButtonDown(GamepadControl.DpadRight))
                SnapInDirection(1);
            if (player.device.GetButtonDown(GamepadControl.DpadDown))
                SnapInDirection(2);
            if (player.device.GetButtonDown(GamepadControl.DpadLeft))
                SnapInDirection(3);

            // smooth move
            Vector3 movement = new Vector3(player.device.GetAxis(GamepadControl.StickLeftX),
                player.device.GetAxis(GamepadControl.StickLeftY), 0);
            Move(movement);

            if (player.device.GetButtonDown(GamepadControl.FaceBottom)
                && highlighted != null)
                highlighted.Select();
        }
        public void UpdateColor()
        {
            // this line breaks
            sprite.color = player.data.color;
        }

        /// <summary>
        /// 0=up, 1=right, 2=down, 3=left
        /// </summary>
        public void SnapInDirection(int dir)
        {
            if (dir < 0 || dir > 3)
                throw new System.ArgumentException("0=up, 1=right, 2=down, 3=left");

            MenuItem nearest = menu.GetNearestInDirection(transform.position, dir);
            if (nearest != null)
                MoveTo(nearest);
        }

        /// <summary>
        /// Moves to and selects the menuITem
        /// </summary>
        /// <param name="item"></param>
        public void MoveTo(MenuItem item)
        {
            highlighted = item;
            transform.SetParent(item.transform);
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        }

        /// <summary>
        /// Smooth move
        /// </summary>
        public void Move(Vector3 input)
        {
            if (transform.parent != menu.transform)
                transform.SetParent(menu.transform);

            transform.localPosition += input * speed;

            Vector2 rect = menu.GetComponent<RectTransform>().rect.size;


            float maxX = rect.x / 2;
            float maxY = rect.y / 2;

            // fix bounds
            if (transform.localPosition.x > maxX)
                transform.localPosition = new Vector3(maxX, transform.localPosition.y, transform.localPosition.z);
            else if (transform.localPosition.x < -maxX)
                transform.localPosition = new Vector3(-maxX, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.y > maxY)
                transform.localPosition = new Vector3(transform.localPosition.x, maxY, transform.localPosition.z);
            else if (transform.localPosition.y < -maxY)
                transform.localPosition = new Vector3(transform.localPosition.x, -maxY, transform.localPosition.z);
        }

        public void Highlight(MenuItem item)
        {
            highlighted = item;
            item.GetComponent<ImageFlasher>()?.StartFlashing();
        }
        public void DeHighlight(MenuItem item)
        {
            if (highlighted == item)
                highlighted = null;

            item.GetComponent<ImageFlasher>()?.StopFlashing();
        }
    }
}
