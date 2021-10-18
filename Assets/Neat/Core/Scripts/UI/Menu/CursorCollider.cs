using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.InputManagement
{
    public class CursorCollider : MonoBehaviour
    {
        private Cursor cursor;

        private void Awake()
        {
            cursor = GetComponentInParent<Cursor>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            print("Collided with " + collision.gameObject.name);
        }
    }
}