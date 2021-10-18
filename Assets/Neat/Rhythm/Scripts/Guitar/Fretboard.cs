using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

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

        // components
        private Canvas _canvas;
        public Canvas canvas
        {
            get
            {
                if (_canvas == null)
                    _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }
        private ScaleSerializer _scale;
        private FretboardLineDrawer lineDrawer;
        [SerializeField] private ChartPlayer _player;


        // display preferences
        [HideInInspector] public int minFret;
        [HideInInspector] public int maxFret;
        [HideInInspector] public Fret.BorderMode borderMode;
        [HideInInspector] public Fret.PlayableMode fretMode;

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

        public MusicScale scale => _scale.scale;
        public event Action<int, int> onFretClicked;


        private TuningSerializer _tuningSerializer;
        public GuitarTuning tuning
        {
            get
            {
                if (_tuningSerializer == null)
                    _tuningSerializer = GetComponent<TuningSerializer>();
                return _tuningSerializer.tuning;
            }
        }

        private void Awake()
        {
            _tuningSerializer = GetComponent<TuningSerializer>();
            _scale = GetComponent<ScaleSerializer>();
            lineDrawer = GetComponent<FretboardLineDrawer>();
        }

        public void Generate()
        {
            // use notes in scale
            frets = InstantiateFrets().ToArray();
            UpdateGridLayout();
            UpdateLines();
            // ApplyFretRange();
            // UpdateGridLayout();
        }
        public void UpdateLines()
        {
            StartCoroutine(AfterFrame(lineDrawer.CreateLines));

            //if (Application.isPlaying)
            //    StartCoroutine(AfterFrame(lineDrawer.CreateLines));
            //else
            //    lineDrawer.CreateLines();

        }
        private IEnumerator AfterFrame(Action a)
        {
            // yield return new WaitForEndOfFrame();
            yield return null;
            a.Invoke();
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
        private List<Fret> InstantiateFrets()
        {
            this.frets = null;
            Destroyer.DestroyChildren<FretUI>(gridLayout.transform);

            List<Fret> fretList = new List<Fret>();

            // border 1
            for (int i = 0; i < MAX_FRETS; i++)
            {
                var f = new BorderObject(fretPrefab, gridLayout.transform, this, i);
                fretList.Add(f);
            }

            // strings
            for (int i = 0; i < tuning.numStrings; i++)
            {
                for (int j = 0; j < MAX_FRETS; j++)
                {
                    var f = new FretObject(fretPrefab, gridLayout.transform, this, i, j);
                    fretList.Add(f);
                }
            }

            // border 2
            for (int i = 0; i < MAX_FRETS; i++)
            {
                var f = new BorderObject(fretPrefab, gridLayout.transform, this, i);
                fretList.Add(f);
            }

            return fretList;
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
        private void UpdateGridLayout()
        {
            gridRect = gridLayout.GetComponent<RectTransform>();

            // to update in editor, otherwise would be updated next frame
            gridLayout.CalculateLayoutInputHorizontal();
            gridLayout.CalculateLayoutInputVertical();
            gridLayout.SetLayoutHorizontal();
            gridLayout.SetLayoutVertical();
        }
        private void ApplyFretRange()
        {
            // Updates grid constraint and enables/disables cells

            gridLayout.constraintCount = maxFret - minFret + 1;

            // disable frets outside of fret range
            foreach (Fret f in frets)
                f.mono.gameObject.SetActive(f.fretNum >= minFret && f.fretNum <= maxFret);
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