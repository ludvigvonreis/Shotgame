using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputEvent : UnityEvent<string, bool> { }

public class Player : MonoBehaviour
{
	public Camera playerCam;

	// Input handling
	[HideInInspector] public InputEvent m_InputEvent;
	public List<string> buttons = new List<string>();

	[HideInInspector] public UnityEvent m_ShootEvent;
	[HideInInspector] public UnityEvent m_ResetRecoil;

	void Start()
	{
		if (m_InputEvent == null)
			m_InputEvent = new InputEvent();

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
