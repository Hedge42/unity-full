using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Neet.Guitar
{
	public class MouseClickHandler
	{
		public UnityAction<FretUI> onMouseDown;

		//public static void MouseDown(PointerEventData eventData, FretUI ui)
		//{
  //          if (fret is FretObject)
  //              fretboard.FretObjectClicked(fret.fretNum, fret.rowIndex);

  //          if (eventData.button == PointerEventData.InputButton.Right)
  //          {
  //              fret.Hide();
  //          }
  //          else if (eventData.button == PointerEventData.InputButton.Left)
  //          {
  //              fret.ToggleMode();
  //          }
  //      }
	}
}
