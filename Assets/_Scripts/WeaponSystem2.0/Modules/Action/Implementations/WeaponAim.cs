using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using static TimedMoving;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponAim : WeaponAction
	{
		[SerializeField] private Transform aimPoint;
		private float aimedFov;

		private float mainOriginFov;
		private float weaponOriginFov;

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

			weaponTransform = groupReference.weaponReference.transform;
			isAtOrigin = true;
			origin = weaponTransform.position;

			moveDuration = groupReference.weaponStats.aimDownSightTime;
			aimedFov = groupReference.weaponStats.aimDownSightFov;

			// TODO: Should really not be coupled with Player script, find other way.
			// Interface maybe
			//var player = groupReference.owner.ownerObject.GetComponent<Gnome.Player>();
			//ownerCamera = player.playerCam;
			//ownerWeaponCamera = player.weaponCam;

			// FIXME: If fov is changed after module is initialized it wont update.
			origin = Vector3.zero; //groupReference.weaponReference.transform.localPosition;
								   //mainOriginFov = ownerCamera.fieldOfView;
								   //weaponOriginFov = ownerWeaponCamera.fieldOfView;
		}

		protected override void ProcessInput(object sender, WeaponEvent.ActionContext context)
		{
			if (groupReference.isRunning == false) return;

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
			StopAllCoroutines();

			StartCoroutine(MoveTransformPosition(
				weaponTransform,
				aimPoint.localPosition,
				moveDuration,
				EasingFunctions.EaseOutQuint,
				true)
			);

			//StartCoroutine(MoveFov(ownerCamera, aimedFov, moveDuration, EasingFunctions.EaseInOutQuad));
			//StartCoroutine(MoveFov(ownerWeaponCamera, aimedFov, moveDuration, EasingFunctions.EaseInOutQuad));

			isAtOrigin = false;
		}

		void MoveBack()
		{
			if (!hasAimedOnce) return;
			StopAllCoroutines();

			StartCoroutine(MoveTransformPosition(weaponTransform, origin, moveDuration, EasingFunctions.EaseOutQuad, true));
			//StartCoroutine(MoveFov(ownerCamera, mainOriginFov, moveDuration, EasingFunctions.EaseInOutQuad));
			//StartCoroutine(MoveFov(ownerWeaponCamera, weaponOriginFov, moveDuration, EasingFunctions.EaseInOutQuad));

			isAtOrigin = true;
		}
	}
}