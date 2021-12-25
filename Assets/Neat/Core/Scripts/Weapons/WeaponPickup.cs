using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.DDD
{
    public class WeaponPickup : MonoBehaviour
    {
        private WeaponComponent weapon;

        public Vector3 rotationSpeed;
        public Vector2 floatSpeed; // frequency, amplitude

        private void OnEnable()
        {
            weapon = GetComponentInChildren<WeaponComponent>();
            var collider = GetComponentInChildren<MeshCollider>();
            // collider.
        }

        private void Update()
        {
            weapon.transform.Rotate(rotationSpeed * Time.deltaTime);
            var sin = floatSpeed.y * Mathf.Sin(floatSpeed.x); // amp * sin(freq)
            var dir = Vector3.up * Time.deltaTime;
            weapon.transform.Translate(sin * dir);
        }

        private void OnTriggerEnter(Collider other)
        {
            print(other.name);
            var holder = other.GetComponent<WeaponHolder>();

            if (holder != null)
            {
                holder.EquipWeapon(weapon.weapon);
            }
        }
    }
}