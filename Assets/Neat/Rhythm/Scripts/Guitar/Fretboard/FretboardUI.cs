using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

namespace Neat.Music
{
	public class FretboardUI : MonoBehaviour
	{
		public const int MAX_FRETS = 25; // includes open

		private static FretboardUI _instance;
		public static FretboardUI instance
		{
			get
			{
				if (_instance == null)
					_instance = GameObject.FindObjectOfType<FretboardUI>();
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		private Fretboard _data;
		public Fretboard data
		{
			get
			{
				if (_data == null)
					_data = new Fretboard();
				return null;
			}
		}

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
		public RectTransform gridRect;
		[HideInInspector] public Fret.BorderMode borderMode;
		[HideInInspector] public Fret.PlayableMode fretMode;

		public RectTransform[] horizontalLines;
		public RectTransform stringLinePrefab;
		public RectTransform stringLineContainer;

		public RectTransform[] verticalLines;
		public RectTransform fretLinePrefab;
		public RectTransform fretLineContainer;

		public Canvas canvas;

		public event Action<int, int> onFretClicked;

		private FretboardLineDrawer lineDrawer;

		private void Start()
		{
			Load();
		}

		public void Load()
		{
			// UpdateGridLayout();

			InstantiateCells();

			ApplyFretRange();
			UpdateGridLayout();

			GetComponent<FretboardLineDrawer>().UpdateLines();
		}

		public void FretClickedHandler(int fret, int gString)
		{
			onFretClicked.Invoke(fret, gString);
		}

		private void UpdateGridLayout()
		{
			gridRect = gridLayout.GetComponent<RectTransform>();

			// to update in editor, otherwise would be updated next frame
			gridLayout.CalculateLayoutInputHorizontal();
			gridLayout.CalculateLayoutInputVertical();
			gridLayout.SetLayoutHorizontal();
			gridLayout.SetLayoutVertical();
		}

		/// <summary>
		/// Updates grid constraint and enables/disables cells
		/// </summary>
		private void ApplyFretRange()
		{
			gridLayout.constraintCount = maxFret - minFret + 1;

			// disable frets outside of fret range
			foreach (Fret f in frets)
				f.mono.gameObject.SetActive(f.fretNum >= minFret && f.fretNum <= maxFret);
		}

		/// <summary>
		/// Instantiates cells
		/// </summary>
		private void InstantiateCells()
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

		/// <summary>
		/// Destroys all children below t
		/// </summary>
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
	}

}