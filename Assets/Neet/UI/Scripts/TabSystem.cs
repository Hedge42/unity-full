using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Neet.UI
{
    // TabSystem is semi-automated.
    // Drag screens under the ScreenContainer
    // and let this script do the rest


#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TabSystem))]
    public class TabSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Initialize tabs"))
            {
                TabSystem _target = (TabSystem)target;
                _target.SpawnTabs();
                _target.SwitchToScreen(_target.currentScreen);
            }
        }
    }
#endif


    public class TabSystem : MonoBehaviour
    {
        // references
        public HorizontalLayoutGroup tabButtonLayoutGroup;
        public Button tabButtonPrefab;
        public RectTransform screenContainer;
        public int currentScreen;

        private List<Button> tabs;
        private List<RectTransform> _screens;
        public List<RectTransform> screens
        {
            get
            {
                if (_screens == null)
                    _screens = GetScreens();

                return _screens;
            }
            set
            {
                _screens = value;
            }
        }

        private void Start()
        {
            SpawnTabs();
            SwitchToScreen(currentScreen);
        }
        private void OnValidate()
        {
            // TODO: tabs not updated
            // editor script needed for error-free spawning and destroying

            if (currentScreen < 0)
                currentScreen = 0;
            if (currentScreen >= screens.Count)
                currentScreen = screens.Count - 1;

            ShowScreen(currentScreen);
        }

        public void SwitchToScreen(int screen)
        {
            ShowScreen(screen);
            ActivateTab(screen);
        }
        public void SwitchToScreen(string screenName)
        {
            var i = GetScreenIndex(screenName);
            SwitchToScreen(i);
        }

        internal void SpawnTabs()
        {
            tabs = new List<Button>();
            tabButtonLayoutGroup.transform.DestroyChildren();

            foreach (var s in screens)
            {
                Button b = Instantiate(tabButtonPrefab, tabButtonLayoutGroup.transform);
                b.onClick.AddListener(delegate { SwitchToScreen(s.name); });

                // the font size automatically changes after instantiating. idk why.
                var prefabFontSize = tabButtonPrefab.GetComponentInChildren<TextMeshProUGUI>().fontSize;
                var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
                tmp.fontSize = prefabFontSize;
                tmp.text = s.name;

                tabs.Add(b);
            }
        }
        private int GetScreenIndex(string screenName)
        {
            for (int i = 0; i < screens.Count; i++)
            {
                var s = screens[i];
                if (s.gameObject.name.Equals(screenName))
                {
                    return i;
                }
            }

            Debug.LogWarning("No screen found with name " + screenName);
            return default;
        }
        private void ActivateTab(int screen)
        {
            GetScreens();

            for (int i = 0; i < tabs.Count; i++)
            {
                if (i == screen)
                    UpdateColors(tabs[i]);
            }
        }
        private void UpdateColors(Button clicked)
        {
            foreach (var b in tabs)
            {
                var img = b.GetComponent<Image>();
                if (b == clicked)
                {
                    img.color = Color.white;
                    b.interactable = false;
                }
                else
                {

                    img.color = Color.white;
                    b.interactable = true;
                }
            }
        }
        private void ShowScreen(int screenIndex)
        {
            GetScreens();

            if (screenIndex < 0 || screenIndex >= screens.Count)
            {
                Debug.LogWarning("Sceen index " + screenIndex + " outside of range " + screens.Count);
                return;
            }
            for (int i = 0; i < screens.Count; i++)
            {
                if (screens[i] != null)
                {
                    currentScreen = screenIndex;
                    screens[i].gameObject.SetActive(i == screenIndex);
                }
            }

            // panel.UpdateSize(screens[currentScreen].GetComponent<VerticalLayoutGroup>());
        }
        private List<RectTransform> GetScreens()
        {
            screens = new List<RectTransform>();

            foreach (var t in screenContainer.GetChildren())
                screens.Add(t.GetComponent<RectTransform>());

            return screens;
        }
    }
}
