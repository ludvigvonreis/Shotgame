using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField] private Transform weaponHolderTransform;
	[SerializeField] private float animTime = 1.5f;

	[Header("Throwing")]
	[SerializeField] private float throwForce;
	[SerializeField] private float throwExtraForce;
	[SerializeField] private float rotationForce;

	private bool isMoving = false;

	// TODO: move this into weapon logic??
	[SerializeField] private LayerMask equippedLayer;
	[SerializeField] private LayerMask droppedLayer;

	private WeaponManager weaponManager;
	private Player player;

	void Start()
	{
		if (!weaponHolderTransform) throw new Exception("Weapon holder transform not set");

		player = GetComponent<Player>();
		weaponManager = player.weaponManager;
		weaponManager.m_onEquip.AddListener(EquipEvent);
	}

	void EquipEvent(WeaponEquipEvent weaponEquipEvent)
	{
		if (!weaponEquipEvent.removed)
		{
			Pickup(weaponEquipEvent.uuid);
		}
		else
		{
			Throw(weaponEquipEvent.uuid);
		}
	}

	void Pickup(string uuid)
	{
		var weapon = weaponManager.GetWeaponByUUID(uuid);
		var weaponObject = weapon.gameObject;
		weaponObject.transform.parent = weaponHolderTransform;
		SetLayerRecursively(weaponObject, (int)Mathf.Log(equippedLayer.value, 2));

		Rigidbody rb;
		if (weaponObject.TryGetComponent<Rigidbody>(out rb))
		{
			Destroy(rb);
		}

		weaponObject.GetComponent<BoxCollider>().enabled = false;

		StartCoroutine(WeaponObjectAnimation(weaponObject.transform));
	}

	void Throw(string uuid)
	{
		var weapon = weaponManager.GetWeaponByUUID(uuid);

		var weaponObject = weapon.gameObject;
		weaponObject.transform.parent = null;
		SetLayerRecursively(weaponObject, (int)Mathf.Log(droppedLayer.value, 2));

		var rb = weaponObject.AddComponent<Rigidbody>();
		rb.mass = 0.1f;
		weaponObject.transform.localPosition = Vector3.zero;
		weaponObject.transform.localRotation = Quaternion.identity;
		var forward = player.playerCam.transform.forward;

		forward.y = 0f;
		rb.velocity = forward * throwForce;
		rb.velocity += Vector3.up * throwExtraForce;
		rb.angularVelocity = UnityEngine.Random.onUnitSphere * rotationForce;

		weaponObject.GetComponent<BoxCollider>().enabled = true;
	}

	IEnumerator WeaponObjectAnimation(Transform objectTransform)
	{
		isMoving = true;

		Vector3 startPosition = objectTransform.localPosition;
		Quaternion startRotation = objectTransform.localRotation;

		var time = 0f;
		while (time < animTime)
		{
			time += Time.deltaTime;
			//var delta = -(Mathf.Cos(Mathf.PI * (time / animTime)) - 1f) / 2f;
			var delta = EasingFunctions.EaseInOutQuad(time / animTime);
			objectTransform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, delta);
			objectTransform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, delta);
			yield return null;
		}

		objectTransform.localPosition = Vector3.zero;
		objectTransform.localRotation = Quaternion.identity;

		isMoving = false;
	}

	static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
		{
			trans.gameObject.layer = layerNumber;
		}
	}
}
