namespace WeaponSystem
{
	public class WeaponAmmo : Weapon.Module, WeaponConstraint.IInterface
	{
		public bool Constraint => (weaponReference.weaponState.currentAmmo <= 0);

		public override void Init()
		{
			base.Init();
		}
	}
}