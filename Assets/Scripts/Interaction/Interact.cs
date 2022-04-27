using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
	public bool CanInteract();

	public void Interact();

	public void PostInteract();
}

public interface IPickup
{
	// Check if pickup is possible
	public bool CanPickup();

	// Object side logic
	public void Pickup();

	// potential cleanup after interaction. ex after pickup destroy self
	public void PostPickup();
}

public class Interaction
{
	public GameObject from;
	public GameObject to;

	public float distance;

	public Interaction(GameObject _from, RaycastHit hit)
	{
		from = _from;

		to = hit.transform.gameObject;
		distance = hit.distance;
	}
}