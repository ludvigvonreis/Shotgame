using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace WeaponSystem
{
	public class WeaponAim : WeaponAction
	{
		[SerializeField]
		private Transform aimPoint;
		private bool isAiming;
		private bool isAtOrigin;
		private bool isMoving;

		// To stop MoveBack from running on start
		private bool hasAimedOnce;

		private Vector3 origin;
		private Transform weaponTransform;

		[SerializeField]
		private Camera tempCamera;

		[SerializeField, Range(0.0f, 10.0f)]
		private float test;

		private float moveDuration;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;

			Processor.inputActions[actionButton.action.name].performed += ProcessInput;
			Processor.inputActions[actionButton.action.name].canceled += ProcessInput;

			weaponTransform = groupReference.weaponReference.transform;
			isAtOrigin = true;
			origin = weaponTransform.position;

			moveDuration = groupReference.weaponStats.aimDownSightTime;
		}

		protected override void ProcessInput(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				if (isAiming == false)
				{
					isAiming = true;
				}
			}

			if (context.canceled)
			{
				if (isAiming == true)
				{
					isAiming = false;
				}
			}

			groupReference.weaponState.isAiming = isAiming;
		}

		void Action()
		{
			if (isAiming == true && isAtOrigin == true)
			{
				MoveToAim();
			}

			if (isAiming == false && isAtOrigin == false)
			{
				MoveBack();
			}
		}

		void MoveToAim()
		{
			if (!hasAimedOnce) hasAimedOnce = true;
			if (isMoving) return;

			origin = groupReference.weaponReference.transform.localPosition;

			var pos = tempCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, test));
			var absolute = weaponTransform.InverseTransformPoint(pos) - aimPoint.localPosition;
			var to = weaponTransform.localPosition + absolute;

			StartCoroutine(MoveWeapon(to));

			isAtOrigin = false;
		}

		void MoveBack()
		{
			if (!hasAimedOnce) return;
			if (isMoving) return;

			StartCoroutine(MoveWeapon(origin));

			isAtOrigin = true;
		}

		IEnumerator MoveWeapon(Vector3 to)
		{
			var startPosition = weaponTransform.localPosition;

			if (moveDuration > Mathf.Epsilon)
			{
				isMoving = true;

				for (float progress = 0; progress < moveDuration; progress += Time.deltaTime)
				{
					weaponTransform.localPosition = Vector3.Lerp(startPosition, to, EaseInOutQuad(progress / moveDuration));
					yield return null;
				}
			}

			weaponTransform.localPosition = to;
			isMoving = false;
		}

		float EaseInOutQuad(float x)
		{
			return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
		}
	}
}