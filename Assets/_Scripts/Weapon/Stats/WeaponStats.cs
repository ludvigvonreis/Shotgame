using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class WeaponStats : BaseStats
{
	[Header("Basic stats")]
	public float damage;
	public float range;
	public float fireRate;
	public FireMode fireMode;

	[Header("Reloading and ammo")]
	public int maxAmmo;

	[Range(0.1f, 10f)]
	public float reloadTime;

	[Header("Recoil")]
	public AnimationCurve recoilHoriz;
	public AnimationCurve recoilVerti;

	[Range(0.1f, 10f)]
	public float recoilHorizMult;
	[Range(0.1f, 10f)]
	public float recoilVertiMult;
}
