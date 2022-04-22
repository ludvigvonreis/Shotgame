using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponState))]
[RequireComponent(typeof(WeaponLogic))]
public class WeaponObject : MonoBehaviour, IPickup
{
	[ReadOnly]
	public string ID;

	public WeaponState state;
	public WeaponStats stats;

	public GameObject worldModel;

	void Awake()
	{
		ID = System.Guid.NewGuid().ToString();
	}

	void Start()
	{
		state.heat = 0;
		state.currentAmmo = stats.maxAmmo;
		state.ammoReserve = stats.maxAmmo * 5;
		state.isReloading = false;
	}

	public void SpawnModel(WeaponObject wepObj)
	{
		var model = Instantiate(wepObj.stats.weaponModel, Vector3.zero, Quaternion.identity);
		model.name = wepObj.stats.ID;
		worldModel = model;
		model.transform.parent = this.transform;
	}

	public void Destroy()
	{
		Object.Destroy(this.gameObject);
	}

	public bool CanPickup()
	{
		return true;
	}

	public void Pickup()
	{
		Debug.Log("Im getting picked up.");
	}

	public void PostPickup()
	{
	}
}
