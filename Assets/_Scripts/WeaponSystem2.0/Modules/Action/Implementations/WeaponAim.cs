using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace WeaponSystem
{
	public class WeaponAim : WeaponAction
	{
		[SerializeField]
		private Transform aimPoint;

		public override void Init()
		{
			base.Init();

			weaponReference.Action.OnPerfom.AddListener(Action);
		}

		void Action()
		{
			if (inputContext.performed)
			{
				Debug.Log("Aim in");
			}
		}
	}
}