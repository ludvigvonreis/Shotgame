using UnityEngine;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponSway : WeaponAction
	{
		[SerializeField] private Transform swayHolder;
		[SerializeField] private Vector2 mouseSensitivity;
		[SerializeField] private float swaySize;
		[SerializeField] private float swaySmooth;

		private Vector3 lastCenterPosition;
		private Quaternion initialRotation;

		public override void Init()
		{
			base.Init();

			lastCenterPosition = transform.root.forward;
			initialRotation = swayHolder.localRotation;

			groupReference.Action.OnPerfom += Action;
		}

		void Action()
		{
			var extra = groupReference.weaponState.isAiming ? 0.1f : 1f;

			var positionDelta = transform.root.forward - lastCenterPosition;
			lastCenterPosition = transform.root.forward;
			var mouseDelta = positionDelta * extra * swaySize;

			/*swayHolder.localPosition = EasingFunctions.EasedLerp(
				swayHolder.localPosition,
				Vector3.zero,
				swaySmooth * Time.deltaTime,
				EasingFunctions.EaseInOutCubic
				);*/

			//swayHolder.localPosition += (Vector3)mouseDelta * swaySize;

			Quaternion final = Quaternion.Euler(initialRotation.x + mouseDelta.y, initialRotation.x + mouseDelta.x, initialRotation.z + mouseDelta.z);
			swayHolder.localRotation = Quaternion.Slerp(swayHolder.localRotation, final, Time.time * swaySmooth);
		}
	}
}