using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class WeaponModelTransform : MonoBehaviour
	{
		public Vector3 Offset => m_offset;

		[SerializeField] private float m_updateTime;
		[SerializeField] private Vector3 m_offset;
		private Vector3 m_lastOffset;

		private Vector3 m_newPosition;


		private Weapon weaponReference;
		private bool canUpdatePosition => weaponReference.isRunning;

		void Start()
		{
			weaponReference = transform.root.GetComponent<Weapon>();

			StartCoroutine(UpdatePosition());

			m_lastOffset = m_offset;
			m_newPosition = m_offset;
		}

		void Update()
		{
			if (m_lastOffset != m_offset)
			{
				m_newPosition -= m_lastOffset;
				m_newPosition += m_offset;

				m_lastOffset = m_offset;
			}
		}

		public void SetPosition(Vector3 pos)
		{
			m_newPosition = pos + m_offset;
		}

		IEnumerator UpdatePosition()
		{
			while (true)
			{
				if (canUpdatePosition)
				{
					while (Vector3.Distance(transform.localPosition, m_newPosition) > 0)
					{
						transform.localPosition = Vector3.Lerp(
								transform.localPosition,
								m_newPosition,
								EasingFunctions.EaseOutQuint(Time.deltaTime)
								);
						yield return null;
					}

				}

				yield return null;
			}
		}
	}
}
