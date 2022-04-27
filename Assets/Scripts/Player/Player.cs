using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputEvent : UnityEvent<string, bool> { }

[System.Serializable]
public class InteractEvent : UnityEvent<Interaction> { }

public class Player : MonoBehaviour
{
	public Camera playerCam;

	// TODO: Move events to singleton event manager.

	// Input handling
	[HideInInspector] public InputEvent m_InputEvent;
	public List<string> buttons = new List<string>();

	[HideInInspector] public UnityEvent m_ShootEvent;
	[HideInInspector] public UnityEvent m_ResetRecoil;
	[HideInInspector] public InteractEvent m_Interact;

	void Start()
	{
		if (m_InputEvent == null)
			m_InputEvent = new InputEvent();

		if (m_Interact == null)
			m_Interact = new InteractEvent();

		if (m_ShootEvent == null)
			m_ShootEvent = new UnityEvent();

		if (m_ResetRecoil == null)
			m_ResetRecoil = new UnityEvent();
	}

	void Update()
	{
		foreach (var item in buttons)
		{
			if (Input.GetButtonDown(item)) m_InputEvent.Invoke(item, true);


			if (Input.GetButtonUp(item)) m_InputEvent.Invoke(item, false);
		}
	}
}
