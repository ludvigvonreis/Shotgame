using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem.Events;

namespace WeaponSystem
{
	// public interface IProcessor : Weapon.IProcessor
	// {
	// 	Dictionary<string, InputAction> inputActions { get; }
	// }

	public interface IProcessor : Weapon.IProcessor
	{
		public WeaponEvent FindEvent(string id);
	}

	[System.Serializable]
	public class WeaponAction : Weapon.Module
	{
		public IProcessor Processor { get; protected set; }

		public WeaponConstraint Constraint => groupReference.Constraint;

		public override void Init()
		{
			base.Init();

			Processor = groupReference.GetProcessor<IProcessor>();

			groupReference.OnGroupProcess += Process;

			//Processor.inputActions[actionButton.action.name].performed += ProcessInput;
			//Processor.inputActions[actionButton.action.name].canceled += ProcessInput;
		}

		void Process()
		{
			//Debug.LogFormat("Contstraints active?: {0}, {1}", this.gameObject.name, Constraint.Active);

			if (Constraint.Active) return;

			Perform();
		}

		[HideInInspector]
		public event Action OnPerfom;
		void Perform()
		{
			OnPerfom?.Invoke();
		}

		public WeaponEventReference actionEvent;
		/*protected virtual void ProcessInput<T>(T data)
		{
			if (groupReference.isRunning == false) return;
		}*/

		/*
		protected virtual void ProcessInput(InputAction.CallbackContext context)
		{
			if (groupReference.isRunning == false) return;
		}
		*/
	}
}