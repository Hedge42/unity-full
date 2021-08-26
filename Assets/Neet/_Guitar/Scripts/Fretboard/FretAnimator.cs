using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Guitar
{
	public class FretAnimator : MonoBehaviour
	{
		public AnimationCurve curve;

		public float animTime;

		public Color textColor;
		public Color fillColor;

		[Range(0, 5)]
		public float scale;

		private Color textStartColor;
		private Color fillStartCoor;

		private FretUI ui;

		private void Start()
		{
			ui = GetComponent<FretUI>();
			textStartColor = ui.tmp.color;
		}

		public void Animate()
		{
			StartCoroutine(_Animate());
		}
		private IEnumerator _Animate()
		{
			// immediately set the color to the new color
			// lerp back to the original 

			float startTime = Time.time;
			while (Time.time < startTime + animTime)
			{
				float t = (Time.time - startTime) / animTime;
				t = curve.Evaluate(t);

				// change colors
				ui.tmp.color = Color.Lerp(textColor, textStartColor, t);
				ui.tmp.transform.localScale = Vector3.Lerp(Vector3.one * scale, Vector3.one, t);


				yield return new WaitForEndOfFrame();
			}

			ui.tmp.color = textStartColor;
			ui.tmp.transform.localScale = Vector3.one;
		}
	}
}