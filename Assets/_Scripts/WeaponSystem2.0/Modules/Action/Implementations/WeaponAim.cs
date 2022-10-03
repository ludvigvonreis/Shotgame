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

		private float defaultZoomMultiplier = 1.0f;
		private float aimedZoomMultiplier;
		private float playerOriginalFov;
		private float weaponOriginalFov;

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

			moveDuration = groupReference.weaponStats.ADSTime;
			aimedZoomMultiplier = groupReference.weaponStats.ADSZoomMultiplier;

			// FIXME: Should really not be coupled with Player script, find other way.
			// Interface maybe
			var player = transform.root.GetComponent<Gnome.Player>();
			ownerCamera = player.playerCam;
			ownerWeaponCamera = player.weaponCam;

			// FIXME: If fov is changed after module is initialized it wont update.
			playerOriginalFov = ownerCamera.fieldOfView;
			weaponOriginalFov = ownerWeaponCamera.fieldOfView;
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
			StartCoroutine(MoveFov(
				ownerCamera,
				Mathf.Round(playerOriginalFov / aimedZoomMultiplier),
				moveDuration,
				EasingFunctions.EaseInOutQuad
			));
			StartCoroutine(MoveFov(
				ownerWeaponCamera,
				Mathf.Round(playerOriginalFov / aimedZoomMultiplier),
				moveDuration,
				EasingFunctions.EaseInOutQuad
			));

			canAimIn = false;
		}

		void AimOut()
		{
			if (!hasAimedOnce) return;
			StopAllCoroutines();

			StartCoroutine(WeaponMoverOut());
			StartCoroutine(MoveFov(ownerCamera, playerOriginalFov, moveDuration, EasingFunctions.EaseOutQuint));
			StartCoroutine(MoveFov(ownerWeaponCamera, weaponOriginalFov, moveDuration, EasingFunctions.EaseOutQuint));

			canAimIn = true;
		}

		IEnumerator WeaponMoverIn()
		{
			var cameraCenter = ownerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distanceFromCamera));
			Vector3 target = weaponTransform.parent.InverseTransformPoint(
				cameraCenter + (weaponTransform.position - aimPoint.position)
			);

			weaponMover.SetIsLerping(true);

			var start = weaponTransform.localPosition;
			var time = 0f;
			while (time < moveDuration)
			{
				var newPos = Vector3.Lerp(
					start,
					target,
					EasingFunctions.Linear(time / moveDuration)
				);
				time += Time.deltaTime;

				weaponMover.SetPosition(newPos);
				yield return null;
			}
			weaponMover.SetIsLerping(false).SetNewPosition(target - weaponMover.Offset);
		}

		IEnumerator WeaponMoverOut()
		{
			weaponMover.SetIsLerping(true);
			var start = weaponTransform.localPosition;
			var time = 0f;
			while (time < moveDuration)
			{
				var newPos = Vector3.Lerp(
					start,
					weaponMover.Offset,
					EasingFunctions.EaseOutQuint(time / moveDuration)
				);
				time += Time.deltaTime;

				weaponMover.SetPosition(newPos);
				yield return null;
			}

			weaponMover.SetIsLerping(false).SetNewPosition(Vector3.zero);
		}
	}
}