using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace WeaponSystem
{
	[System.Serializable]
	public class ActionHandler
	{
		[Range(0.1f, 5.0f)]
		public float releaseTimeout;

		[HideInInspector] public UnityEvent<bool> m_performed;
		[HideInInspector] public UnityEvent<bool> m_canceled;

		[HideInInspector] public UnityEvent m_ReleasedThisFrame;
		[HideInInspector] public bool isActionHeld;

		bool timeoutSent;
		[HideInInspector] public UnityEvent<bool> m_releaseTimeout;

		public ActionHandler(InputAction action)
		{
			m_performed = new UnityEvent<bool>();
			m_canceled = new UnityEvent<bool>();
			m_ReleasedThisFrame = new UnityEvent();
			m_releaseTimeout = new UnityEvent<bool>();

			releaseTimeout = 0.3f;

			action.performed += OnAction;
			action.canceled += OnAction;
		}

		void OnAction(InputAction.CallbackContext context)
		{
			Debug.LogFormat("Got an input thing, performed={0}, canceled={1}",
							context.performed, context.canceled);

			m_performed.Invoke(context.performed);
			m_canceled.Invoke(context.canceled);

			if (context.performed)
			{
				isActionHeld = true;
				timeoutSent = false;
			}

			if (context.canceled)
			{
				m_ReleasedThisFrame.Invoke();
				isActionHeld = false;
			}
		}

		public IEnumerator ReleaseTimeout()
		{
			while (true)
			{
				yield return new WaitUntilForSeconds(() => isActionHeld, releaseTimeout,
								(float t) => { m_releaseTimeout.Invoke(false); });

				if (!timeoutSent)
				{
					timeoutSent = true;
					m_releaseTimeout.Invoke(true);
				}
			}
		}
	}

	public class WeaponInput : MonoBehaviour
	{
		public ActionHandler attack;
		public ActionHandler aim;
		public ActionHandler reload;

		public InputActionReference attackButton;
		public InputActionReference aimButton;
		public InputActionReference reloadButton;

		// FIXME: TEMPORARY
		public PlayerInput playerInput;

		void Start()
		{
			playerInput = FindObjectOfType<PlayerInput>();
			var action = playerInput.actions[attackButton.action.name];
			attack = new ActionHandler(action);

			StartCoroutine(attack.ReleaseTimeout());
		}
	}
}