using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField] private List<GameObject> heldWeapons;

	[SerializeField] private Vector3 holderAlignment;

	private Player player;
	private Camera playerCamera;

	[SerializeField, Range(0.001f, 3)] private float lerpTime = 1;

	void Start()
	{
		player = transform.root.GetComponent<Player>();
		playerCamera = player.playerCam;
	}

	void Update()
	{
		this.transform.localPosition = holderAlignment;
	}

	public void AddWeapon(WeaponObject weaponObject)
	{
		GameObject obj = weaponObject.gameObject;
		obj.transform.parent = this.transform;
		obj.GetComponent<BoxCollider>().enabled = false;

		StartCoroutine(WeaponLerp(obj));

		heldWeapons.Add(obj);
	}

	// TODO: Make this dropping instead of destruction
	public void RemoveWeaponObject(string ID)
	{
		var obj = heldWeapons.Single(w => w.GetComponent<WeaponObject>().ID == ID);
		heldWeapons.Remove(obj);
		obj.GetComponent<BoxCollider>().enabled = true;
		Destroy(obj);
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
}
