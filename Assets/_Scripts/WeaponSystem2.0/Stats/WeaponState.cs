using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
	[System.Serializable]
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

	public class WeaponState : MonoBehaviour
	{
		private IUpdator updator;
		public interface IUpdator
		{
			public UnityEvent<AmmoChangeEvent> m_ammoChange { get; }
			//public UnityEvent<HeatChangeEvent> m_heatChange { get; }
		}

		public int heat;
		[SerializeField] private float decreaseTime;
		private bool isDecreasing = false;

		public int currentAmmo
		{
			get
			{
				return _currentAmmo;
			}
			set
			{
				_currentAmmo = value;
				updator.m_ammoChange.Invoke(new AmmoChangeEvent(currentAmmo, ammoReserve));
			}
		}
		private int _currentAmmo;

		public int ammoReserve;
		public bool isReloading;
		public bool isAiming;

		public void Init(WeaponStats stats)
		{
			updator = transform.root.GetComponents<IUpdator>()[0];

			// FIXME: Derive ammo reserve from somewhere else
			ammoReserve = stats.maxAmmo * 5;
			_currentAmmo = stats.maxAmmo;

			updator.m_ammoChange.Invoke(new AmmoChangeEvent(currentAmmo, ammoReserve));
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