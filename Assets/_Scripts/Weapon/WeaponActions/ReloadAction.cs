using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponActions
{
	class ReloadAction : AWeaponAction
	{
		private Player player;
		private WeaponLogic weaponLogic;

		// Weapon stats
		private WeaponStats weaponStats;
		private WeaponState weaponState;
		private WeaponVFX weaponVFX;

		public override void Init(WeaponObject weaponObject, WeaponLogic _logic)
		{
			weaponLogic = _logic;
			player = _logic.player;

			weaponVFX = weaponObject.vfx;
			weaponState = weaponObject.state;
			weaponStats = weaponObject.stats;
		}

		// TODO: Tactical and empty reloads??
		public override void Run(InputAction action)
		{
			if (!action.WasPerformedThisFrame()) return;

			// To stop trying to reload twice
			if (weaponState.isReloading) return;

			// Magazine full, no reload
			if (weaponState.currentAmmo >= weaponStats.maxAmmo) return;

			// No ammo reserve to reload from
			if (weaponState.ammoReserve <= 0) return;

			StartCoroutine(ReloadRoutine());
		}

		IEnumerator ReloadRoutine()
		{
			weaponState.isReloading = true;

			var reloadTime = weaponStats.reloadTime;
			var currentAmmo = weaponState.currentAmmo;
			var reserve = weaponState.ammoReserve;
			var maxAmmo = weaponStats.maxAmmo;

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
			weaponState.currentAmmo = 0;

			// TODO: Play a reload animation for the duration of reload time

			yield return new WaitForSeconds(reloadTime);

			weaponState.currentAmmo += difference;
			weaponState.ammoReserve -= difference;

			weaponState.isReloading = false;
		}
	}
}