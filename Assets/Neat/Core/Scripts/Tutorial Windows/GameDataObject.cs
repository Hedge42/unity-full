using UnityEditor;
using UnityEngine;
using Neat.Attributes;
using UnityEditor.Callbacks;

namespace Neat.Tutorials
{
    public class GameDataObject : MonoBehaviour
    {
        [BeginDisabledGroup, Range(0, 10)]
        public int thing;
        [EndDisabledGroup]

        [MinMax(0, 10)]
        public Vector2 minMax;

        // [Sidebar]
        public GameData[] gameData;
    }

    [System.Serializable]
    public class GameData
    {
        public string data;
    }
}
