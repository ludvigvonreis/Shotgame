using System.Collections;
using UnityEngine;
using static TimedMoving;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponAim : WeaponAction
	{
		[SerializeField] private Transform aimPoint;
		[SerializeField] private Transform weaponTransform;
		[SerializeField] private float distanceFromCamera;
		private WeaponModelTransform weaponMover;

		private float aimedFov;
		private float mainOriginFov;
		private float weaponOriginFov;

		private float moveDuration;

		// Is currently holding aim action
		[SerializeField] private bool isAimingAction;
		private bool canAimIn;

		// True if weapon has been aimed once.
		// To stop MoveBack from running on start
		private bool hasAimedOnce;

		private Camera ownerCamera;
		private Camera ownerWeaponCamera;


		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;

			weaponMover = weaponTransform.GetComponent<WeaponModelTransform>();

			canAimIn = true;

			moveDuration = groupReference.weaponStats.aimDownSightTime;
			aimedFov = groupReference.weaponStats.aimDownSightFov;

			// FIXME: Should really not be coupled with Player script, find other way.
			// Interface maybe
			var player = transform.root.GetComponent<Gnome.Player>();
			ownerCamera = player.playerCam;
			ownerWeaponCamera = player.weaponCam;

			// FIXME: If fov is changed after module is initialized it wont update.
			mainOriginFov = ownerCamera.fieldOfView;
			weaponOriginFov = ownerWeaponCamera.fieldOfView;
		}

		protected override void ProcessInput(object sender, WeaponEvent.ActionContext context)
		{
			if (groupReference.isRunning == false) return;

			if (context.performed)
			{
				if (isAimingAction == false)
				{
					isAimingAction = true;
				}
			}

			if (context.canceled)
			{
				if (isAimingAction == true)
				{
					isAimingAction = false;
				}
			}

			groupReference.weaponState.isAiming = isAimingAction;
		}

		void Action()
		{
			if (isAimingAction == true && canAimIn == true)
			{
				AimIn();
			}

			if (isAimingAction == false && canAimIn == false)
			{
				AimOut();
			}
		}

		void AimIn()
		{
			if (!hasAimedOnce) hasAimedOnce = true;
			StopAllCoroutines();

			StartCoroutine(WeaponMoverIn());
			StartCoroutine(MoveFov(ownerCamera, aimedFov, moveDuration, EasingFunctions.EaseInOutQuad));
			StartCoroutine(MoveFov(ownerWeaponCamera, aimedFov, moveDuration, EasingFunctions.EaseInOutQuad));

			canAimIn = false;
		}

		void AimOut()
		{
			if (!hasAimedOnce) return;
			StopAllCoroutines();

			StartCoroutine(WeaponMoverOut());
			StartCoroutine(MoveFov(ownerCamera, mainOriginFov, moveDuration, EasingFunctions.EaseInOutQuad));
			StartCoroutine(MoveFov(ownerWeaponCamera, weaponOriginFov, moveDuration, EasingFunctions.EaseInOutQuad));

			canAimIn = true;
		}

		IEnumerator WeaponMoverIn()
		{
			var startPos = weaponTransform.localPosition;

			var cameraCenter = ownerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distanceFromCamera));
			Vector3 result = weaponTransform.parent.InverseTransformPoint(
				cameraCenter + (weaponTransform.position - aimPoint.position)
			);

			for (float progress = 0; progress < moveDuration; progress += Time.deltaTime)
			{
				var aimedPos = Vector3.Lerp(
					startPos,
					result,
					progress / moveDuration
				);
				weaponMover.SetPosition(aimedPos - weaponMover.Offset);
				yield return null;
			}

			weaponMover.SetPosition(result - weaponMover.Offset);
		}

		IEnumerator WeaponMoverOut()
		{
			var startPos = weaponTransform.localPosition;
			for (float progress = 0; progress < moveDuration; progress += Time.deltaTime)
			{
				weaponMover.SetPosition(
						Vector3.zero
					);
				yield return null;
			}
		}
	}
}