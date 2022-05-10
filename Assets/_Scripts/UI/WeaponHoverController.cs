using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IHoverable
{
	void OnHover(bool exit);
}

namespace WeaponSystem
{
	public class WeaponHoverController : MonoBehaviour, IHoverable
	{
		[SerializeField] private GameObject uiHolder;
		[SerializeField] private HoverPanel hoverPanel;

		[SerializeField] private bool showPanel;

		void Start()
		{
			var weapon = GetComponent<Weapon>();
			hoverPanel.Setup(weapon);

			showPanel = false;
			uiHolder.SetActive(showPanel);
		}

		public void OnHover(bool exit)
		{
			showPanel = !exit;
			uiHolder.SetActive(showPanel);
		}
	}
}