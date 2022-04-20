using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponState))]
public class WeaponObject : MonoBehaviour
{
	public string ID;

	public WeaponState state;
	public WeaponStats stats;

	public GameObject worldModel;

	void Awake()
	{
		ID = System.Guid.NewGuid().ToString();
	}
}
