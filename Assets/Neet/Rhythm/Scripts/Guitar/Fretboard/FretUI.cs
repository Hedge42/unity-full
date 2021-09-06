using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

// a monobehavior layer for fret/border objects
// serves as a template + container for fret and border objects
namespace Neat.Music
{
	public class FretUI : MonoBehaviour, IPointerClickHandler
	{
		public Fret fret;

		public TextMeshProUGUI tmp;

		public Image fill;
		public Image borderDot1;
		public Image borderDot2;

		public event UnityAction<PointerEventData> onClick;

		public Fret.FretToggleMode displayMode
		{
			get { return fret.displayMode; }
			set { fret.displayMode = value; }
		}
		public FretboardUI fretboard
		{
			get { return fret.fretboardUI; }
			set { fret.fretboardUI = value; }
		}
		public int fretNum
		{
			get { return fret.fretNum; }
			set { fret.fretNum = value; }
		}

		public void UpdateText()
		{
			fret.UpdateDisplay();
		}
		public void Hide()
		{
			fret.Hide();
		}
		public void ToggleMode()
		{
			fret.ToggleMode();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			onClick?.Invoke(eventData);

			GetComponent<FretAnimator>().Animate();

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