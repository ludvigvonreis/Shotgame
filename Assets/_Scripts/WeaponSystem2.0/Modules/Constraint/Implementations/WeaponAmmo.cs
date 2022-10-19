namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponAmmo : Weapon.Module, WeaponConstraint.IInterface
	{
		public bool Constraint => (groupReference.weaponState.currentAmmo <= 0);
	}
}