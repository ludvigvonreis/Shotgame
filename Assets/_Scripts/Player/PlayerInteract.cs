using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
	[SerializeField] private InputActionReference interactButton;
	[SerializeField] private InputActionReference dropButton;

	[SerializeField] private Transform interactTransform;
	[SerializeField, Range(1, 100)] private float range = 1;
	[SerializeField, Range(1, 100)] private float radius = 1;

	private Player player;

	void Start()
	{
		player = GetComponent<Player>();

		var interactAction = player.playerInput.actions[interactButton.action.name];
		interactAction.performed += _ => { Interact(); };

		var dropAction = player.playerInput.actions[dropButton.action.name];
		dropAction.performed += _ => { Drop(); };
	}

	void Drop()
	{
		var wepMan = GetComponent<WeaponManager>();
		wepMan.DropCurrent();
	}

	void Interact()
	{
		var hitList = new RaycastHit[256];
		var hitNumber = Physics.CapsuleCastNonAlloc(interactTransform.position,
			interactTransform.position + interactTransform.forward * range, radius, interactTransform.forward,
			hitList);

		var realList = new List<RaycastHit>();
		for (var i = 0; i < hitNumber; i++)
		{
			var hit = hitList[i];

			if (!hit.transform.TryGetComponent<IInteractible>(out _)) continue;

			if (hit.point == Vector3.zero)
			{
				realList.Add(hit);
			}
			else if (Physics.Raycast(interactTransform.position, hit.point - interactTransform.position, out var hitInfo,
				hit.distance + 0.1f) && hitInfo.transform == hit.transform)
			{
				realList.Add(hit);
			}
		}

		if (realList.Count == 0) return;

		realList.Sort((hit1, hit2) =>
		{
			var dist1 = GetDistanceTo(hit1);
			var dist2 = GetDistanceTo(hit2);
			return Mathf.Abs(dist1 - dist2) < 0.001f ? 0 : dist1 < dist2 ? -1 : 1;
		});

		var interactable = realList[0].transform.GetComponent<IInteractible>();

		var type = interactable.InteractType();
		if (type == "Weapon")
		{
			var weapon = realList[0].transform.GetComponent<WeaponSystem.Weapon>();
			GetComponent<WeaponManager>().EquipWeapon(weapon);
		}

		interactable.Interact();
	}

	private float GetDistanceTo(RaycastHit hit)
	{
		return Vector3.Distance(interactTransform.position, hit.point == Vector3.zero ? hit.transform.position : hit.point);
	}
}
