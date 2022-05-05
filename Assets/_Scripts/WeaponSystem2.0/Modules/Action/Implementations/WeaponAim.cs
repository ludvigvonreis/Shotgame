using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using System.Linq;

namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponAim : WeaponAction
	{
		[SerializeField] private Transform aimPoint;
		[SerializeField] private float fovMoveDuration;
		[SerializeField] private float aimedFov;
		private float originFov;
		private float origin2Fov;

		private float moveDuration;

		[SerializeField]
		private bool isAiming;
		private bool isAtOrigin;
		private bool isMoving;

		// To stop MoveBack from running on start
		private bool hasAimedOnce;

		private Vector3 origin;
		private Transform weaponTransform;

		private Camera ownerCamera;
		private Camera ownerWeaponCamera;

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

			// FIXME: Temporary until player script is activated
			var cameras = groupReference.owner.ownerObject.GetComponentsInChildren<Camera>();
			ownerCamera = cameras.Single(x => x.name == "PlayerCamera");
			ownerWeaponCamera = cameras.Single(x => x.name == "WeaponCamera");
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
			originFov = ownerCamera.fieldOfView;
			origin2Fov = ownerWeaponCamera.fieldOfView;

			StartCoroutine(MoveWeapon(aimPoint.localPosition));
			StartCoroutine(MoveFov(aimedFov, aimedFov));

			isAtOrigin = false;
		}

		void MoveBack()
		{
			if (!hasAimedOnce) return;
			if (isMoving) return;

			StartCoroutine(MoveWeapon(origin));
			StartCoroutine(MoveFov(originFov, origin2Fov));

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
					weaponTransform.localPosition = Vector3.Lerp(startPosition, to, EasingFunctions.EaseInOutQuad(progress / moveDuration));
					yield return null;
				}
			}

			weaponTransform.localPosition = to;
			isMoving = false;
		}

		IEnumerator MoveFov(float to, float to2)
		{
			var start1 = ownerCamera.fieldOfView;
			var start2 = ownerWeaponCamera.fieldOfView;

			for (float progress = 0; progress < fovMoveDuration; progress += Time.deltaTime)
			{
				ownerCamera.fieldOfView = Mathf.Lerp(start1, to, EasingFunctions.EaseInOutQuad(progress / fovMoveDuration));
				ownerWeaponCamera.fieldOfView = Mathf.Lerp(start2, to2, EasingFunctions.EaseInOutQuad(progress / fovMoveDuration));
				yield return null;
			}

			ownerCamera.fieldOfView = to;
			ownerWeaponCamera.fieldOfView = to2;
		}
	}
}