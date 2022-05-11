using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponInteract : MonoBehaviour, Gnome.IInteractible
{
	[SerializeField] private Collider weaponCollider;
	[SerializeField] private LayerMask equippedLayer;
	[SerializeField] private LayerMask droppedLayer;

	[Header("Throwing")]
	[SerializeField] private float throwForce;
	[SerializeField] private float throwExtraForce;
	[SerializeField] private float rotationForce;
	[SerializeField] private float mass;
	[SerializeField] private float drag;

	[HideInInspector] public UnityEvent<Transform, bool> m_onWeaponInteract;

	void Start()
	{
		m_onWeaponInteract = new UnityEvent<Transform, bool>();
		m_onWeaponInteract.AddListener(OnWeaponInteract);
	}

	private void OnWeaponInteract(Transform holder, bool remove)
	{
		if (!remove) Pickup(holder);
		else Throw(holder);
	}

	public void Interact()
	{
	}

	public string InteractType()
	{
		// FIXME: Implement better??
		return "Weapon";
	}

	void Pickup(Transform holder)
	{
		transform.parent = holder;
		SetLayerRecursively(gameObject, (int)Mathf.Log(equippedLayer.value, 2));

		Rigidbody rb;
		if (TryGetComponent<Rigidbody>(out rb))
		{
			Destroy(rb);
		}

		weaponCollider.enabled = false;
	}

	void Throw(Transform holder)
	{
		transform.parent = null;
		SetLayerRecursively(gameObject, (int)Mathf.Log(droppedLayer.value, 2));

		var rb = gameObject.AddComponent<Rigidbody>();
		rb.mass = mass;
		rb.drag = drag;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		//FIXME: not using camera but should?? 
		var forward = holder.forward;//player.playerCam.transform.forward;

		forward.y = 0f;
		rb.velocity = forward * throwForce;
		rb.velocity += Vector3.up * throwExtraForce;
		rb.angularVelocity = UnityEngine.Random.onUnitSphere * rotationForce;

		weaponCollider.enabled = true;
	}

	static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
		{
			trans.gameObject.layer = layerNumber;
		}
	}
}