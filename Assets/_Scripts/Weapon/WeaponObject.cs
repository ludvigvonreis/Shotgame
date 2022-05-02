using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponState))]
[RequireComponent(typeof(WeaponVFX))]
public class WeaponObject : MonoBehaviour, IInteractible
{
	[ReadOnly]
	public string ID;

	public bool isHeld;

	public WeaponState state;
	public BaseStats stats;
	public WeaponVFX vfx;

	public GameObject worldModel;

	void Awake()
	{
		ID = System.Guid.NewGuid().ToString();
	}

	void Start()
	{
		state = GetComponent<WeaponState>();

		state.heat = 0;
		if (stats is WeaponStats)
		{
			state.currentAmmo = ((WeaponStats)stats).maxAmmo;
			state.ammoReserve = ((WeaponStats)stats).maxAmmo * 5;
		}
		state.isReloading = false;
	}

	public void SpawnModel(WeaponObject wepObj)
	{
		var model = Instantiate(wepObj.stats.model, Vector3.zero, Quaternion.identity);
		model.name = wepObj.stats.ID;
		worldModel = model;
		model.transform.parent = this.transform;
	}

	public void Destroy()
	{
		Object.Destroy(this.gameObject);
	}

	public void Interact() { }

	public void PostInteract() { }
}
