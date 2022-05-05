using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

namespace WeaponSystem
{
	public class WeaponAction : Weapon.Module
	{
		public IProcessor Processor { get; protected set; }
		public interface IProcessor : Weapon.IProcessor
		{
			Dictionary<string, InputAction> inputActions { get; }
		}

		public WeaponConstraint Constraint => groupReference.Constraint;

		public override void Init()
		{
			base.Init();

			Processor = groupReference.GetProcessor<IProcessor>();

			groupReference.OnGroupProcess += Process;

			Processor.inputActions[actionButton.action.name].performed += ProcessInput;
			Processor.inputActions[actionButton.action.name].canceled += ProcessInput;
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

		public InputActionReference actionButton;
		[HideInInspector]
		public InputAction.CallbackContext inputContext;
		protected virtual void ProcessInput(InputAction.CallbackContext context)
		{
			inputContext = context;
		}
	}
}