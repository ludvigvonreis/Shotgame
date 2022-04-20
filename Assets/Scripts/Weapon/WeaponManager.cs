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

	void Start()
	{

	}

	public bool CanEquip()
	{
		return equippedWeapons.Count < maxWeapons;
	}

	#region Dictionary abstraction

	public void AddWeapon(WeaponObject wepObj)
	{
		var newWep = weaponHolder.CreateWeaponObject(wepObj);
		equippedWeapons.Add(newWep.ID, newWep);
	}

	public void RemoveWeapon(WeaponObject wepObj)
	{
		equippedWeapons.Remove(wepObj.ID);
	}

	public void RemoveWeapon(string wepId)
	{
		equippedWeapons.Remove(wepId);
	}

	public WeaponObject GetWeapon(string wepId)
	{
		return equippedWeapons[wepId];
	}

	#endregion
}
