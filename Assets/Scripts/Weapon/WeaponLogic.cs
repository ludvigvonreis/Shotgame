using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IWeapon
{
	public void PrimaryAction(bool letGo);
	public void SecondaryAction(bool letGo);
	public void ReloadAction();
}

public class WeaponLogic : MonoBehaviour, IWeapon
{
	[SerializeField]
	private Transform shootPoint;

	private WeaponObject weaponObject;
	private WeaponStats stats;
	private WeaponState state;

	[SerializeField]
	private bool isHolding = false;
	private bool hasLetGo = false;

	void Start()
	{
		weaponObject = GetComponent<WeaponObject>();

		state = weaponObject.state;
		stats = weaponObject.stats;

		StartCoroutine(PrimaryLoop());
	}

	// FIXME: this is super temporary. Put this in a player controller
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			PrimaryAction(true);
		}

		if (Input.GetButtonUp("Fire1"))
		{
			PrimaryAction(false);
		}
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
		if (startPress == false) hasLetGo = true;
	}

	IEnumerator PrimaryLoop()
	{
		var currentAmmo = state.currentAmmo;
		var fireMode = stats.fireMode;

		// To be able to stop shooting while out of ammo
		while (true)
		{
			if (currentAmmo <= 0)
			{
				Debug.Log("Out of ammo");
				yield return null;
			}

			switch (fireMode)
			{
				case FireMode.Rapid:
					if (isHolding)
					{
						RapidFire();
						yield return new WaitForSeconds(0.1f);
					}
					break;

				case FireMode.Single:
					if (isHolding && hasLetGo)
					{
						hasLetGo = false;
						SingleFire();
						yield return new WaitForSeconds(0.1f);
					}
					break;

				case FireMode.Burst:
					if (isHolding && hasLetGo)
					{
						hasLetGo = false;

						BurstFire();
						yield return new WaitForSeconds(0.1f);
					}
					break;
			}

			// Super important
			yield return null;
		}
	}

	void SingleFire()
	{
		UpdateCurrentAmmo(-1);

		RaycastHit hit;
		if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, stats.range))
		{
			Debug.LogFormat("I just hit {0}", hit.transform.name);
			// Check if target hit is a "killable" object or something else
			// if something else create a decal at point. Using a decal manager singleton
		}

		// Step 2 run visual stuff. Animations, particles.

		// Step 3 apply recoil to player
	}

	void RapidFire()
	{
		UpdateCurrentAmmo(-1);
	}

	void BurstFire()
	{
		UpdateCurrentAmmo(-1);
	}

	#endregion

	public void SecondaryAction(bool letGo) { }

	public void ReloadAction() { }
}
