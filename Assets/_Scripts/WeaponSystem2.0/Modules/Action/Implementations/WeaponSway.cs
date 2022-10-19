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

		public override void Init()
		{
			base.Init();

			lastCenterPosition = transform.root.forward;

			groupReference.Action.OnPerfom += Action;
		}

		void Action()
		{
			var extra = groupReference.weaponState.isAiming ? 0.1f : 1f;

			var positionDelta = transform.root.forward - lastCenterPosition;
			lastCenterPosition = transform.root.forward;
			var mouseDelta = (positionDelta * mouseSensitivity) * extra;

			swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
			swayHolder.localPosition += (Vector3)mouseDelta * swaySize;
		}
	}
}