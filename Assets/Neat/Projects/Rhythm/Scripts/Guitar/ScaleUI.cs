using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Audio.Music
{
	public class ScaleUI : MonoBehaviour
	{
		Fretboard fretboardUI;

		public void AddClickHandlers()
		{

		}

		public void ToggleFret(Fret fret, Fretboard fretboard, PointerEventData eventData)
		{
			if (fret is FretObject)
				fretboard.FretClickedHandler(fret.fretNum, fret.stringNum);

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
