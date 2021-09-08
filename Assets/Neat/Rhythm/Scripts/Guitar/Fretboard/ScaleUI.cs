using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
	public class ScaleUI : MonoBehaviour
	{
		FretboardUI fretboardUI;

		public void AddClickHandlers()
		{

		}

		public void ToggleFret(Fret fret, FretboardUI fretboard, PointerEventData eventData)
		{
			if (fret is FretObject)
				fretboard.FretClickedHandler(fret.fretNum, fret.rowIndex);

			if (eventData.button == PointerEventData.InputButton.Right)
			{
				fret.Hide();
			}
			else if (eventData.button == PointerEventData.InputButton.Left)
			{
				fret.ToggleMode();
			}
		}
	}
}
