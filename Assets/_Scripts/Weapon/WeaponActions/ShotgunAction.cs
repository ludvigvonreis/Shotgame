using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponActions
{
	class ShotgunAction : AWeaponAction
	{
		private Player player;
		private WeaponObject weaponObject;

		// Weapon stats
		private WeaponStats weaponStats;
		private WeaponState weaponState;
		private WeaponVFX weaponVFX;

		[SerializeField]
		private Camera shootCamera;
		private InputAction action;

		// Shooting state
		private bool canShoot => (shootCamera != null && !weaponState.isReloading);

		private bool isHoldingFire = false;
		private bool hasReleasedFire = false;
		private bool hasFired = false;

		// Timeout
		[SerializeField] private float shootTimeout = .6f;
		private bool canTimeout => (!isHoldingFire && hasReleasedFire && hasFired);
		private bool isTimeoutDone = false;

		public override void Init(Player _player)
		{
			player = _player;
			shootCamera = player.playerCam;

			action = player.playerInput.actions[player.shootButton.action.name];

			StartCoroutine(PrimaryLoop());
			StartCoroutine(StoppedShootingTimeout());
		}

		public override void Terminate()
		{
			player = null;
			shootCamera = null;
			action = null;

			StopAllCoroutines();
		}

		void Start()
		{
			weaponObject = GetComponent<WeaponObject>();

			weaponVFX = weaponObject.vfx;
			weaponState = weaponObject.state;
			weaponStats = weaponObject.stats;
		}

		void Update()
		{
			if (!weaponObject.isHeld) return;

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
			// FIXME: Temporary implementation

			if (!(weaponStats is ShotgunStats)) return;

			weaponState.currentAmmo -= 1;
			hasFired = true;
			weaponState.IncreaseHeat();

			// Shoot from camera
			for (int i = 0; i < ((ShotgunStats)weaponStats).totalPellets; i++)
			{
				Vector3 deviation3D = Random.insideUnitCircle * ((ShotgunStats)weaponStats).maxDevitation;
				Quaternion rot = Quaternion.LookRotation(Vector3.forward * weaponStats.range + deviation3D);
				Vector3 forwardVector = shootCamera.transform.rotation * rot * Vector3.forward;

				RaycastHit hit;
				if (Physics.Raycast(
					shootCamera.transform.position, forwardVector, out hit, weaponStats.range
				))
				{
					EventManager.Instance.m_HitEvent.Invoke(new Hit(player.gameObject, hit, weaponStats));
					DecalManager.Instance.PlaceDecal(hit.point, Quaternion.identity);
				}
			}

			// Step 2 run visual stuff. Animations, particles.
			weaponVFX.PlayMuzzleflash();

			// Step 3 apply recoil to player
			player.m_ShootEvent.Invoke();
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
							BaseShoot();
							yield return new WaitForSeconds(fireRate);
						}
						break;

					case FireMode.Single:
						if (isHoldingFire && hasReleasedFire)
						{
							hasReleasedFire = false;
							BaseShoot();
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