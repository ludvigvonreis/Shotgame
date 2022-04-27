using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
	public void Interact();

	public void PostInteract();
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