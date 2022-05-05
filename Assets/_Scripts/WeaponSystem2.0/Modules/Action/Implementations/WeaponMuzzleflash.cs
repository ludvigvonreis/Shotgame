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

			groupReference.Action.OnPerfom.AddListener(Action);
		}

		// TODO: Implement rest of shooting function
		void Action()
		{
			if (inputContext.performed)
			{
				muzzleFlash.Play();
			}
		}
	}
}