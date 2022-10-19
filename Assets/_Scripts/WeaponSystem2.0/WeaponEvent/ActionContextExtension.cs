using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem.Events
{
	public static class ActionContextExtension
	{
		/// <summary>
		/// Create actionContext from input callbackcontext
		/// </summary>
		public static WeaponEvent.ActionContext FromInputContext(this WeaponEvent.ActionContext context, InputAction.CallbackContext inputContext)
		{
			var newCtx = new WeaponEvent.ActionContext
			{
				canceled = inputContext.canceled,
				performed = inputContext.performed,
				time = inputContext.time
			};

			return newCtx;
		}
	}
}