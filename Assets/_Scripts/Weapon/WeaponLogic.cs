using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AWeaponAction : MonoBehaviour
{
	public virtual void Init(WeaponObject weaponObject, WeaponLogic _logic) { }
	public virtual void Run(InputAction action) { }
}

public class WeaponLogic : MonoBehaviour
{
	[SerializeField] private Transform shootPoint;
	[SerializeField] public Camera shootCamera;

	private Player _player;

	[HideInInspector]
	public Player player
	{
		get { return _player; }
		set
		{
			if (value == null)
			{
				_player = null;
				shootCamera = null;
			}

			_player = value;
			shootCamera = value.playerCam;

			Init();
		}
	}

	[SerializeField]
	private AWeaponAction primaryAction;
	[SerializeField]
	private AWeaponAction secondaryAction;
	[SerializeField]
	private AWeaponAction reloadAction;

	private bool isInitiated = false;

	// Input
	private InputAction primaryInput;
	private InputAction secondaryInput;
	private InputAction reloadInput;

	/*
	// Stats
	private WeaponObject weaponObject;
	private WeaponStats weaponStats;
	private WeaponState weaponState;
	private WeaponVFX weaponVFX;

	// Shooting state
	private bool isHoldingFire = false;
	private bool hasReleasedFire = false;
	private bool hasFired = false;
	private bool canShoot => (shootCamera != null && !weaponState.isReloading);

	// Timeout
	[SerializeField] private float shootTimeout = .6f;
	private bool canTimeout => (!isHoldingFire && hasReleasedFire && hasFired);
	private bool timeoutDone = false;
	*/

	void Init()
	{
		var weaponObject = GetComponent<WeaponObject>();

		//weaponVFX = weaponObject.vfx;
		//weaponState = weaponObject.state;
		//weaponStats = weaponObject.stats;

		//StartCoroutine(PrimaryLoop());
		//StartCoroutine(StoppedShootingTimeout());

		primaryInput = player.playerInput.actions[player.shootButton.action.name];
		secondaryInput = player.playerInput.actions[player.shootButton.action.name];
		reloadInput = player.playerInput.actions[player.reloadButton.action.name];

		primaryAction.Init(weaponObject, this);
		secondaryAction.Init(weaponObject, this);
		reloadAction.Init(weaponObject, this);


		isInitiated = true;
	}

	void Update()
	{
		if (!isInitiated) return;

		primaryAction.Run(primaryInput);
		secondaryAction.Run(secondaryInput);
		reloadAction.Run(reloadInput);
	}

	/*
	#region Primary

	public void PrimaryAction()
	{
		if (shootAction.WasPerformedThisFrame())
		{
			timeoutDone = false;
			isHoldingFire = true;
		}

		if (shootAction.WasReleasedThisFrame())
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
			EventManager.Instance.m_HitEvent.Invoke(new Hit(this.transform.root.gameObject, hit, weaponStats));
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

			if (timeoutDone == false)
			{
				weaponState.DecreaseHeat();

				player.m_ResetRecoil.Invoke();
				timeoutDone = true;
			}


			yield return null;
		}
	}

	#endregion

	public void SecondaryAction() { }

	#region Reload

	// TODO: Tactical and empty reloads??
	public void ReloadAction()
	{
		if (!reloadAction.WasPerformedThisFrame()) return;

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

	#endregion
	*/
}
