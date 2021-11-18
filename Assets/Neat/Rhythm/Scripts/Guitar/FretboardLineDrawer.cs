using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Neat.Music
{
	[ExecuteAlways]
	public class FretboardLineDrawer : MonoBehaviour
	{
		private RectTransform[] horizontalLines;
		public RectTransform horizontalLinePrefab;
		public RectTransform horizontalLineContainer;

		private RectTransform[] verticalLines;
		public RectTransform verticalLinePrefab;
		public RectTransform verticalLineContainer;

		private Fret[] frets { get { return ui.frets; } }
		private GuitarTuning tuning { get { return ui.tuning; } }
		private RectTransform gridRect { get { return ui.gridRect; } }
		private Canvas canvas { get { return ui.canvas; } }

		private Fretboard _ui;
		private Fretboard ui
		{
			get
			{
				if (_ui == null)
					_ui = GetComponent<Fretboard>();
				return _ui;
			}
		}

		private void Awake()
		{
			canvas.GetComponent<RectChangedHandler>().onChange +=
				UpdateLinePositions;
		}

        public void CreateLines()
		{
			InstantiateHorizontalLines();
			UpdateHorizontalLines();
			ApplyStringDefaults();

			InstantiateVerticalLines();
			UpdateVerticalLines();
			ApplyBorderDefaults();
		}

		private void InstantiateHorizontalLines()
		{
			horizontalLines = null;

			Destroyer.DestroyChildren<PixelSizeAdjuster>(horizontalLineContainer);

			var lineList = new List<RectTransform>();

			for (int i = 0; i < tuning.numStrings; i++)
			{
				// instantiate the prefab
				var go = Instantiate(horizontalLinePrefab.gameObject, horizontalLineContainer);
				go.SetActive(true);

				lineList.Add(go.GetComponent<RectTransform>());
			}

			horizontalLines = lineList.ToArray();
		}
		private void InstantiateVerticalLines()
		{
			verticalLines = null;
			Destroyer.DestroyChildren<PixelSizeAdjuster>(verticalLineContainer);

			var lineList = new List<RectTransform>();

			for (int i = 0; i < Fretboard.MAX_FRETS - 1; i++)
			{
				var go = Instantiate(verticalLinePrefab.gameObject, verticalLineContainer);
				go.SetActive(true);

				lineList.Add(go.GetComponent<RectTransform>());
			}

			verticalLines = lineList.ToArray();
		}

		private void UpdateLinePositions()
        {
			UpdateHorizontalLines();
			UpdateVerticalLines();
        }
		private void UpdateHorizontalLines()
		{
			// ???
			if (horizontalLines == null)
				return;

			var leftmostPlayable = ui.playableFrets.Where(o => o.fretNum == 0);

			for (int i = 0; i < horizontalLines.Length; i++)
			{
				// set the dimensions
				var idx = Fretboard.MAX_FRETS * (i + 1); // +1 to skip border
				var fretRect = frets[idx].rect;
				var coords = fretRect.transform.position + Vector3.left * fretRect.rect.width / 2;
				var rect = horizontalLines[i];
				rect.position = (Vector3)coords;
			}
		}
		private void UpdateVerticalLines()
		{
			if (verticalLines == null)
				return;

			for (int i = 0; i < verticalLines.Length; i++)
			{
				var rect = verticalLines[i];
				var fretRect = frets[i].rect;

				var posY = gridRect.position.y;
				var posX = fretRect.position.x + fretRect.sizeDelta.x / 2f * canvas.transform.localScale.x;
				rect.position = new Vector3(posX, posY);
			}
		}

		private void ApplyStringDefaults()
		{
			int offset = Fretboard.MAX_FRETS;
			for (int j = 0; j < Fretboard.MAX_FRETS; j++)
			{
				for (int i = 0; i < tuning.numStrings; i++)
				{
					var fo = (FretObject)frets[offset + j + Fretboard.MAX_FRETS * i];

					if (!MusicScale.HasNote(ui.scale.notes, fo.note))
						fo.displayMode = Fret.FretToggleMode.Hidden;

					fo.UpdateDisplay();
				}
			}
		}
		private void ApplyBorderDefaults()
		{
			// using default settings

			// +1 to include other border
			int offset = (tuning.numStrings + 1) * Fretboard.MAX_FRETS;
			for (int i = 0; i < Fretboard.MAX_FRETS; i++)
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
