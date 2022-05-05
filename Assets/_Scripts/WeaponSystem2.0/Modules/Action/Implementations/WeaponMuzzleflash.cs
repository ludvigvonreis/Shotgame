using UnityEngine;
using UnityEngine.VFX;

namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponMuzzleflash : WeaponAction
	{
		[SerializeField]
		private VisualEffect muzzleFlash;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;
		}

		void Action()
		{
			if (inputContext.performed)
			{
				muzzleFlash.Play();
			}
		}
	}
}