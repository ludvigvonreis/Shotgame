using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField] private List<GameObject> heldWeapons;

	[SerializeField] private Vector3 holderAlignment;
	[SerializeField] private Camera playerCamera;

	[SerializeField, Range(0.001f, 3)] private float lerpTime = 1;

	void Update()
	{
		this.transform.localPosition = holderAlignment;
	}

	public void AddWeapon(WeaponObject weaponObject)
	{
		GameObject obj = weaponObject.gameObject;
		// TODO: Lerp animation from original position to new position
		obj.transform.parent = this.transform;
		StartCoroutine(WeaponLerp(obj));


		heldWeapons.Add(obj);
	}

	IEnumerator WeaponLerp(GameObject weaponObject)
	{
		Vector3 currentPos = weaponObject.transform.localPosition;
		Quaternion currentRot = weaponObject.transform.localRotation;

		float elapsedTime = 0;
		while (elapsedTime < lerpTime)
		{
			weaponObject.transform.localPosition = Vector3.Slerp(currentPos, Vector3.zero, (elapsedTime / lerpTime));
			weaponObject.transform.localRotation = Quaternion.Slerp(currentRot, Quaternion.identity, (elapsedTime / lerpTime));
			elapsedTime += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		yield return null;
	}

	/*
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
	*/

	public void RemoveWeaponObject(string ID)
	{
		var obj = heldWeapons.Single(w => w.GetComponent<WeaponObject>().ID == ID);
		heldWeapons.Remove(obj);
		Destroy(obj);
	}
}
