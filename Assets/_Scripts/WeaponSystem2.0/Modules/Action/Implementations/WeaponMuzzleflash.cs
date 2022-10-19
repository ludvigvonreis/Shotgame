using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponMuzzleflash : WeaponAction
	{
		[SerializeField]
		private VisualEffect muzzleFlash;

		bool performed;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;
		}

		protected override void ProcessInput(InputAction.CallbackContext context)
		{
			if (groupReference.isRunning == false) return;

			performed = context.performed;
		}

		void Action()
		{
			if (performed)
			{
				muzzleFlash.Play();
			}
		}
	}
}