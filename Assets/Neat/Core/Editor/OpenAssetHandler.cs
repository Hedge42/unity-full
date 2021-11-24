using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;
using UnityEditor;
using UnityEditor.Callbacks;
using Neat.Tutorials;

namespace Neat.Tools
{
    public class AssetHandler
    {
        [OnOpenAsset()]
        public static bool OpenEditor(int instanceId, int line)
        {
            GameDataObject obj = EditorUtility.InstanceIDToObject(instanceId) as GameDataObject;

            if (obj != null)
            {
                ExtendedEditorWindow.Open(obj);
                return true;
            }
            return false;
        }
    }
}