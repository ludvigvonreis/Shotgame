using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	[SerializeField] private Transform interactTransform;
	[SerializeField, Range(1, 100)]
	private float range = 1;

	private Player player;

	void Start()
	{
		player = GetComponent<Player>();

		var interactAction = player.playerInput.actions[player.interactButton.action.name];
		interactAction.performed += _ => { Interact(); };
	}


	void Interact()
	{
		RaycastHit hit;
		if (Physics.Raycast(interactTransform.position, interactTransform.forward, out hit, range))
		{
			// Check if object is interactable
			if (hit.transform.TryGetComponent<IInteractible>(out _))
			{
				var interaction = new Interaction(this.gameObject, hit);
				EventManager.Instance.m_Interact.Invoke(interaction);
			}
		}
	}
}
