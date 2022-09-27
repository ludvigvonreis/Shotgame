using UnityEngine;
using UnityEngine.VFX;
using WeaponSystem.Events;

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

		protected override void ProcessInput(object sender, WeaponEvent.ActionContext context)
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