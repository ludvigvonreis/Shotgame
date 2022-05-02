using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponActions
{
	class ReloadAction : AWeaponAction
	{
		private Player player;
		private WeaponObject weaponObject;
		private InputAction action;

		// Weapon stats
		private WeaponStats weaponStats;
		private WeaponState weaponState;
		private WeaponVFX weaponVFX;

		void Start()
		{
			weaponObject = GetComponent<WeaponObject>();
			weaponVFX = weaponObject.vfx;
			weaponState = weaponObject.state;

			if (weaponObject.stats is WeaponStats)
				weaponStats = (WeaponStats)weaponObject.stats;
			else
				Debug.LogError("Weapon is not shootable");
		}

		public override void Init(Player _player)
		{
			player = _player;
			action = player.playerInput.actions[player.reloadButton.action.name];
		}

		public override void Terminate()
		{
			player = null;
			action = null;
			StopAllCoroutines();
		}

		// TODO: Tactical and empty reloads??
		void Update()
		{
			if (!weaponObject.isHeld) return;

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