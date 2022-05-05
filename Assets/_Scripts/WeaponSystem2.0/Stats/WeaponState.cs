using System.Collections;
using UnityEngine;

namespace WeaponSystem
{
	public class WeaponState : MonoBehaviour
	{
		public int heat;
		[SerializeField] private float decreaseTime;
		private bool isDecreasing = false;

		public int currentAmmo;
		public int ammoReserve;
		public bool isReloading;
		public bool isAiming;

		public void Init(WeaponStats stats)
		{
			currentAmmo = stats.maxAmmo;
			// FIXME: Derive ammo reserve from somewhere else
			ammoReserve = stats.maxAmmo * 5;
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
}