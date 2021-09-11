﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Neat.Music
{
	public class FretObject : Fret
	{
		public int stringOpenNote { get { return fretboardUI.tuning.values[rowIndex]; } }
		public int note { get { return stringOpenNote + fretNum; } }

		public FretObject(FretUI prefab, Transform container, FretboardUI tab, int stringIndex, int fret)
		{
			var mono = GameObject.Instantiate(prefab, container);
			mono.gameObject.SetActive(true);
			mono.fret = (Fret)this;

			this.mono = mono;
			this.rect = mono.GetComponent<RectTransform>();
			this.rowIndex = stringIndex;
			this.fretNum = fret;
			this.fretboardUI = tab;

			this.mono.borderDot1.gameObject.SetActive(false);
			this.mono.borderDot2.gameObject.SetActive(false);
		}

		public override void UpdateDisplay()
		{
			var isHidden = displayMode == FretToggleMode.Hidden;
			var isEmphasized = displayMode == FretToggleMode.Emphasized;

			var isNote = fretboardUI.fretMode == Fret.PlayableMode.Note;
			var isInterval = fretboardUI.fretMode == Fret.PlayableMode.Interval;
			var isBoth = fretboardUI.fretMode == Fret.PlayableMode.NoteAndInterval;
			var isDots = fretboardUI.fretMode == Fret.PlayableMode.Dot;

			mono.borderDot1.gameObject.SetActive(!isHidden && isDots);
			mono.tmp.gameObject.SetActive(!isHidden && !isDots);

			if (!isHidden && !isDots)
			{
				string s = "";

				if (isNote || isBoth)
					s = MusicScale.NoteValueToName(note, fretboardUI.preferFlats, false);

				if (isBoth)
					s += "";

				if (isInterval || isBoth)
					s += MusicScale.NoteToIntervalString(fretboardUI.scale.notes, note, fretboardUI.preferFlats);

				mono.tmp.text = s;
				if (isEmphasized)
					mono.tmp.fontStyle = FontStyles.Bold;
				else
					mono.tmp.fontStyle = FontStyles.Normal;
			}

		}
	}
}