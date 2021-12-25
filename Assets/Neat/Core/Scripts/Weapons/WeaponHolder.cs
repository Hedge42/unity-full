using UnityEngine;

namespace Neat.DDD
{
    public class WeaponHolder : MonoBehaviour
    {
        public Weapon[] weapons;

        [Range(1, 5)]
        public int capacity;

        public void EquipWeapon(Weapon weapon)
        {
            Debug.Log("Equipping weapon using ik...");
        }
    }
}