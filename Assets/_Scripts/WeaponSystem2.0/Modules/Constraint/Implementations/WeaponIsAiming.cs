namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponIsAiming : Weapon.Module, WeaponConstraint.IInterface
	{
		public bool Constraint => (groupReference.weaponState.isAiming);
	}
}