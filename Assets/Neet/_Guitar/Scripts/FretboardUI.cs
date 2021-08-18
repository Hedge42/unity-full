using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FretboardUI : MonoBehaviour
{
    private const int MAX_FRETS = 25; // includes open

    public FretUI fretPrefab;
    public Fret[] frets;

    // public Color foregroundColor;
    // public Color backgroundColor;

    private Scale _scale;
    public Scale scale
    {
        get
        {
            if (_scale == null)
                _scale = new Scale();
            return _scale;
        }
        set
        {
            _scale = value;
        }
    }

    private GuitarTuning _tuning;
    public GuitarTuning tuning
    {
        get
        {
            if (_tuning == null)
                _tuning = new GuitarTuning();
            return _tuning;
        }
        set
        {
            _tuning = value;
        }
    }

    [HideInInspector] public int key;
    [HideInInspector] public int mode;

    [HideInInspector] public int minFret;
    [HideInInspector] public int maxFret;

    [HideInInspector] public bool preferFlats;
    public GridLayoutGroup gridLayout;
    private RectTransform gridRect;
    [HideInInspector] public Fret.BorderMode borderMode;
    [HideInInspector] public Fret.PlayableMode fretMode;

    private RectTransform[] stringLines;
    public RectTransform stringLinePrefab;
    public RectTransform stringLineContainer;

    private RectTransform[] fretLines;
    public RectTransform fretLinePrefab;
    public RectTransform fretLineContainer;

    public Canvas canvas;

    private void Start()
    {
        Display();
    }

    public void Display()
    {
        SetupFrets();
        SetupBorders();
        UpdateStrings();

        InitializeGrid();

        UpdateFretActivity();

        CreateStringLines();
        CreateFretLines();
        UpdateStringLines();
        UpdateFretLines();
    }

    private void InitializeGrid()
    {
        // to update in editor, otherwise would be updated next frame
        gridLayout.CalculateLayoutInputHorizontal();
        gridLayout.CalculateLayoutInputVertical();
        gridLayout.SetLayoutHorizontal();
        gridLayout.SetLayoutVertical();

        gridRect = gridLayout.GetComponent<RectTransform>();
        //gridBackground.position 

        canvas.GetComponent<RectChangedHandler>().onChange +=
            delegate { UpdateStringLines(); UpdateFretLines(); };
    }

    private void CreateStringLines()
    {
        stringLines = null;
        DestroyChildren(stringLineContainer);

        var lineList = new List<RectTransform>();

        for (int i = 0; i < tuning.numStrings; i++)
        {
            // instantiate the prefab
            var go = Instantiate(stringLinePrefab.gameObject, stringLineContainer);
            go.SetActive(true);

            lineList.Add(go.GetComponent<RectTransform>());
        }

        stringLines = lineList.ToArray();
    }
    private void CreateFretLines()
    {
        fretLines = null;
        DestroyChildren(fretLineContainer);

        var lineList = new List<RectTransform>();

        for (int i = 0; i < MAX_FRETS - 1; i++)
        {
            var go = Instantiate(fretLinePrefab.gameObject, fretLineContainer);
            go.SetActive(true);

            lineList.Add(go.GetComponent<RectTransform>());
        }

        fretLines = lineList.ToArray();
    }
    private void UpdateStringLines()
    {
        for (int i = 0; i < stringLines.Length; i++)
        {
            // set the dimensions
            var idx = MAX_FRETS * (i + 1); // +1 to skip border
            var fretRect = frets[idx].rect;
            var coords = fretRect.position + Vector3.left * fretRect.rect.width / 2;
            var rect = stringLines[i];
            rect.position = (Vector3)coords;
            rect.sizeDelta = new Vector2(gridRect.sizeDelta.x, 1 / canvas.transform.localScale.y);
        }
    }
    private void UpdateFretLines()
    {
        for (int i = 0; i < fretLines.Length; i++)
        {
            var rect = fretLines[i];
            var fretRect = frets[i].rect;

            var posY = gridRect.position.y;
            var posX = fretRect.position.x + fretRect.sizeDelta.x / 2f * canvas.transform.localScale.x;
            rect.position = new Vector3(posX, posY);
            rect.sizeDelta = new Vector2(1 / canvas.transform.localScale.x, gridRect.rect.height);
        }
    }

    private void UpdateFretActivity()
    {
        gridLayout.constraintCount = maxFret - minFret + 1;
        // disable frets outside of fret range
        foreach (Fret f in frets)
            f.mono.gameObject.SetActive(f.fretNum >= minFret && f.fretNum <= maxFret);
    }

    private void SetupFrets()
    {
        this.frets = null;
        DestroyChildren(gridLayout.transform);

        int max = 25;
        List<Fret> fretList = new List<Fret>();

        // border 1
        for (int i = 0; i < max; i++)
            fretList.Add(new BorderObject(fretPrefab, gridLayout.transform, this, i));

        // strings
        for (int i = 0; i < tuning.numStrings; i++)
        {
            for (int j = 0; j < max; j++)
            {
                fretList.Add(new FretObject(fretPrefab, gridLayout.transform, this, i, j));
            }
        }

        // border 2
        for (int i = 0; i < max; i++)
            fretList.Add(new BorderObject(fretPrefab, gridLayout.transform, this, i));

        this.frets = fretList.ToArray();
    }
    private void DestroyChildren(Transform t)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in t.transform)
            children.Add(child.gameObject);

        foreach (GameObject g in children)
        {
            if (Application.isPlaying)
                Destroy(g);
            else
                DestroyImmediate(g);
        }

    }

    private void UpdateBorders()
    {
        int offset = tuning.numStrings * MAX_FRETS;
        for (int i = 0; i <= MAX_FRETS; i++)
        {
            frets[i].UpdateDisplay();
            frets[i + offset].UpdateDisplay();
        }
    }
    private void UpdateStrings()
    {
        int offset = MAX_FRETS;
        for (int i = 0; i < tuning.numStrings; i++)
        {
            for (int j = 0; j < MAX_FRETS; j++)
            {
                var fo = (FretObject)frets[offset + j + MAX_FRETS * i];

                if (!Scale.HasNote(scale.notes, fo.note))
                    fo.displayMode = Fret.FretToggleMode.Hidden;

                fo.UpdateDisplay();
            }
        }
    }
    private void SetupBorders()
    {
        // using default settings

        // +1 to include other border
        int offset = (tuning.numStrings + 1) * MAX_FRETS;
        for (int i = 0; i < MAX_FRETS; i++)
        {
            var j = i % 12;

            if (j == 0)
            {
                frets[i].displayMode = Fret.FretToggleMode.Emphasized;
                frets[i + offset].displayMode = Fret.FretToggleMode.Emphasized;
            }

            else if (j == 3 || j == 5 || j == 7 || j == 9)
            {
                frets[i].displayMode = Fret.FretToggleMode.Normal;
                frets[i + offset].displayMode = Fret.FretToggleMode.Normal;
            }
            else
            {
                frets[i].displayMode = Fret.FretToggleMode.Hidden;
                frets[i + offset].displayMode = Fret.FretToggleMode.Hidden;
            }

            frets[i].UpdateDisplay();
            frets[i + offset].UpdateDisplay();
        }
    }
}

