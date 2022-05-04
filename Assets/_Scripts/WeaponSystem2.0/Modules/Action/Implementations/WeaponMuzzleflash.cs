using UnityEngine;
using UnityEngine.VFX;

namespace WeaponSystem
{
	public class WeaponMuzzleflash : WeaponAction
	{
		[SerializeField]
		private VisualEffect muzzleFlash;

		public override void Init()
		{
			base.Init();

			weaponReference.Action.OnPerfom.AddListener(Action);
		}

		// TODO: Implement rest of shooting function
		void Action()
		{
			if (Processor.performed)
			{
				muzzleFlash.Play();
			}
		}
	}
}