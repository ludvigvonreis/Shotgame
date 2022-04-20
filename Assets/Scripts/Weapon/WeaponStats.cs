using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 2)]
public class WeaponStats : ScriptableObject
{
	[ReadOnly]
	public string ID;
	public new string name;

	public float damage;
	public float range;
	public float fireRate;

	public int maxAmmo;

	public GameObject weaponModel;

	public AnimationCurve recoil;

	void OnValidate()
	{
		if (!System.Guid.TryParse(ID, out _))
			ID = System.Guid.NewGuid().ToString();
	}
}
