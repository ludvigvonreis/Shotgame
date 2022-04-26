using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	public Camera playerCam;

	public UnityEvent m_ShootEvent;

	void Start()
	{
		if (m_ShootEvent == null)
			m_ShootEvent = new UnityEvent();
	}
}
