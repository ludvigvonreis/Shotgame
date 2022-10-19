using UnityEngine;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponSway : WeaponAction
	{
		[SerializeField] private Transform swayHolder;
		[SerializeField] private WeaponModelTransform weaponMover;

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

			weaponMover = swayHolder.GetComponent<WeaponModelTransform>();
		}

		void Action()
		{
			var extra = groupReference.weaponState.isAiming ? 0.1f : 1f;

			var positionDelta = transform.root.forward - lastCenterPosition;
			lastCenterPosition = transform.root.forward;
			var mouseDelta = positionDelta * extra * swaySize;

			var newPos = (mouseDelta * swaySize);

			weaponMover.AddPosition(newPos);
		}
	}
}