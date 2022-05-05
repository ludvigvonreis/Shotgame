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

			groupReference.OnGroupProcess += Action;
			mouseAction = Processor.inputActions[actionButton.action.name];
		}

		void Action()
		{
			var mouseDelta = -mouseAction.ReadValue<Vector2>() * mouseSensitivity;
			swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
			swayHolder.localPosition += (Vector3)mouseDelta * swaySize;
		}
	}
}