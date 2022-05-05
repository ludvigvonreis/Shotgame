using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace WeaponSystem
{
	public class WeaponModuleGroup : MonoBehaviour
	{
		public Weapon weaponReference;

		public List<Weapon.IBehaviour> behaviours { get; protected set; } = new List<Weapon.IBehaviour>();
		public List<Weapon.IModule> modules { get; protected set; } = new List<Weapon.IModule>();

		public WeaponAction Action { get; protected set; }
		public WeaponConstraint Constraint { get; protected set; }

		public WeaponState weaponState => weaponReference.weaponState;
		public WeaponStats weaponStats => weaponReference.weaponStats;
		public Weapon.IOwner owner => weaponReference.owner;

		public void Init(Weapon reference)
		{
			weaponReference = reference;

			behaviours = GetComponentsInChildren<Weapon.IBehaviour>(false).ToList();
			modules = GetComponentsInChildren<Weapon.IModule>(false).ToList();

			Action = modules.First(x => x is WeaponAction) as WeaponAction;
			Constraint = modules.First(x => x is WeaponConstraint) as WeaponConstraint;

			modules.ForEach(x => x.Set(this));
			behaviours.ForEach(x => x.Configure());
			behaviours.ForEach(x => x.Init());

			weaponReference.OnProcess += Process;
		}

		[HideInInspector]
		public event Action OnGroupProcess;
		void Process()
		{
			OnGroupProcess?.Invoke();
		}

		public T GetProcessor<T>()
where T : Weapon.IProcessor
		{
			return weaponReference.GetProcessor<T>();
		}
	}
}
