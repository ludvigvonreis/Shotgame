using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using WeaponSystem;
using WeaponSystem.Events;

namespace Gnome
{
	[System.Serializable]
	public class InputActionToWeaponEvent
	{
		public InputActionReference actionReference;
		public WeaponEventReference eventReference;
	}

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
	public class WeaponManager : MonoBehaviour, IStateUpdate
	{
		///// TODO: Add multiple weapons and switching

		[SerializeField] WeaponEventInterface weaponEventInterface;

		Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();

		[SerializeField] List<InputActionToWeaponEvent> weaponBindings = new List<InputActionToWeaponEvent>();

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


			m_ammoChange?.Invoke(new Gnome.UI.AmmoChangeEvent(weaponState.CurrentAmmo, weaponState.CurrentAmmoReserve));
		}

		// Setup by player
		public void Setup(Player reference)
		{
			var playerInput = reference.playerInput;
			//_inputActions = playerInput.actions.ToDictionary(x => x.name, x => x);

			//weapons.Values.ToList().ForEach(weapon => SetupWeapon(weapon));
		}

		void BindWeapon()
		{
			foreach (var binding in weaponBindings)
			{
				binding.actionReference.action.performed += (a) =>
				{
					weaponEventInterface.FindEvent(binding.eventReference.Id)?.Invoke(this, a);
				};

				binding.actionReference.action.canceled += (a) =>
				{
					weaponEventInterface.FindEvent(binding.eventReference.Id)?.Invoke(this, a);
				};
			}
		}

		public void EquipWeapon(Weapon weapon)
		{
			var uuid = System.Guid.NewGuid().ToString();
			weapons.Add(uuid, weapon);

			// Signal on equip event, false means equipped. Used by weapon holder
			m_onEquip.Invoke(new WeaponEquipEvent(uuid, false));

			weaponEventInterface.SetupWeapon(weapon);

			currentWeaponUUID = uuid;

			BindWeapon();
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

