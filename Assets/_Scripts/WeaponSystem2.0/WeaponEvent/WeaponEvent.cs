using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace WeaponSystem.Events
{
	[Serializable]
	public class WeaponEvent
	{
		[SerializeField, ReadOnly] internal string m_Id;
		[SerializeField, ReadOnly] internal string m_Name;
		//[SerializeField] internal WeaponEventAsset m_asset;

		public string name => m_Name;

		public event Action<object, ActionContext> action;

		public static WeaponEvent Create(WeaponEventReference reference)
		{
			var weaponEvent = new WeaponEvent();
			weaponEvent.m_Id = reference.Id;
			weaponEvent.m_Name = reference.name;

			return weaponEvent;
		}

		public void Invoke(object invoker, ActionContext context)
		{
			Debug.LogFormat("[{0}] {1} invoked event.", name, invoker.ToString());
			action.Invoke(invoker, context);
		}

		public void Invoke(object invoker, InputAction.CallbackContext context)
		{
			action.Invoke(invoker, ActionContext.FromInputContext(context));
		}

		public class ActionContext
		{
			public double time;

			public bool performed;
			public bool canceled;

			public ActionContext(bool _performed = false, bool _canceled = false)
			{
				time = Time.realtimeSinceStartupAsDouble;
				performed = _performed;
				canceled = _canceled;
			}

			/// <summary>
			/// Create actionContext from input callbackcontext
			/// </summary>
			public static ActionContext FromInputContext(InputAction.CallbackContext inputContext)
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
}