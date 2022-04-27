using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	private Player player;
	[SerializeField] private Transform interactTransform;

	[SerializeField, Range(1, 100)]
	private float range = 1;

	void Start()
	{
		player = GetComponent<Player>();
		player.m_InputEvent.AddListener(InputListener);
	}

	private void InputListener(string button, bool down)
	{
		if (button == "Interact" && down) DoInteract();
	}

	void DoInteract()
	{
		RaycastHit hit;
		if (Physics.Raycast(interactTransform.position, interactTransform.forward, out hit, range))
		{
			// Check if object is interactable
			if (hit.transform.TryGetComponent<IInteractible>(out _))
			{
				var interaction = new Interaction(this.gameObject, hit);
				player.m_Interact.Invoke(interaction);
			}
		}
	}
}
