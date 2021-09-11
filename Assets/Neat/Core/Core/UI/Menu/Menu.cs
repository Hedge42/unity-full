using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.InputManagement
{
    /// <summary>
    /// There should only be one menu active at a time.
    /// Don't try to nest one menu inside of another.
    /// </summary>
    public class Menu : MonoBehaviour
    {
        public static List<Cursor> cursors;

        private MenuItem[] items;

        public MenuItem[] defaultCursorPositions;

        /// <summary>
        /// 0 = up, 1 = right, 2 = down, 3 = left
        /// shouldn't this be in on the MenuItem?
        /// </summary>
        public MenuItem GetNearestInDirection(Vector3 pos, int dir)
        {
            if (dir < 0 || dir > 3)
            {
                throw new System.ArgumentOutOfRangeException("Range should be [0, 3]");
            }

            float minDistance = float.MaxValue;
            MenuItem nearest = null;
            foreach (MenuItem item in items)
            {
                // ignore self
                if (item == this)
                    continue;


                // shorthand
                Vector3 itemPos = item.transform.position;

                bool isInDirection = false;
                if (dir == 0)
                    isInDirection = pos.y < itemPos.y;
                else if (dir == 1)
                    isInDirection = pos.x < itemPos.x;
                else if (dir == 2)
                    isInDirection = pos.y > itemPos.y;
                else if (dir == 3)
                    isInDirection = pos.x > itemPos.x;

                // ignore and move on
                if (!isInDirection)
                    continue;

                float distance = Vector3.Distance(pos, itemPos);

                // checking isinDirection is redundant
                if (isInDirection && distance < minDistance)
                {
                    minDistance = distance;
                    nearest = item;
                }
            }

            return nearest;
        }

        private void Awake()
        {
            if (cursors == null)
                cursors = new List<Cursor>();

            FindItems();
        }
        private void Start()
        {
        }
        private void FindItems()
        {
            items = transform.GetComponentsInChildren<MenuItem>();
        }
        public void Show()
        {
            SnapCursorsToDefaultPositions();
            Canvas c = GetComponent<Canvas>();
            c.enabled = true;
        }
        public void Hide()
        {
            Canvas c = GetComponent<Canvas>();
            c.enabled = false;
        }


        private void FindCursors()
        {

        }
        public void HideCursors()
        {
            foreach (Cursor c in cursors)
                c.gameObject.SetActive(false);
        }
        public void ShowCursors()
        {
            foreach (Cursor c in cursors)
                c.gameObject.SetActive(true);
        }

        private void SnapCursorsToDefaultPositions()
        {
            // sets the transform of existing cursors to be under this object

            // bring cursors to new menu
            // this is a mess
            int i = 0;
            foreach (Player p in PlayerManager.instance.players)
            {
                if (p == null)
                    continue;

                p.cursor.menu = this;

                if (i < defaultCursorPositions.Length)
                    p.cursor.MoveTo(defaultCursorPositions[i]);
                else
                    p.cursor.MoveTo(defaultCursorPositions[0]);
                i++;
            }
        }
    }
}