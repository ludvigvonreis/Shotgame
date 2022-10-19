using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
	public interface IStateUpdate
	{
		public UnityEvent m_stateChange { get; }
	}

	public class WeaponState : MonoBehaviour
	{
		private Weapon weaponReference;

		private IStateUpdate stateUpdate;

		[Header("Ammo")]
		private bool noting;

		public int currentAmmo
		{
			get
			{
				return _currentAmmo;
			}
			set
			{
				_currentAmmo = value;
				stateUpdate.m_stateChange.Invoke();
			}
		}
		private int _currentAmmo;

		public int ammoReserve
		{
			get
			{
				return _ammoReserve;
			}
			set
			{
				_ammoReserve = value;
				stateUpdate?.m_stateChange.Invoke();
			}
		}
		private int _ammoReserve;

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
			weaponReference.owner.ownerObject.TryGetComponent<IStateUpdate>(out stateUpdate);

			// FIXME: Derive ammo reserve from somewhere else
			_ammoReserve = stats.maxAmmo * 5;
			_currentAmmo = stats.maxAmmo;

			stateUpdate?.m_stateChange.Invoke();
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