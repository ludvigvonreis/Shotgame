using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	public class Weapon : MonoBehaviour
	{
		#region Interfaces
		public interface IOwner
		{
			GameObject ownerObject { get; }
			List<IProcessor> Processors { get; }
		}

		public interface IBehaviour
		{
			//Like Awake
			void Configure();

			//Like Start
			void Init();
		}

		public abstract class Behaviour : MonoBehaviour, IBehaviour
		{
			public virtual void Configure() { }

			public virtual void Init() { }
		}

		public interface IModule
		{
			void Set(Weapon reference);
		}

		public abstract class Module : Behaviour, IModule
		{
			public Weapon weaponReference { get; protected set; }

			public virtual void Set(Weapon reference)
			{
				weaponReference = reference;
			}
		}

		public interface IProcessor { }
		#endregion

		public IOwner owner { get; protected set; }
		public List<IBehaviour> behaviours { get; protected set; } = new List<IBehaviour>();
		public List<IModule> modules { get; protected set; } = new List<IModule>();


		public WeaponAction Action { get; protected set; }
		public WeaponConstraint Constraint { get; protected set; }

		public WeaponState weaponState;
		public WeaponStats weaponStats;

		public void Setup(IOwner reference)
		{
			if (!TryGetComponent<WeaponState>(out weaponState))
			{
				weaponState = this.gameObject.AddComponent<WeaponState>();
			}
			weaponState.Init(weaponStats);

			owner = reference;

			behaviours = GetComponentsInChildren<IBehaviour>(true).ToList();
			modules = GetComponentsInChildren<IModule>(true).ToList();

			Action = modules.First(x => x is WeaponAction) as WeaponAction;
			Constraint = modules.First(x => x is WeaponConstraint) as WeaponConstraint;



			modules.ForEach(x => x.Set(this));
			behaviours.ForEach(x => x.Configure());
			behaviours.ForEach(x => x.Init());
		}

		void Update()
		{
			Process();
		}

		[HideInInspector]
		public UnityEvent OnProcess;
		void Process()
		{
			OnProcess?.Invoke();
		}

		public T GetProcessor<T>()
	where T : IProcessor
		{
			foreach (var _processor in owner.Processors)
			{
				if (_processor is T processor)
					return processor;
			}

			throw new Exception($"No Processor of Type {typeof(T)} Found");
		}
	}
}