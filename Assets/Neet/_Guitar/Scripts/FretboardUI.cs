using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FretboardUI : MonoBehaviour
{
    private const int MAX_FRETS = 25; // includes open

    public FretUI fretPrefab;
    public Fret[] frets;

    public Color foregroundColor;
    public Color backgroundColor;

    [HideInInspector] public Scale scale;
    [HideInInspector] public GuitarTuning tuning;

    [HideInInspector] public int key;
    [HideInInspector] public int mode;

    [HideInInspector] public int minFret;
    [HideInInspector] public int maxFret;

    public bool preferFlats;
    public GridLayoutGroup grid;
    public Fret.BorderMode borderMode;
    public Fret.PlayableMode fretMode;

    private void Start()
    {
        Display();
    }

    public void Display()
    {
        tuning = new GuitarTuning();
        scale = new Scale(key, mode);

        SetupFrets();

        SetupBorders();
        UpdateStrings();

        UpdateFretActivity();
    }

    private void UpdateFretActivity()
    {
        grid.constraintCount = maxFret - minFret + 1;
        // disable frets outside of fret range
        foreach (Fret f in frets)
            f.mono.gameObject.SetActive(f.fretNum >= minFret && f.fretNum <= maxFret);
    }

    private void SetupFrets()
    {
        ClearFrets();

        int max = 25;
        List<Fret> frets = new List<Fret>();

        // border 1
        for (int i = 0; i < max; i++)
            frets.Add(new BorderObject(fretPrefab, grid.transform, this, i));

        // strings
        for (int i = 0; i < tuning.numStrings; i++)
        {
            for (int j = 0; j < max; j++)
            {
                frets.Add(new FretObject(fretPrefab, grid.transform, this, i, j));
            }
        }

        // border 2
        for (int i = 0; i < max; i++)
            frets.Add(new BorderObject(fretPrefab, grid.transform, this, i));

        this.frets = frets.ToArray();
    }
    private void ClearFrets()
    {
        frets = null;

        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in grid.transform)
            children.Add(t.gameObject);

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

