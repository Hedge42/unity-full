using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

namespace Neat.Audio.Music
{
	public class FretObject : Fret
	{
		public int stringOpenNote { get { return fretboardUI.tuning.values[stringNum]; } }
		public int note { get { return stringOpenNote + fretNum; } }

		public FretObject(FretUI prefab, Transform container, Fretboard tab, int stringIndex, int fret)
		{
			var mono = GameObject.Instantiate(prefab, container);
			mono.gameObject.SetActive(true);
			mono.fret = (Fret)this;

			this.mono = mono;
			this.rect = mono.GetComponent<RectTransform>();
			this.stringNum = stringIndex;
			this.fretNum = fret;
			this.fretboardUI = tab;

			this.mono.borderDot1.gameObject.SetActive(false);
			this.mono.borderDot2.gameObject.SetActive(false);
		}

		public override void UpdateDisplay()
		{
			var setting = fretboardUI.displaySetting;
			var isHidden = displayMode == FretToggleMode.Hidden;
			var isEmphasized = displayMode == FretToggleMode.Emphasized;

			var isNote = setting.fretMode == Fret.PlayableMode.Note;
			var isInterval = setting.fretMode == Fret.PlayableMode.Interval;
			var isBoth = setting.fretMode == Fret.PlayableMode.NoteAndInterval;
			var isDots = setting.fretMode == Fret.PlayableMode.Dot;

			mono.borderDot1.gameObject.SetActive(!isHidden && isDots);
			mono.tmp.gameObject.SetActive(!isHidden && !isDots);

			if (!isHidden && !isDots)
			{
				string s = "";

				if (isNote || isBoth)
					s = MusicScale.NoteValueToName(note, fretboardUI.scale.preferFlats, false);

				if (isBoth)
					s += "";

				if (isInterval || isBoth)
					s += MusicScale.NoteToIntervalString(fretboardUI.scale.notes, note, fretboardUI.scale.preferFlats);

				mono.tmp.text = s;
				if (isEmphasized)
					mono.tmp.fontStyle = FontStyles.Bold;
				else
					mono.tmp.fontStyle = FontStyles.Normal;
			}

		}
	}
}