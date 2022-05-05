namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponIsReloading : Weapon.Module, WeaponConstraint.IInterface
	{
		public bool Constraint => (groupReference.weaponState.isReloading);
	}
}