using UnityEngine;

namespace WeaponSystem
{
	public class BaseStats : ScriptableObject
	{
		[ReadOnly]
		public string ID;
		public new string name;

		[Header("Visual")]
		public GameObject model;

		void OnValidate()
		{
			if (!System.Guid.TryParse(ID, out _))
				ID = System.Guid.NewGuid().ToString();
		}
	}

	[CreateAssetMenu(fileName = "New Weapon stats", menuName = "Weapon/Weapon stats")]
	public class WeaponStats : BaseStats
	{
		[Header("Basic stats")]
		public float damage;
		public float range;
		public float fireRate;

		[Header("Reloading and ammo")]
		public int maxAmmo;

		[Range(0.1f, 10f)]
		public float reloadTime;

		[Header("Recoil")]
		public AnimationCurve recoilHoriz;
		public AnimationCurve recoilVerti;

		[Range(0.1f, 10f)] public float recoilHorizMult;
		[Range(0.1f, 10f)] public float recoilVertiMult;
	}
}
