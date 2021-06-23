using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCanvasSwitcher : MonoBehaviour
{
    public int comeFrom;
    public int goTo;

    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Go);
    }
    private void Go()
    {
        // CanvasNavigator.instance.Activate(comeFrom, goTo);
    }
}
