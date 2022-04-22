using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	private Dictionary<string, WeaponObject> equippedWeapons = new Dictionary<string, WeaponObject>();
	private string currentWeaponID;

	[SerializeField]
	private int maxWeapons = 1;

	[SerializeField]
	private WeaponHolder weaponHolder;


	public bool CanEquip => equippedWeapons.Count < maxWeapons;

	#region Dictionary abstraction

	public void AddWeapon(WeaponObject wepObj)
	{
		weaponHolder.AddWeapon(wepObj);
		equippedWeapons.Add(wepObj.ID, wepObj);
	}

	public void RemoveWeapon(WeaponObject wepObj) => equippedWeapons.Remove(wepObj.ID);
	public void RemoveWeapon(string wepId) => equippedWeapons.Remove(wepId);

	public WeaponObject GetWeapon(string wepId) => equippedWeapons[wepId];

	#endregion
}
