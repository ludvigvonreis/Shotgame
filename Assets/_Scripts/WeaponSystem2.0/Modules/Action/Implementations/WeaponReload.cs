using System.Collections;
using UnityEngine;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponReload : WeaponAction
	{
		[SerializeField]
		private Transform animationHolder;
		[SerializeField]
		private bool forwardSpin;

		private bool performed;
		private bool isReloading;

		private WeaponState weaponState;
		private WeaponStats weaponStats;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;

			weaponState = groupReference.weaponState;
			weaponStats = groupReference.weaponStats;
		}

		protected override void ProcessInput(object sender, WeaponEvent.ActionContext context)
		{
			if (groupReference.isRunning == false) return;

			performed = context.performed;
		}

		void Action()
		{
			if (performed && !isReloading)
			{
				StartCoroutine(Reload());
			}
		}

		IEnumerator Reload()
		{
			var currentAmmo = weaponState.CurrentAmmo;
			var reserve = weaponState.CurrentAmmoReserve;
			var maxAmmo = weaponStats.magazineSize;

			if (reserve <= 0) yield break;

			weaponState.isReloading = true;
			isReloading = true;

			var difference = maxAmmo - currentAmmo;

			// Set diffrence to reserve if it has less that needed. i.e reload a non full magazine
			// Otherwise add current ammo to it
			if (reserve < difference)
			{
				difference = reserve;
			}
			else
			{
				difference += currentAmmo;
			}

			// Set ammo to zero to simulate removing magazine from weapon
			weaponState.CurrentAmmo = 0;

			yield return StartCoroutine(ReloadAnimation());

			weaponState.CurrentAmmo += difference;
			weaponState.CurrentAmmoReserve -= difference;

			weaponState.isReloading = false;
			isReloading = false;

			// TODO: Invoke reload event or something for ui.
		}

		IEnumerator ReloadAnimation()
		{
			var reloadTime = weaponStats.reloadTime;

			Quaternion _startRotation = animationHolder.localRotation;
			float _time = 0f;
			while (_time < reloadTime)
			{
				_time += Time.deltaTime;
				var spinDelta = -(Mathf.Cos(Mathf.PI * (_time / reloadTime)) - 1f) / 2f;

				if (!forwardSpin) spinDelta = -spinDelta;

				animationHolder.localRotation = Quaternion.Euler(new Vector3(spinDelta * 360f, 0, 0));

				yield return null;
			}
		}
	}
}