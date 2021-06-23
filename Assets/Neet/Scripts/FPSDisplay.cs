using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    // http://wiki.unity3d.com/index.php/FramesPerSecond

    private static FPSDisplay instance;

    [Range(1, 100)]
    public int fontSize;
    [Range(-2000f, 2000f)]
    public float xOffset, yOffset;
    public Color color;
    public TextAnchor anchor;
    public bool displayMilliseconds;

    private float deltaTime = 0.0f;

    private GUIStyle style;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        // called on awake or on editor change
        // what changes in the editor should be changed in this method
        // displayMilliseconds...
        // fontSize, alignment, color (Style)
        // width?

        // set GUIStyle here...
        style = new GUIStyle();

        style.alignment = anchor;
        style.fontSize = fontSize;
        style.normal.textColor = color;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        Rect rect = new Rect(xOffset, yOffset, w, h * 2 / 100);


        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = displayMilliseconds ? string.Format("{1:0.} fps ({0:0.0} ms)", msec, fps) : string.Format("{0:0.} fps", fps);
        GUI.Label(rect, text, style);

        // GUI.Label(rect, displayMilliseconds ? string.Format("{1:0.} fps ({0:0.0} ms)", deltaTime * 1000.0f, 1.0f / deltaTime) : string.Format("{0:0.} fps", 1.0f / deltaTime), style);
    }
}