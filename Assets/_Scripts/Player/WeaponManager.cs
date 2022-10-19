using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

using WeaponSystem;

namespace Gnome
{
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

	[RequireComponent(typeof(PlayerRecoil))]
	[RequireComponent(typeof(WeaponHolder))]
	public class WeaponManager : MonoBehaviour, Weapon.IOwner, WeaponSystem.IProcessor, IStateUpdate
	{
		///// TODO: Add multiple weapons and switching


		// Processor interface
		GameObject Weapon.IOwner.ownerObject => this.gameObject;

		public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();
		public Dictionary<string, InputAction> inputActions => _inputActions;
		Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();

		// Weapon events
		public UnityEvent m_stateChange => m_stateChangeEvent;
		private UnityEvent m_stateChangeEvent;

		// UI events
		[SerializeField]
		private UnityEvent<Gnome.UI.AmmoChangeEvent> m_ammoChange;

		// Manager stuff
		private Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
		private string currentWeaponUUID;
		public UnityEvent<WeaponEquipEvent> m_onEquip;

		void Awake()
		{
			m_stateChangeEvent = new UnityEvent();
			m_stateChangeEvent.AddListener(StateChangeHandler);
		}

		void StateChangeHandler()
		{
			var weaponState = GetCurrentWeapon()?.weaponState;
			if (weaponState == null) return;


			m_ammoChange?.Invoke(new Gnome.UI.AmmoChangeEvent(weaponState.currentAmmo, weaponState.ammoReserve));
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
			currentWeaponUUID = null;
		}

		public void DropCurrent()
		{
			DropWeapon(currentWeaponUUID);
		}


		public Weapon GetWeaponByUUID(string uuid)
		{
			return weapons[uuid];
		}

		public Weapon GetCurrentWeapon()
		{
			if (currentWeaponUUID == null) return null;
			if (!weapons.ContainsKey(currentWeaponUUID)) return null;

			return weapons[currentWeaponUUID];
		}
	}
}

