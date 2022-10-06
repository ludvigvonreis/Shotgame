using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace WeaponSystem
{
	public class WeaponState : MonoBehaviour
	{
		private Weapon weaponReference;

		[Header("Ammo")]
		private int currentAmmo;
		private int currentAmmoReserve;

		public int CurrentAmmo
		{
			get { return currentAmmo; }
			set
			{
				currentAmmo = value;
				ExecuteEvents.
				ExecuteHierarchy<IWeaponStateEvents>(gameObject, null, (x, y) => x.OnAmmoChange(value));
			}
		}
		public int CurrentAmmoReserve
		{
			get { return currentAmmoReserve; }
			set
			{
				currentAmmoReserve = value;
				ExecuteEvents.
				ExecuteHierarchy<IWeaponStateEvents>(gameObject, null, (x, y) => x.OnAmmoReserveChange(value));
			}
		}

		[Header("Heat")]
		public int heat;

		[SerializeField] private float decreaseTime;
		[SerializeField] private bool isDecreasing = false;

		[Header("State")]
		public bool isReloading;
		public bool isAiming;

		public void Init(WeaponStats stats, Weapon reference)
		{
			weaponReference = reference;

			// FIXME: Derive ammo reserve from somewhere else
			currentAmmoReserve = stats.magazineSize * 5;
			currentAmmo = stats.magazineSize;
		}

		public void IncreaseHeat()
		{
			heat += 1;

			ExecuteEvents.
			ExecuteHierarchy<IWeaponStateEvents>(gameObject, null, (x, y) => x.OnHeatIncrease());
		}

		public void DecreaseHeat()
		{
			isDecreasing = true;

			ExecuteEvents.
			ExecuteHierarchy<IWeaponStateEvents>(gameObject, null, (x, y) => x.OnHeatDecrease());

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