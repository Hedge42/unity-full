using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Guitar
{
	public class FretboardLineDrawer : MonoBehaviour
	{
		private RectTransform[] horizontalLines;
		public RectTransform stringLinePrefab;
		public RectTransform stringLineContainer;

		private RectTransform[] verticalLines;
		public RectTransform fretLinePrefab;
		public RectTransform fretLineContainer;

		private Fret[] frets { get { return ui.frets; } }
		private GuitarTuning tuning { get { return ui.tuning; } }
		private RectTransform gridRect { get { return ui.gridRect; } }
		private Canvas canvas { get { return ui.canvas; } }

		private FretboardUI _ui;
		private FretboardUI ui
		{
			get
			{
				if (_ui == null)
					_ui = GetComponent<FretboardUI>();
				return _ui;
			}
		}

		private void Start()
		{
			canvas.GetComponent<RectChangedHandler>().onChange +=
				delegate { UpdateHorizontalLines(); UpdateVerticalLines(); };
		}

		public void UpdateLines()
		{
			InstantiateHorizontalLines();
			UpdateHorizontalLines();
			ApplyStringDefaults();

			InstantiateVerticalLines();
			UpdateVerticalLines();
			ApplyBorderDefaults();
		}


		/// <summary>
		/// Destroys children and respawns lines from prefab
		/// </summary>
		private void InstantiateHorizontalLines()
		{
			horizontalLines = null;
			DestroyChildren(stringLineContainer);

			var lineList = new List<RectTransform>();

			for (int i = 0; i < tuning.numStrings; i++)
			{
				// instantiate the prefab
				var go = Instantiate(stringLinePrefab.gameObject, stringLineContainer);
				go.SetActive(true);

				lineList.Add(go.GetComponent<RectTransform>());
			}

			horizontalLines = lineList.ToArray();
		}

		/// <summary>
		/// Destroys children and respawns lines from prefab
		/// </summary>
		private void InstantiateVerticalLines()
		{
			verticalLines = null;
			DestroyChildren(fretLineContainer);

			var lineList = new List<RectTransform>();

			for (int i = 0; i < FretboardUI.MAX_FRETS - 1; i++)
			{
				var go = Instantiate(fretLinePrefab.gameObject, fretLineContainer);
				go.SetActive(true);

				lineList.Add(go.GetComponent<RectTransform>());
			}

			verticalLines = lineList.ToArray();
		}

		/// <summary>
		/// Sets position and size
		/// </summary>
		private void UpdateHorizontalLines()
		{
			for (int i = 0; i < horizontalLines.Length; i++)
			{
				// set the dimensions
				var idx = FretboardUI.MAX_FRETS * (i + 1); // +1 to skip border
				var fretRect = frets[idx].rect;
				var coords = fretRect.transform.position + Vector3.left * fretRect.rect.width / 2;
				var rect = horizontalLines[i];
				rect.position = (Vector3)coords;


				// don't I have a class for this?
				rect.sizeDelta = new Vector2(gridRect.sizeDelta.x, 1 / canvas.transform.localScale.y);


				// rect.position = gridRect.position
			}
		}

		/// <summary>
		/// Sets position and size
		/// </summary>
		private void UpdateVerticalLines()
		{
			for (int i = 0; i < verticalLines.Length; i++)
			{
				var rect = verticalLines[i];
				var fretRect = frets[i].rect;

				var posY = gridRect.position.y;
				var posX = fretRect.position.x + fretRect.sizeDelta.x / 2f * canvas.transform.localScale.x;
				rect.position = new Vector3(posX, posY);
				rect.sizeDelta = new Vector2(1 / canvas.transform.localScale.x, gridRect.rect.height);
			}
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

		/// <summary>
		/// Updates contents of playable cells
		/// FIXME don't set defaults
		/// </summary>
		private void ApplyStringDefaults()
		{
			int offset = FretboardUI.MAX_FRETS;
			for (int j = 0; j < FretboardUI.MAX_FRETS; j++)
			{
				for (int i = 0; i < tuning.numStrings; i++)
				{
					var fo = (FretObject)frets[offset + j + FretboardUI.MAX_FRETS * i];

					if (!Scale.HasNote(ui.scale.notes, fo.note))
						fo.displayMode = Fret.FretToggleMode.Hidden;

					fo.UpdateDisplay();
				}
			}
		}

		/// <summary>
		/// Updates border displays
		/// </summary>
		private void ApplyBorderDefaults()
		{
			// using default settings

			// +1 to include other border
			int offset = (tuning.numStrings + 1) * FretboardUI.MAX_FRETS;
			for (int i = 0; i < FretboardUI.MAX_FRETS; i++)
			{
				var fretNote = i % 12;

				if (fretNote == 0)
				{
					frets[i].displayMode = Fret.FretToggleMode.Emphasized;
					frets[i + offset].displayMode = Fret.FretToggleMode.Emphasized;
				}

				else if (fretNote == 3 || fretNote == 5 || fretNote == 7 || fretNote == 9)
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
}
