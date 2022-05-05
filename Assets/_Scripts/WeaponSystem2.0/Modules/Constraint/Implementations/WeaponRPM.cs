using UnityEngine;

namespace WeaponSystem
{
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

			groupReference.OnGroupProcess.AddListener(Process);
			groupReference.Action.OnPerfom.AddListener(Action);

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