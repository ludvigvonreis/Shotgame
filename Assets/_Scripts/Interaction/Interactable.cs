using UnityEngine;

public interface IInteractible
{
	public void Interact();
	public string InteractType();
}

public class InteractionEvent
{
	public GameObject interactor;
	public GameObject subject;

	public float distance;

	public InteractionEvent(GameObject _from, RaycastHit hit)
	{
		interactor = _from;

		subject = hit.transform.gameObject;
		distance = hit.distance;
	}
}