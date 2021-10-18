using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Neat.Music
{
	public abstract class Fret
	{
		// toggled by clicking
		public enum FretToggleMode
		{
			Normal,
			Hidden,
			Emphasized
		}
		public enum BorderMode
		{
			Dots,
			Numbers
		}
		public enum PlayableMode
		{
			Dot,
			Note,
			Interval,
			NoteAndInterval
		}

		public FretUI mono;
		public RectTransform rect;
		public FretToggleMode displayMode;
		public Fretboard fretboardUI;

		public int rowIndex; // which guitar string
		public int fretNum; // which fret

		public abstract void UpdateDisplay();

		public virtual void Hide()
		{
			displayMode = FretToggleMode.Hidden;
			UpdateDisplay();
		}
		public virtual void Show()
        {
			displayMode = FretToggleMode.Normal;
			UpdateDisplay();
        }
		public virtual void Emphasize()
        {
			displayMode = FretToggleMode.Emphasized;
			UpdateDisplay();
        }
		public virtual void ToggleMode()
		{
			if (displayMode == FretToggleMode.Normal)
				displayMode = FretToggleMode.Emphasized;
			else
				displayMode = FretToggleMode.Normal;
			UpdateDisplay();
		}
	}
}