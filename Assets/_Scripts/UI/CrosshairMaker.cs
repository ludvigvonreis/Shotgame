using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gnome.UI
{
	public class CrosshairMaker : MonoBehaviour
	{
		[Range(0.0f, 255.0f)]
		public float alpha;
		[Range(0.0f, 100.0f)]
		public float thickness;
		[Range(0.0f, 100.0f)]
		public float size;
		[Range(-100, 100)]
		public int gap;
		[Range(0, 1)]
		public int outline;
		[ColorUsage(false)] public Color color;
		public bool centerDot;
		public bool tCross;

		[Header("Components")]
		public GameObject leftPart;
		public GameObject rightPart;
		public GameObject topPart;
		public GameObject bottomPart;
		public GameObject dot;

		private List<GameObject> parts = new List<GameObject>();

		private List<RectTransform> trans = new List<RectTransform>();
		private List<Image> imgs = new List<Image>();
		private List<Outline> outs = new List<Outline>();

		private float hiddenGap;

		void OnValidate()
		{
			parts.Clear();
			trans.Clear();
			imgs.Clear();
			outs.Clear();
			parts.Add(leftPart);
			parts.Add(rightPart);
			parts.Add(topPart);
			parts.Add(bottomPart);

			foreach (var item in parts)
			{
				var tran = item.GetComponent<RectTransform>();
				var img = item.GetComponent<Image>();
				var ou = item.GetComponent<Outline>();
				trans.Add(tran);
				imgs.Add(img);
				outs.Add(ou);
			}
			hiddenGap = thickness + 1 + gap;

			color.a = alpha;

			// left
			trans[0].sizeDelta = new Vector2(size, thickness);
			trans[0].anchoredPosition = new Vector2(-hiddenGap, 0);
			imgs[0].color = color;
			outs[0].effectDistance = new Vector2(outline, outline);

			// right
			trans[1].sizeDelta = new Vector2(size, thickness);
			trans[1].anchoredPosition = new Vector2(hiddenGap, 0);
			imgs[1].color = color;
			outs[1].effectDistance = new Vector2(outline, outline);

			topPart.SetActive(!tCross);
			if (!tCross)
			{
				// top
				trans[2].sizeDelta = new Vector2(thickness, size);
				trans[2].anchoredPosition = new Vector2(0, hiddenGap);
				imgs[2].color = color;
				outs[2].effectDistance = new Vector2(outline, outline);
			}


			// bottom
			trans[3].sizeDelta = new Vector2(thickness, size);
			trans[3].anchoredPosition = new Vector2(0, -hiddenGap);
			imgs[3].color = color;
			outs[3].effectDistance = new Vector2(outline, outline);

			dot.SetActive(centerDot);
			if (centerDot)
			{
				var dt = dot.GetComponent<RectTransform>();
				var di = dot.GetComponent<Image>();

				dt.sizeDelta = new Vector2(thickness, thickness);
				di.color = color;
			}
		}
	}
}