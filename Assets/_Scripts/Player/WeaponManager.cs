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

public class WeaponManager : MonoBehaviour, Weapon.IOwner, WeaponAction.IProcessor
{
	// Processor interface
	GameObject Weapon.IOwner.ownerObject => this.gameObject;

	public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();
	public Dictionary<string, InputAction> inputActions => _inputActions;
	Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();

	private Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
	private string currentWeaponUUID;

	public UnityEvent<WeaponEquipEvent> m_onEquip;

	public Weapon testWeapon;
	public InputActionReference throwButton;

	// Setup by player
	public void Setup(Player reference)
	{
		var playerInput = reference.playerInput;
		playerInput.actions.ToList().ForEach(action => _inputActions.Add(action.name, action));

		Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();

		weapons.Values.ToList().ForEach(weapon => SetupWeapon(weapon));

		EquipWeapon(testWeapon);

		//player.playerInput.actions[throwButton.name].performed += DropWeapon;
	}

	void SetupWeapon(Weapon weapon)
	{
		weapon.Setup(this);
	}

	void EquipWeapon(Weapon weapon)
	{
		var uuid = System.Guid.NewGuid().ToString();
		weapons.Add(uuid, weapon);

		// Signal on equip event, false means equipped. Used by weapon holder
		m_onEquip.Invoke(new WeaponEquipEvent(uuid, false));

		SetupWeapon(weapon);
	}

	void DropWeapon(string uuid)
	{
		var weapon = weapons[uuid];
		weapon.Reset();
		// Signal on equip event, true means removed.
		m_onEquip.Invoke(new WeaponEquipEvent(uuid, true));

		weapons.Remove(uuid);
	}


	public Weapon GetWeaponByUUID(string uuid)
	{
		return weapons[uuid];
	}
}
