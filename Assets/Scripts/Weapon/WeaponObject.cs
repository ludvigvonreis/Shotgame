using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponState))]
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
		Debug.Log(string.Format("Im getting picked up by {0}", name));
	}

	public void PostPickup()
	{
		Debug.Log("Goodbye cruel world!");
		Destroy();
	}
}
