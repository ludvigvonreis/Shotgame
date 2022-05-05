using UnityEngine.InputSystem;

namespace WeaponSystem
{
	public class WeaponSingleFire : Weapon.Module, WeaponConstraint.IInterface
	{
		private InputAction.CallbackContext inputContext;

		private bool isHolding = false;
		private bool hasReleased = false;

		private bool _canShoot = false;
		public bool Constraint => !_canShoot;

		public override void Init()
		{
			base.Init();

			weaponReference.OnProcess.AddListener(Process);
			weaponReference.Action.OnPerfom.AddListener(Action);

			inputContext = weaponReference.Action.inputContext;
		}

		void Process()
		{
			if (inputContext.performed)
			{
				isHolding = true;
			}

			if (inputContext.canceled)
			{
				isHolding = false;
				hasReleased = true;
			}

			_canShoot = (isHolding && hasReleased);
		}

		void Action()
		{
			hasReleased = false;
		}
	}
}