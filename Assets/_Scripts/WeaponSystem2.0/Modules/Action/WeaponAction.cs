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
		public WeaponEventReference actionEvent;

		public override void Init()
		{
			base.Init();

			Processor = groupReference.GetProcessor<IProcessor>();

			groupReference.OnGroupProcess += Process;

			if (actionEvent != null)
			{
				var evnt = Processor.FindEvent(actionEvent.Id);
				if (evnt != null) evnt.action += ProcessInput;
			}

			//Processor.inputActions[actionButton.action.name].performed += ProcessInput;
			//Processor.inputActions[actionButton.action.name].canceled += ProcessInput;
		}

		void Process()
		{
			if (Constraint.Active) return;

			Perform();
		}

		[HideInInspector]
		public event Action OnPerfom;
		void Perform()
		{
			OnPerfom?.Invoke();
		}

		protected virtual void ProcessInput(object sender, WeaponEvent.ActionContext context)
		{
			if (groupReference.isRunning == false) return;
		}

		/*
		protected virtual void ProcessInput(InputAction.CallbackContext context)
		{
			if (groupReference.isRunning == false) return;
		}
		*/
	}
}