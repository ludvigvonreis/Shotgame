using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
	public class WeaponAction : Weapon.Module
	{
		public IProcessor Processor { get; protected set; }
		public interface IProcessor : Weapon.IProcessor
		{
			bool performed { get; }
			bool canceled { get; }
		}

		public WeaponConstraint Constraint => weaponReference.Constraint;

		public override void Init()
		{
			base.Init();

			Processor = weaponReference.GetProcessor<IProcessor>();

			weaponReference.OnProcess.AddListener(Process);
		}

		void Process()
		{
			if (Constraint.Active) return;

			Perform();
		}

		[HideInInspector]
		public UnityEvent OnPerfom;
		void Perform()
		{
			OnPerfom?.Invoke();
		}
	}
}