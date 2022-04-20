using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> equipped_weapons;

	public WeaponObject temp;

	void CreateWeaponObject(WeaponObject wepObj)
	{
		var obj = new GameObject(wepObj.stats.ID);
		obj.AddComponent<WeaponState>();
		var _wepObj = obj.AddComponent<WeaponObject>();
		_wepObj.state = wepObj.state;
		_wepObj.stats = wepObj.stats;

		var model = Instantiate(wepObj.stats.weaponModel, Vector3.zero, Quaternion.identity);
		model.name = wepObj.stats.ID;
		_wepObj.worldModel = model;
		model.transform.parent = obj.transform;

		obj.transform.parent = this.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;

		equipped_weapons.Add(obj);
	}

	void Start()
	{
		CreateWeaponObject(temp);
	}
}
