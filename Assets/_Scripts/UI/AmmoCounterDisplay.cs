using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AmmoChangeEvent
{
	public int currentAmmo;
	public int maxAmmo;

	public AmmoChangeEvent(int _a, int _ma)
	{
		currentAmmo = _a;
		maxAmmo = _ma;
	}
}

public class AmmoCounterDisplay : MonoBehaviour
{
	[SerializeField] private WeaponManager weaponManager;
	[SerializeField] private TMP_Text text;

	//private UnityEvent<AmmoChangeEvent> m_ammoChange;

	void Start()
	{
		m_ammoChange = new UnityEvent<AmmoChangeEvent>();
		m_ammoChange.AddListener(HandleAmmoChange);
	}

	public void HandleAmmoChange(AmmoChangeEvent ammoChange)
	{
		UpdateDisplay(ammoChange.currentAmmo, ammoChange.maxAmmo);
	}

	void UpdateDisplay(int currentAmmo, int maxAmmo)
	{
		text.text = string.Format("{0} / {1}", currentAmmo, maxAmmo);
	}
}
