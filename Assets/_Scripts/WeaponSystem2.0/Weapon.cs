using UnityEngine;
using System.Linq;
using System;

namespace WeaponSystem
{
	public class Weapon : MonoBehaviour
	{
		public IOwner Owner { get; protected set; }
		public interface IOwner { }

		public interface IBehaviour
		{
			//Like Awake
			void Configure(ActionHandler actionHandler);

			//Like Start
			void Init();
		}

		public IBehaviour[] Behaviours { get; protected set; }
		public abstract class Behaviour : MonoBehaviour, IBehaviour
		{
			public virtual void Configure(ActionHandler actionHandler) { }

			public virtual void Init() { }
		}

		public IModule[] Modules { get; protected set; }
		public interface IModule
		{
			void Set(Weapon reference);
		}

		public abstract class Module : Behaviour, IModule
		{
			public Weapon Weapon { get; protected set; }
			public virtual void Set(Weapon reference)
			{
				Weapon = reference;
			}
		}

		public WeaponAction Action { get; protected set; }

		public void Setup(IOwner reference)
		{
			Owner = reference;

			Behaviours = GetComponentsInChildren<IBehaviour>(true);
			Modules = GetComponentsInChildren<IModule>(true);

			Action = Modules.First(x => x is WeaponAction) as WeaponAction;

			Array.ForEach(Modules, x => x.Set(this));

			var wepInp = GetComponent<WeaponInput>().attack;

			Array.ForEach(Behaviours, x => x.Configure(wepInp));
			Array.ForEach(Behaviours, x => x.Init());
		}
	}
}