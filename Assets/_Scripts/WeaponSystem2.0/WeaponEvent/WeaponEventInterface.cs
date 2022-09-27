using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Events;
using System.Linq;

namespace WeaponSystem
{
	public class WeaponEventInterface : MonoBehaviour, Weapon.IOwner, WeaponSystem.IProcessor
	{
		[SerializeField] private WeaponEventAsset m_eventAsset;


		// Incoming to weapon
		[SerializeField] private Weapon m_weapon;
		[SerializeField] private List<WeaponEvent> m_eventsHosted = new List<WeaponEvent>();

		// Weapon to outgoing

		public Weapon Weapon => m_weapon;
		GameObject Weapon.IOwner.ownerObject => this.gameObject;
		public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();


		void LoadAllEvents()
		{
			// Take all weaponeventreferences and create event objects without duplicates.
			m_eventsHosted =
				m_weapon
				.GetComponentsInChildren<WeaponAction>()
				.Select(x => x.actionEvent)
				.Where(x => x != null)
				.Distinct()
				.Select(x => WeaponEvent.Create(x))
				.ToList();
		}

		public void SetupWeapon(Weapon weapon)
		{
			m_weapon = weapon;
			m_eventAsset = m_weapon.weaponEventAsset;

			Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();

			LoadAllEvents();

			weapon.Setup(this);
		}

		public WeaponEvent FindEvent(string id)
		{
			return m_eventsHosted.Where(x => x.m_Id == id)?.FirstOrDefault();
		}
	}
}
