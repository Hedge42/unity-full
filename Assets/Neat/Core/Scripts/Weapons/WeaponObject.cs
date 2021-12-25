using UnityEngine;

namespace Neat.DDD
{
    [CreateAssetMenu(menuName ="Neat/Weapon")]
    public class WeaponObject : ScriptableObject
    {
        public Weapon weapon;
        public GameObject prefab;
    }
}