using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponSway : WeaponAction
	{
		[SerializeField] private Transform swayHolder;
		[SerializeField] private Vector2 mouseSensitivity;
		[SerializeField] private float swaySize;
		[SerializeField] private float swaySmooth;

		private InputAction mouseAction;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;
			mouseAction = Processor.inputActions[actionButton.action.name];
		}

		void Action()
		{
			var extra = groupReference.weaponState.isAiming ? 0.1f : 1f;
			var mouseDelta = (-mouseAction.ReadValue<Vector2>() * mouseSensitivity) * extra;
			swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
			swayHolder.localPosition += (Vector3)mouseDelta * swaySize;
		}
	}
}