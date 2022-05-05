using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	public class WeaponAction : Weapon.Module
	{
		public IProcessor Processor { get; protected set; }
		public interface IProcessor : Weapon.IProcessor
		{
			Dictionary<string, InputAction> inputActions { get; }
		}

		public WeaponConstraint Constraint => weaponReference.Constraint;

		public override void Init()
		{
			base.Init();

			Processor = weaponReference.GetProcessor<IProcessor>();

			weaponReference.OnProcess.AddListener(Process);

			Processor.inputActions[actionButton.action.name].canceled += ProcessInput;
			Processor.inputActions[actionButton.action.name].performed += ProcessInput;
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

		[SerializeField]
		InputActionReference actionButton;
		[HideInInspector]
		public InputAction.CallbackContext inputContext;

		void ProcessInput(InputAction.CallbackContext context)
		{
			inputContext = context;
		}
	}
}