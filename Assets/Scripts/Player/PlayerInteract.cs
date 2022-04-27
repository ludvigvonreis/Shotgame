using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	[SerializeField, Range(1, 100)]
	private float range = 1;

	private Player player;
	private Transform interactTransform;

	void Start()
	{
		player = GetComponent<Player>();
		player.m_InputEvent.AddListener(InputListener);
	}

	private void InputListener(string button, bool down)
	{
		if (button == "Interact") DoInteract();
	}

	void DoInteract()
	{
		RaycastHit hit;
		if (Physics.Raycast(interactTransform.position, interactTransform.forward, out hit, range))
		{
			Debug.Log("Interacted with something");

			var interaction = new Interaction(this.gameObject, hit);

			player.m_Interact.Invoke(interaction);
		}
	}
}
