using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace WeaponActions
{
	class ShootAction : AWeaponAction
	{
		private Player player;
		private WeaponLogic weaponLogic;

		// Weapon stats
		private WeaponStats weaponStats;
		private WeaponState weaponState;
		private WeaponVFX weaponVFX;

		private Camera shootCamera;

		// Shooting state
		private bool canShoot => (shootCamera != null && !weaponState.isReloading);

		private bool isHoldingFire = false;
		private bool hasReleasedFire = false;
		private bool hasFired = false;

		// Timeout
		[SerializeField] private float shootTimeout = .6f;
		private bool canTimeout => (!isHoldingFire && hasReleasedFire && hasFired);
		private bool isTimeoutDone = false;

		public override void Init(WeaponObject weaponObject, WeaponLogic _logic)
		{
			weaponLogic = _logic;
			player = _logic.player;
			shootCamera = _logic.shootCamera;

			weaponVFX = weaponObject.vfx;
			weaponState = weaponObject.state;
			weaponStats = weaponObject.stats;

			StartCoroutine(PrimaryLoop());
			StartCoroutine(StoppedShootingTimeout());
		}

		public override void Run(InputAction action)
		{
			if (action.WasPerformedThisFrame())
			{
				isTimeoutDone = false;
				isHoldingFire = true;
			}

			if (action.WasReleasedThisFrame())
			{
				isHoldingFire = false;
				hasReleasedFire = true;
			}
		}


		void BaseShoot()
		{
			weaponState.currentAmmo -= 1;

			hasFired = true;

			weaponState.IncreaseHeat();

			// Shoot from camera
			RaycastHit hit;
			if (Physics.Raycast(shootCamera.transform.position, shootCamera.transform.forward, out hit, weaponStats.range))
			{
				// Check if target hit is a "killable" object or something else

				// FIXME: Currently weapon logic sits on weapon object so "this.gameobject" is not the player but
				// the gun object. is this something important?
				EventManager.Instance.m_HitEvent.Invoke(new Hit(weaponLogic.transform.root.gameObject, hit, weaponStats));
				DecalManager.Instance.PlaceDecal(hit.point, Quaternion.identity);
			}

			// Step 2 run visual stuff. Animations, particles.
			weaponVFX.PlayMuzzleflash();

			// Step 3 apply recoil to player
			player.m_ShootEvent.Invoke();
		}

		// Are these really needed?

		void SingleFire()
		{
			BaseShoot();
		}

		void RapidFire()
		{
			BaseShoot();
		}

		void BurstFire()
		{
			BaseShoot();
		}

		IEnumerator PrimaryLoop()
		{
			var fireRate = 1 / (weaponStats.fireRate / 60);
			// To be able to stop shooting while out of ammo
			while (true)
			{
				var currentAmmo = weaponState.currentAmmo;
				var fireMode = weaponStats.fireMode;

				if (currentAmmo <= 0)
				{
					//Debug.Log("Out of ammo");
					yield return null;
					continue;
				}

				if (!canShoot)
				{
					yield return null;
					continue;
				};

				switch (fireMode)
				{
					case FireMode.Rapid:
						if (isHoldingFire)
						{
							hasReleasedFire = false;
							RapidFire();
							yield return new WaitForSeconds(fireRate);
						}
						break;

					case FireMode.Single:
						if (isHoldingFire && hasReleasedFire)
						{
							hasReleasedFire = false;
							SingleFire();
							yield return new WaitForSeconds(fireRate);
						}
						break;

					case FireMode.Burst:
						if (isHoldingFire && hasReleasedFire)
						{
							hasReleasedFire = false;

							BurstFire();
							yield return new WaitForSeconds(fireRate);
						}
						break;
				}

				// Super important
				yield return null;
			}
		}

		IEnumerator StoppedShootingTimeout()
		{
			while (true)
			{
				bool stillWaiting = true;
				while (stillWaiting)
				{
					yield return new WaitUntil(() => canTimeout);

					float pauseTime = shootTimeout;
					yield return new WaitUntil(() =>
					{
						pauseTime -= Time.deltaTime;
						stillWaiting = !canTimeout;
						return stillWaiting || pauseTime <= 0;
					});

					if (stillWaiting)
					{
						// Stuff when pause is interrupted goes here
						weaponState.CancelDecrease();
					}
				}

				if (isTimeoutDone == false)
				{
					Debug.Log("timed out");

					weaponState.DecreaseHeat();

					player.m_ResetRecoil.Invoke();
					isTimeoutDone = true;
				}


				yield return null;
			}
		}

	}
}