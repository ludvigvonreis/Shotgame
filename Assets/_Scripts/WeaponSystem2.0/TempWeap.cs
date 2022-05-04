using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using WeaponSystem;

public class TempWeap : MonoBehaviour, Weapon.IOwner, WeaponAction.IProcessor
{
	[SerializeField]
	private InputActionReference attackButton;

	[SerializeField]
	Weapon weapon;

	public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();
	GameObject Weapon.IOwner.ownerObject => this.gameObject;

	public bool performed => _performed;
	public bool canceled => _canceled;

	bool _performed = false;
	bool _canceled = false;

	[HideInInspector] public UnityEvent<RecoilData> m_ShootEvent = new UnityEvent<RecoilData>();
	[HideInInspector] public UnityEvent m_ResetRecoil = new UnityEvent();


	void Start()
	{
		Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();
		weapon.Setup(this);

		var playerInput = GetComponent<PlayerInput>();
		var act = playerInput.actions[attackButton.action.name];
		act.canceled += InputHandler;
		act.performed += InputHandler;
	}

	void InputHandler(InputAction.CallbackContext context)
	{
		_performed = context.performed;
		_canceled = context.canceled;
	}
}
