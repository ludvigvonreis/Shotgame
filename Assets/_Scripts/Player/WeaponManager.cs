using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;
using UnityEngine.Events;

[System.Serializable]
public class WeaponEquipEvent
{
	public string uuid;
	public bool removed;

	public WeaponEquipEvent(string _uuid, bool _removed)
	{
		uuid = _uuid;
		removed = _removed;
	}
}

public class WeaponManager : MonoBehaviour, Weapon.IOwner, WeaponAction.IProcessor, WeaponState.IUpdator
{
	// Processor interface
	GameObject Weapon.IOwner.ownerObject => this.gameObject;

	public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();
	public Dictionary<string, InputAction> inputActions => _inputActions;
	Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();

	private Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
	private string currentWeaponUUID;

	public UnityEvent<WeaponEquipEvent> m_onEquip;

	// Weapon events
	public UnityEvent<AmmoChangeEvent> m_ammoChange => _m_ammoChange;
	public UnityEvent<AmmoChangeEvent> _m_ammoChange;

	void Awake()
	{
		_m_ammoChange = new UnityEvent<AmmoChangeEvent>();
	}

	// Setup by player
	public void Setup(Player reference)
	{
		var playerInput = reference.playerInput;
		playerInput.actions.ToList().ForEach(action => _inputActions.Add(action.name, action));

		Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();

		weapons.Values.ToList().ForEach(weapon => SetupWeapon(weapon));
	}

	void SetupWeapon(Weapon weapon)
	{
		weapon.Setup(this);
	}

	public void EquipWeapon(Weapon weapon)
	{
		var uuid = System.Guid.NewGuid().ToString();
		weapons.Add(uuid, weapon);

		// Signal on equip event, false means equipped. Used by weapon holder
		m_onEquip.Invoke(new WeaponEquipEvent(uuid, false));

		SetupWeapon(weapon);

		currentWeaponUUID = uuid;
	}

	void DropWeapon(string uuid)
	{
		var weapon = weapons[uuid];
		weapon.Reset();
		// Signal on equip event, true means removed.
		m_onEquip.Invoke(new WeaponEquipEvent(uuid, true));

		weapons.Remove(uuid);
	}

	public void DropCurrent()
	{
		DropWeapon(currentWeaponUUID);
	}


	public Weapon GetWeaponByUUID(string uuid)
	{
		return weapons[uuid];
	}
}
