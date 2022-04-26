using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
	// Check if pickup is possible
	public bool CanPickup();

	// Object side logic
	public void Pickup();

	// potential cleanup after interaction. ex after pickup destroy self
	public void PostPickup();
}

public class Pickup : MonoBehaviour
{
	[SerializeField, Range(1, 100)]
	private float range = 1;

	// FIXME: These should not be referenced like this
	// it should be given by a "Player" script or something.
	[SerializeField]
	private WeaponManager wepMan;

	private Camera interactCamera;

	void Start()
	{
		interactCamera = GetComponent<Player>().playerCam;
	}

	void Update()
	{
		if (Input.GetButtonDown("Interact"))
		{
			DoPickup();
		}
	}

	void DoPickup()
	{
		RaycastHit hit;
		if (Physics.Raycast(interactCamera.transform.position, interactCamera.transform.forward, out hit, range))
		{
			GameObject hitObj = hit.transform.gameObject;

			IPickup inter;
			if (hitObj.TryGetComponent<IPickup>(out inter))
			{
				inter.Pickup();
				var wepObj = hitObj.GetComponent<WeaponObject>();

				if (wepMan.CanEquip)
				{
					wepMan.AddWeapon(wepObj);
					inter.PostPickup();
				}
			}
		}
	}
}
