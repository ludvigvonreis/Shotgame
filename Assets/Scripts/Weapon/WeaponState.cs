using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
	public int heat;

	public int currentAmmo;
	public int ammoReserve;
	public bool isReloading;

	public void Clone(WeaponState _old)
	{
		heat = _old.heat;
		currentAmmo = _old.currentAmmo;
		ammoReserve = _old.ammoReserve;
		isReloading = _old.isReloading;
	}
}
