using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	public Camera playerCam;

	public UnityEvent m_ShootEvent;
	public UnityEvent m_ResetRecoil;

	void Start()
	{
		if (m_ShootEvent == null)
			m_ShootEvent = new UnityEvent();

		if (m_ResetRecoil == null)
			m_ResetRecoil = new UnityEvent();
	}
}
