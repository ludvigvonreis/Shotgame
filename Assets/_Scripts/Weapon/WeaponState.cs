using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
	public int heat;
	[SerializeField] private float decreaseTime;
	private bool isDecreasing = false;


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

	public void IncreaseHeat()
	{
		heat += 1;
	}

	public void DecreaseHeat()
	{
		isDecreasing = true;
		StartCoroutine(HeatDecreaser());
	}

	public void CancelDecrease()
	{
		isDecreasing = false;
	}

	IEnumerator HeatDecreaser()
	{
		int oldHeat = heat;

		float t = decreaseTime;
		float elapsedTime = 0.0f;
		while (t > 0.0f)
		{
			if (!isDecreasing) break;

			t -= Time.deltaTime;
			elapsedTime += Time.deltaTime;

			float progress = Mathf.Pow(elapsedTime / decreaseTime, 3);

			// Cubic ease in decrease
			heat = (int)Mathf.Lerp(oldHeat, 0, progress);

			yield return null;
		}
	}
}
