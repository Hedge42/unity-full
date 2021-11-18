using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Neat.Extensions;

namespace Neat.Music
{
    [ExecuteAlways]
    public class Fretboard : MonoBehaviour
    {
        public const int MAX_FRETS = 25; // includes open

        // references
        public FretUI fretPrefab;
        public GridLayoutGroup gridLayout;
        public RectTransform gridRect;
        public RectTransform panel;
        public Vector2 size;

        private Canvas _canvas;
        public Canvas canvas => this.CacheGetComponent(ref _canvas);
        private FretboardDisplaySetting _displaySetting;
        public FretboardDisplaySetting displaySetting => this.CacheAddComponent(ref _displaySetting);
        private FretboardLineDrawer _lineDrawer;
        private FretboardLineDrawer lineDrawer => this.CacheGetComponent(ref _lineDrawer);

        // data
        private Fret[] _frets;
        public Fret[] frets
        {
            get
            {
                if (_frets == null)
                    Generate();
                return _frets;
            }
            private set
            {
                _frets = value;
            }
        }


        // calculated
        public Fret this [int _row, int _col] => frets[_col + _row * MAX_FRETS];
        public List<FretObject> playableFrets => frets.Where(f => f is FretObject).Cast<FretObject>().ToList();
        public List<BorderObject> borders => frets.Where(f => f is BorderObject).Cast<BorderObject>().ToList();

        // scales
        private ScaleSerializer _scaleSerializer;
        private ScaleSerializer scaleSerializer => this.CacheGetComponent(ref _scaleSerializer);
        public MusicScale scale
        {
            get => scaleSerializer.scale;
            set => scaleSerializer.scale = value;
        }

        // tuning
        private TuningSerializer _tuningSerializer;
        private TuningSerializer tuningSerializer => this.CacheGetComponent(ref _tuningSerializer);
        public GuitarTuning tuning => tuningSerializer.tuning;

        // grid
        private FretboardSpacer _spacer;
        public FretboardSpacer spacer => this.CacheAddComponent(ref _spacer);

        public event Action<int, int> onFretClicked;

        // generation
        public void Generate()
        {
            // use notes in scale
            frets = InstantiateFrets().ToArray();
            UpdateGridLayout();
            UpdateLines();
        }
        private List<Fret> InstantiateFrets()
        {
            this.frets = null;
            Destroyer.DestroyChildren<FretUI>(gridLayout.transform);

            List<Fret> fretList = new List<Fret>();

            fretList.AddRange(MakeBorder());
            fretList.AddRange(MakeAllStrings(tuning));
            fretList.AddRange(MakeBorder());

            return fretList;
        }
        private void UpdateGridLayout()
        {
            gridLayout.enabled = true;

            gridRect = gridLayout.GetComponent<RectTransform>();

            //gridLayout.cellSize = ...;

            var x = panel.sizeDelta.x / gridLayout.constraintCount;
            var y = panel.sizeDelta.y / (tuning.numStrings + 2);
            gridLayout.cellSize = new Vector2(x, y);

            // to update in editor, otherwise would be updated next frame
            gridLayout.CalculateLayoutInputHorizontal();
            gridLayout.CalculateLayoutInputVertical();
            gridLayout.SetLayoutHorizontal();
            gridLayout.SetLayoutVertical();
        }
        public void UpdateLines()
        {
            StartCoroutine(AfterFrame(lineDrawer.CreateLines));
        }
        private IEnumerator AfterFrame(Action a)
        {
            // yield return new WaitForEndOfFrame();
            yield return null;
            a.Invoke();
        }



        // spacer
        public void PreviewSpacing()
        {
            size = panel.sizeDelta;
            var grid = FretboardSpacer.MakeGrid(tuning, size);
            var xAxis = grid.Item1;
            var yAxis = grid.Item2;

            print($"X Positions: {xAxis}\n" +
                $"X Sizes: {String.Join(",", xAxis.Distances())})\n" +
                $"Y Positions: {yAxis}\n" +
                $"Y Sizes: {String.Join(",", yAxis.Distances())}");
        }
        public void DynamicSpacing()
        {
            gridLayout.enabled = false;

            size = panel.sizeDelta;

            spacer.Fix();

            UpdateLines();
        }


        public FretUI GetFret(Note n)
        {
            if (!Contains(n))
            {
                Debug.Log(n.ToString() + " not contained in fretboard");
                return null;
            }

            var index = n.fret + MAX_FRETS + (n.lane * MAX_FRETS);
            if (index < 0 || index >= frets.Length)
            {
                Debug.Log("Invalid index " + index + "/" + frets.Length);
                return null;
            }


            return frets[index].mono;
        }

        private Fret[,] InstantiateFrets2D()
        {
            throw new NotImplementedException();
            this.frets = null;
            Destroyer.DestroyChildren<FretUI>(gridLayout.transform);

            var f = new Fret[tuning.numStrings + 2, MAX_FRETS];

            // for (int i = 0; i < )
        }

        private List<BorderObject> MakeBorder()
        {
            var fretList = new List<BorderObject>();
            for (int i = 0; i < MAX_FRETS; i++)
            {
                var f = new BorderObject(fretPrefab, gridLayout.transform, this, i);
                fretList.Add(f);
            }
            return fretList;
        }
        private List<FretObject> MakeAllStrings(GuitarTuning tuning)
        {
            var list = new List<FretObject>();

            for (int _string = 0; _string < tuning.numStrings; _string++)
            {
                for (int _fret = 0; _fret < MAX_FRETS; _fret++)
                {
                    var f = new FretObject(fretPrefab, gridLayout.transform, this, _string, _fret);
                    list.Add(f);
                }
            }

            return list;
        }

        public bool Contains(Note n)
        {
            var laneOK = n.lane >= 0 && n.lane <= tuning.numStrings;
            var fretOK = n.fret < MAX_FRETS && n.fret >= 0;

            return laneOK && fretOK;
        }
        public void FretClickedHandler(int fret, int gString)
        {
            onFretClicked?.Invoke(fret, gString);
        }

        // ???
        private void UpdateBorders()
        {
            int offset = tuning.numStrings * MAX_FRETS;
            for (int i = 0; i <= MAX_FRETS; i++)
            {
                frets[i].UpdateDisplay();
                frets[i + offset].UpdateDisplay();
            }
        }
        private void PrintChildThing()
        {
            foreach (Transform child in gridRect.transform)
                print(child.position);
        }


        // ???
        public void ShowAll()
        {
            foreach (Fret f in frets)
            {
                if (f.GetType() == typeof(FretObject))
                {
                    f.displayMode = Fret.FretToggleMode.Normal;
                    f.UpdateDisplay();
                }
            }
        }
        public void HideAll()
        {
            foreach (Fret f in frets)
            {
                if (f.GetType() == typeof(FretObject))
                {
                    f.displayMode = Fret.FretToggleMode.Hidden;
                    f.UpdateDisplay();
                }
            }
        }
    }

}