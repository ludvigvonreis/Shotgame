using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
	// Check if interaction is possible
	public bool CanInteract();

	// Object side interaction logic
	public void Interact();

	// Post interaction logic
	public void PostInteract();
}

public class Interact : MonoBehaviour
{
	[SerializeField, Range(1, 100)]
	private float range = 1;

	// FIXME: This should not be referenced like this
	// it should be given by a "Player" script or something.
	[SerializeField]
	private Camera interactCamera;

	void Update()
	{
		// TODO: Add visual feedback for interaction. i.e., glow or outline
	}

	// Should be ran on button press
	void PerformInteraction()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, interactCamera.transform.forward, out hit, range))
		{
			IInteractible inter;
			if (hit.transform.gameObject.TryGetComponent<IInteractible>(out inter))
			{
				if (inter.CanInteract())
					inter.Interact();
			}
		}
	}
}
