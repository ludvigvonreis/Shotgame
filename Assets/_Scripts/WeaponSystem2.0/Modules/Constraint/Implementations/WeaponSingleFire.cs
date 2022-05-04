namespace WeaponSystem
{
	public class WeaponSingleFire : Weapon.Module, WeaponConstraint.IInterface
	{
		public WeaponAction.IProcessor Processor { get; protected set; }

		private bool isHolding = false;
		private bool hasReleased = false;

		private bool _canShoot = false;
		public bool Constraint => !_canShoot;

		public override void Init()
		{
			base.Init();

			weaponReference.OnProcess.AddListener(Process);
			weaponReference.Action.OnPerfom.AddListener(Action);

			Processor = weaponReference.Action.Processor;
		}

		void Process()
		{
			if (Processor.performed)
			{
				isHolding = true;
			}

			if (Processor.canceled)
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