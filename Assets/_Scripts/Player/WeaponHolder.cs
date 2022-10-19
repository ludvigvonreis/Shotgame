using System;
using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace Gnome
{
	public class WeaponHolder : MonoBehaviour
	{
		[SerializeField] private Transform weaponHolderTransform;
		[SerializeField] private float animTime = 1.5f;

		private bool isMoving = false;

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

			weapon.GetComponent<WeaponInteract>().m_onWeaponInteract.Invoke(weaponHolderTransform, false);

			StartCoroutine(WeaponObjectAnimation(weaponObject.transform));
		}

		void Throw(string uuid)
		{
			var weapon = weaponManager.GetWeaponByUUID(uuid);
			var weaponObject = weapon.gameObject;
			weapon.GetComponent<WeaponInteract>().m_onWeaponInteract.Invoke(weaponHolderTransform, true);

			var weaponCollider = weaponObject.GetComponent<BoxCollider>();
			Physics.IgnoreCollision(weaponCollider, player.GetComponent<Collider>());
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
	}
}