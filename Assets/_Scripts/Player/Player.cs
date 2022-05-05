using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class HealthChange
{
	public float health;
	public float maxHealth;

	public HealthChange(float _h, float _mh)
	{
		health = _h;
		maxHealth = _mh;
	}
}

[System.Serializable]
public class HealthChangeEvent : UnityEvent<HealthChange> { }

public class Player : MonoBehaviour
{
	/*
	public Camera playerCam;
	public PlayerInput playerInput;

	[Header("Player stats")]
	public float health;
	public float maxHealth;

	[Header("Button references")]
	public InputActionReference interactButton;
	public InputActionReference shootButton;
	public InputActionReference aimButton;
	public InputActionReference reloadButton;
	public InputActionReference mouseButton; // Not really a button but watevs

	[HideInInspector] public UnityEvent m_ShootEvent;
	[HideInInspector] public UnityEvent m_ResetRecoil;
	[HideInInspector] public HealthChangeEvent m_HealthChange;
	[HideInInspector] public UnityEvent m_AmmoChange;

	void OnValidate()
	{
		playerInput = GetComponent<PlayerInput>();
	}

	void Awake()
	{
		if (m_ShootEvent == null)
			m_ShootEvent = new UnityEvent();

		if (m_HealthChange == null)
			m_HealthChange = new HealthChangeEvent();

		if (m_ResetRecoil == null)
			m_ResetRecoil = new UnityEvent();

		if (m_AmmoChange == null)
			m_AmmoChange = new UnityEvent();
	}

	void Start()
	{
		health = maxHealth;

		if (interactButton == null) Debug.LogError("you need to define interact button");

		if (shootButton == null) Debug.LogError("you need to define shoot button");
		if (aimButton == null) Debug.LogError("you need to define aim button");
		if (reloadButton == null) Debug.LogError("you need to define reload button");

		if (mouseButton == null) Debug.LogError("you need to define mouse button");
	}

	// TODO: Refactor this in a better way
	public void DealDamage(float damage)
	{
		health = Mathf.Max(health - damage, 0);

		m_HealthChange.Invoke(new HealthChange(health, maxHealth));
	}
	*/
}
