using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
	public Camera playerCam;
	public PlayerInput playerInput;

	[Header("Button references")]
	public InputActionReference interactButton;
	public InputActionReference shootButton;
	public InputActionReference aimButton;
	public InputActionReference reloadButton;
	public InputActionReference mouseButton; // Not really a button but watevs

	[HideInInspector] public UnityEvent m_ShootEvent;
	[HideInInspector] public UnityEvent m_ResetRecoil;

	void OnValidate()
	{
		playerInput = GetComponent<PlayerInput>();
	}

	void Awake()
	{
		if (m_ShootEvent == null)
			m_ShootEvent = new UnityEvent();

		if (m_ResetRecoil == null)
			m_ResetRecoil = new UnityEvent();
	}

	void Start()
	{
		if (interactButton == null) Debug.LogError("you need to define interact button");

		if (shootButton == null) Debug.LogError("you need to define shoot button");
		if (aimButton == null) Debug.LogError("you need to define aim button");
		if (reloadButton == null) Debug.LogError("you need to define reload button");

		if (mouseButton == null) Debug.LogError("you need to define mouse button");
	}

}
