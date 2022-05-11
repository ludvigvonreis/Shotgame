using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

namespace WeaponSystem.UI
{
	public class HoverPanel : MonoBehaviour
	{
		[SerializeField] private TMP_Text equipButton;
		[SerializeField] private InputActionReference interactButton;

		[SerializeField] private TMP_Text weaponName;
		[SerializeField] private TMP_Text flavorText;

		public void Setup(Weapon weapon)
		{
			weaponName.text = weapon.weaponStats.name;
			flavorText.text = "Test flavor text";
			equipButton.text = string.Format("[{0}] EQUIP", interactButton.action.GetBindingDisplayString());
		}
	}
}