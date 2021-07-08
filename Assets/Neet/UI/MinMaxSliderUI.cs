using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neet.UI
{
    public class MinMaxSliderUI : MonoBehaviour
    {
        public Slider minSlider;
        public Slider maxSlider;

        public float minValue;
        public float maxValue;

        private void OnValidate()
        {
            ReplaceTransforms();
        }

        // https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
        T ReplaceComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.GetComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        void ReplaceTransforms()
        {
            var rect = GetComponent<RectTransform>();
            //minSlider.gameObject.Component

            ReplaceComponent<RectTransform>(rect, maxSlider.gameObject);
        }
    }
}
