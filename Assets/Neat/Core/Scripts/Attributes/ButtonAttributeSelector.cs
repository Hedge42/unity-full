using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Attributes
{
    [Serializable]
    public class ButtonAttributeSelector // : UnityEditor.SerializedProperty
    {
        // this becomes the serializedObject
        public Action method;

        public void Invoke()
        {
            method?.Invoke();
        }
    }
}