using UnityEngine;

namespace WeaponSystem
{
	public class BaseStats : ScriptableObject
	{
		[ReadOnly]
		public string ID;
		public new string name;

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
		[Tooltip("Range a weapon can hit to.")]
		public float range;
		[Tooltip("Weapon firerate expressed in rounds per minute.")]
		public float fireRate;

		[Header("ADS")]
		[Tooltip("Time it takes to aim down sights.")]
		public float ADSTime;
		[Tooltip("Zoom multiplier when aiming down sights")]
		public float ADSZoomMultiplier;

		[Header("Reloading and ammo")]
		public int magazineSize;

		[Range(0.1f, 10f)]
		public float reloadTime;

		[Header("Recoil")]
		public AnimationCurve recoilHoriz;
		public AnimationCurve recoilVerti;

		[Range(0.1f, 10f)] public float recoilHorizMult;
		[Range(0.1f, 10f)] public float recoilVertiMult;

		[Range(0.01f, 10f)] public float shootTimeoutTime;
	}
}
