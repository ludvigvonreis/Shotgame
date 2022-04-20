using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 2)]
public class WeaponStats : ScriptableObject
{
	public new string name;
	public string ID;

	public float damage;
	public float range;
	public float fireRate;

	public int maxAmmo;

	public GameObject weaponModel;

	public AnimationCurve recoil;
}
