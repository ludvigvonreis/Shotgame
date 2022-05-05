using UnityEngine;

namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponRPM : Weapon.Module, WeaponConstraint.IInterface
	{
		//[SerializeField]
		float rpm = 600;

		public float Delay => 60f / rpm;

		float timer = 0f;

		public bool Constraint => timer > 0f;

		public override void Init()
		{
			base.Init();

			groupReference.OnGroupProcess += Process;
			groupReference.Action.OnPerfom += Action;

			rpm = groupReference.weaponStats.fireRate;
		}

		void Process()
		{
			timer = Mathf.MoveTowards(timer, 0f, Time.deltaTime);
		}

		void Action()
		{
			timer = Delay;
		}
	}
}