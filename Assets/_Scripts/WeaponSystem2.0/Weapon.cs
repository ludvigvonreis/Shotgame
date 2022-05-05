using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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
			void Set(WeaponModuleGroup reference);
		}

		public abstract class Module : Behaviour, IModule
		{
			public WeaponModuleGroup groupReference { get; protected set; }

			public virtual void Set(WeaponModuleGroup reference)
			{
				groupReference = reference;
			}
		}

		public interface IProcessor { }
		#endregion

		public IOwner owner { get; protected set; }
		public List<WeaponModuleGroup> moduleGroups { get; protected set; } = new List<WeaponModuleGroup>();

		public WeaponState weaponState;
		public WeaponStats weaponStats;

		public void Setup(IOwner reference)
		{
			owner = reference;

			if (!TryGetComponent<WeaponState>(out weaponState))
			{
				weaponState = this.gameObject.AddComponent<WeaponState>();
			}
			weaponState.Init(weaponStats);

			moduleGroups = GetComponentsInChildren<WeaponModuleGroup>(true).ToList();
			moduleGroups.ForEach(x => x.Init(this));
		}

		void Update()
		{
			Process();
		}

		[HideInInspector]
		public event Action OnProcess;
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