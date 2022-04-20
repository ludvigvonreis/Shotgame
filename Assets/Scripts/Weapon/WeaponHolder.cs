using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> heldWeapons;

	public WeaponObject CreateWeaponObject(WeaponObject wepObj)
	{
		var obj = new GameObject(wepObj.stats.ID);
		var state = obj.AddComponent<WeaponState>();
		var _wepObj = obj.AddComponent<WeaponObject>();

		// HACK: to make sure i dont try to change values on another state
		state.Clone(wepObj.state);
		_wepObj.state = state;
		_wepObj.stats = wepObj.stats;

		var model = Instantiate(wepObj.stats.weaponModel, Vector3.zero, Quaternion.identity);
		model.name = wepObj.stats.ID;
		_wepObj.worldModel = model;
		model.transform.parent = obj.transform;

		obj.transform.parent = this.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;

		heldWeapons.Add(obj);

		//wepObj.Destroy(); is called in post pickup instead

		return _wepObj;
	}

	public void RemoveWeaponObject(string ID)
	{
		var obj = heldWeapons.Single(w => w.GetComponent<WeaponObject>().ID == ID);
		heldWeapons.Remove(obj);
		Destroy(obj);
	}
}
