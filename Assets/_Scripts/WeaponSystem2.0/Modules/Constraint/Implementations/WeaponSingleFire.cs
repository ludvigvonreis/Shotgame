using UnityEngine.InputSystem;

namespace WeaponSystem
{
	[System.Serializable]
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

			groupReference.OnGroupProcess += Process;
			groupReference.Action.OnPerfom += Action;

			//var actionButton = groupReference.Action.actionButton;

			//groupReference.Action.Processor.inputActions[actionButton.action.name].performed += ProcessInput;
			//groupReference.Action.Processor.inputActions[actionButton.action.name].canceled += ProcessInput;
		}

		/*protected void ProcessInput(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				isHolding = true;
			}

			if (context.canceled)
			{
				isHolding = false;
				hasReleased = true;
			}
		}*/

		void Process()
		{
			_canShoot = (isHolding && hasReleased);
		}

		void Action()
		{
			hasReleased = false;
		}
	}
}