using System.Linq;
using TMPro;
using UnityEngine;

public class AmmoCounterDisplay : MonoBehaviour
{
	[SerializeField] private WeaponManager weaponManager;
	[SerializeField] private TMP_Text text;

	void Start()
	{
		weaponManager._m_ammoChange.AddListener(HandleAmmoChange);
	}

	public void HandleAmmoChange(WeaponSystem.AmmoChangeEvent ammoChange)
	{
		UpdateDisplay(ammoChange.currentAmmo, ammoChange.maxAmmo);
	}

	void UpdateDisplay(int currentAmmo, int maxAmmo)
	{
		text.text = string.Format("{0} / {1}", currentAmmo, maxAmmo);
	}
}
