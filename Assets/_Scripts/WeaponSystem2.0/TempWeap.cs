using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using WeaponSystem;

[System.Obsolete("TempWeap is obsolete, use weaponManager instead")]
public class TempWeap : MonoBehaviour, Weapon.IOwner, WeaponAction.IProcessor
{
	[SerializeField]
	Weapon weapon;

	public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();
	GameObject Weapon.IOwner.ownerObject => this.gameObject;

	//public bool performed => _performed;
	//public bool canceled => _canceled;

	public Dictionary<string, InputAction> inputActions => _inputActions;

	//bool _performed = false;
	//bool _canceled = false;
	Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();

	[HideInInspector] public UnityEvent<RecoilData> m_ShootEvent = new UnityEvent<RecoilData>();
	[HideInInspector] public UnityEvent m_ResetRecoil = new UnityEvent();


	void Awake()
	{
		var playerInput = GetComponent<Player>().playerInput;
		playerInput.actions.ToList().ForEach(x => AddInputAction(x));
	}

	void Start()
	{
		Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();
		weapon.Setup(this);
	}

	void AddInputAction(InputAction action)
	{
		_inputActions.Add(action.name, action);
	}
}
