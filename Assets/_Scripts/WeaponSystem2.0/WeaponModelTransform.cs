using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class WeaponModelTransform : MonoBehaviour
	{
		public Vector3 Offset => m_offset;

		[SerializeField] private float m_updateSpeed;
		[SerializeField] private Vector3 m_offset;
		private Vector3 m_lastOffset;

		private Vector3 m_newPosition;

		private EasingFunctions.Ease easing;
		private EasingFunctions.Ease defaultEasing;

		private bool isLerping;

		private Weapon weaponReference;
		private bool canUpdatePosition => weaponReference.isRunning;

		void Start()
		{
			weaponReference = transform.root.GetComponent<Weapon>();

			defaultEasing = EasingFunctions.Ease.OutQuint;
			easing = defaultEasing;

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

		public WeaponModelTransform SetNewPosition(Vector3 pos)
		{
			m_newPosition = pos + m_offset;

			return this;
		}

		public WeaponModelTransform SetPosition(Vector3 pos)
		{
			//if (isLerping == false) return this;

			transform.localPosition = pos;

			return this;
		}

		public WeaponModelTransform AddNewPosition(Vector3 delta)
		{
			m_newPosition += delta;

			return this;
		}

		public WeaponModelTransform AddPosition(Vector3 delta)
		{
			transform.localPosition += delta;

			return this;
		}

		public WeaponModelTransform ResetPosition()
		{
			m_newPosition = m_offset;

			return this;
		}

		public WeaponModelTransform LerpTo(Vector3 to, float duration)
		{
			StartCoroutine(LerpPosition(to, duration));

			return this;
		}

		public WeaponModelTransform SetEasingFunction(EasingFunctions.Ease func)
		{
			easing = func;

			return this;
		}

		public WeaponModelTransform ResetEasingFunction()
		{
			easing = defaultEasing;

			return this;
		}

		public WeaponModelTransform SetIsLerping(bool value)
		{
			isLerping = value;

			return this;
		}

		IEnumerator UpdatePosition()
		{
			while (true)
			{
				if (canUpdatePosition && !isLerping)
				{
					while (Vector3.Distance(transform.localPosition, m_newPosition) > 0)
					{
						transform.localPosition = Vector3.Lerp(
							transform.localPosition,
							m_newPosition,
							m_updateSpeed * Time.deltaTime
						);
						yield return null;
					}
				}

				yield return null;
			}
		}

		IEnumerator LerpPosition(Vector3 to, float duration)
		{
			var easingFunction = EasingFunctions.GetEase(easing);
			isLerping = true;

			var startPos = transform.localPosition;
			for (float progress = 0; progress < duration; progress += Time.deltaTime)
			{
				var aimedPos = Vector3.Lerp(
					startPos,
					to,
					easingFunction(progress / duration)
				);

				transform.localPosition = aimedPos + m_offset;
				yield return null;
			}
			isLerping = false;

			SetNewPosition(to);
		}
	}
}
