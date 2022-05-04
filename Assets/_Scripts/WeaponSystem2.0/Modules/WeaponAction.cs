using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	public class WeaponAction : Weapon.Module
	{
		public ActionHandler actionHandler;

		public override void Init()
		{
			base.Init();
		}

		public override void Configure(ActionHandler _actionHandler)
		{
			base.Configure(_actionHandler);

			actionHandler = _actionHandler;
		}

		void Perform()
		{

		}
	}
}