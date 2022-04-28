using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

interface IWeapon
{
	public void PrimaryAction(bool down);
	public void SecondaryAction(bool down);
	public void ReloadAction(bool down);
}

public class WeaponLogic : MonoBehaviour, IWeapon
{
	[SerializeField] private Transform shootPoint;

	// This should be set when you pickup this gun
	private Camera shootCamera;
	private Player player;

	private WeaponObject weaponObject;
	private WeaponStats stats;
	private WeaponState state;
	private WeaponVFX weaponVFX;

	[SerializeField]
	private bool isHolding = false;
	[SerializeField]
	private bool hasLetGo = false;
	private bool hasShot = false;
	private bool canShoot => (shootCamera != null && !state.isReloading);

	[SerializeField] private float shootTimeout = .6f;
	private bool canTimeout => (!isHolding && hasLetGo && hasShot);
	private bool hasTimedout = false;

	void Init()
	{
		weaponObject = GetComponent<WeaponObject>();
		weaponVFX = GetComponent<WeaponVFX>();

		state = weaponObject.state;
		stats = weaponObject.stats;

		StartCoroutine(PrimaryLoop());
		StartCoroutine(StoppedShootingTimeout());

		player.m_InputEvent.AddListener(InputListener);
	}

	private void InputListener(string button, bool down)
	{
		if (button == "Fire1") PrimaryAction(down);

		if (button == "Fire2") SecondaryAction(down);

		if (button == "Reload") ReloadAction(down);
	}

	public void SetPlayer(Player _player)
	{
		player = _player;
		shootCamera = _player.playerCam;

		Init();
	}

	public void UnsetPlayer()
	{
		player = null;
		shootCamera = null;
	}

	void UpdateCurrentAmmo(int value = 0)
	{
		state.currentAmmo += value;
	}

	#region Primary

	// when holding primary button "isHolding" = true
	// when let go primary button "isHolding" = false
	public void PrimaryAction(bool startPress)
	{
		//Debug.Log("Primary action");

		isHolding = startPress;
		if (startPress == false)
		{
			hasLetGo = true;
		}
		else
		{
			hasTimedout = false;
		}
	}

	IEnumerator PrimaryLoop()
	{
		var fireRate = 1 / (stats.fireRate / 60);
		// To be able to stop shooting while out of ammo
		while (true)
		{
			var currentAmmo = state.currentAmmo;
			var fireMode = stats.fireMode;

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
					if (isHolding)
					{
						hasLetGo = false;
						RapidFire();
						yield return new WaitForSeconds(fireRate);
					}
					break;

				case FireMode.Single:
					if (isHolding && hasLetGo)
					{
						hasLetGo = false;
						SingleFire();
						yield return new WaitForSeconds(fireRate);
					}
					break;

				case FireMode.Burst:
					if (isHolding && hasLetGo)
					{
						hasLetGo = false;

						BurstFire();
						yield return new WaitForSeconds(fireRate);
					}
					break;
			}

			// Super important
			yield return null;
		}
	}

	void BaseShoot()
	{
		UpdateCurrentAmmo(-1);

		hasShot = true;

		state.IncreaseHeat();

		// Shoot from camera
		RaycastHit hit;
		if (Physics.Raycast(shootCamera.transform.position, shootCamera.transform.forward, out hit, stats.range))
		{
			// Check if target hit is a "killable" object or something else

			// FIXME: Currently weapon logic sits on weapon object so "this.gameobject" is not the player but
			// the gun object. is this something important?
			EventManager.Instance.m_HitEvent.Invoke(new Hit(this.transform.root.gameObject, hit, stats));
			DecalManager.Instance.PlaceDecal(hit.point, Quaternion.identity);
		}

		// Step 2 run visual stuff. Animations, particles.
		weaponVFX.PlayMuzzleflash();

		// Step 3 apply recoil to player
		player.m_ShootEvent.Invoke();
	}

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
					state.CancelDecrease();
				}
			}

			if (hasTimedout == false)
			{
				state.DecreaseHeat();

				player.m_ResetRecoil.Invoke();
				hasTimedout = true;
			}


			yield return null;
		}
	}

	#endregion

	public void SecondaryAction(bool down) { }

	// TODO: Tactical and empty reloads??
	public void ReloadAction(bool down)
	{
		if (!down) return;

		// To stop trying to reload twice
		if (state.isReloading) return;

		// Magazine full, no reload
		if (state.currentAmmo >= stats.maxAmmo) return;

		// No ammo reserve to reload from
		if (state.ammoReserve <= 0) return;

		StartCoroutine(ReloadRoutine());
	}

	IEnumerator ReloadRoutine()
	{
		state.isReloading = true;

		var reloadTime = stats.reloadTime;
		var currentAmmo = state.currentAmmo;
		var reserve = state.ammoReserve;
		var maxAmmo = stats.maxAmmo;

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
		state.currentAmmo = 0;

		// TODO: Play a reload animation for the duration of reload time

		yield return new WaitForSeconds(reloadTime);

		state.currentAmmo += difference;
		state.ammoReserve -= difference;

		state.isReloading = false;
	}
}
