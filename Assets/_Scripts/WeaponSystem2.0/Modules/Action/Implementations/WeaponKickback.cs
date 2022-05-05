using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponKickback : WeaponAction
	{
		[SerializeField] private Transform kickbackHolder;
		[SerializeField] public float kickbackForce;
		[SerializeField] public float resetSmoothing;

		private bool isShooting;
		private bool hasReset;

		public override void Init()
		{
			base.Init();

			//groupReference.OnGroupProcess += Action;
			groupReference.Action.OnPerfom += Action;
			Processor.inputActions[actionButton.action.name].performed += ProcessInput;
			Processor.inputActions[actionButton.action.name].canceled += ProcessInput;
		}

		protected override void ProcessInput(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				isShooting = true;
			}

			if (context.canceled)
			{
				isShooting = false;
			}
		}

		void OnProcess()
		{
			if (isShooting) return;
			if (hasReset) return;

			if (Vector3.Distance(kickbackHolder.localPosition, Vector3.zero) >= 0.1f)
				kickbackHolder.localPosition = Vector3.Lerp(kickbackHolder.localPosition,
							Vector3.zero, Time.deltaTime * resetSmoothing);
			else

				hasReset = true;
		}

		void Action()
		{
			if (isShooting)
			{
				hasReset = false;
				kickbackHolder.localPosition -= new Vector3(0, 0, kickbackForce);
			}
		}
	}
}