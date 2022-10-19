using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Events;
using System.Linq;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	public class WeaponEventInterface : MonoBehaviour, Weapon.IOwner, WeaponSystem.IProcessor
	{
		[SerializeField] private Weapon m_weapon;
		[SerializeField] private WeaponEventAsset m_eventAsset;

		[SerializeField] private List<WeaponEventReference> m_eventsNeeded = new List<WeaponEventReference>();
		public bool snd;

		public Weapon Weapon => m_weapon;

		GameObject Weapon.IOwner.ownerObject => this.gameObject;

		public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();

		public Dictionary<string, InputAction> inputActions => throw new System.NotImplementedException();

		void Start()
		{
			m_weapon = transform.GetComponentInChildren<Weapon>();
			m_eventAsset = m_weapon.weaponEventAsset;

			Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();

			m_weapon.Setup(this);

			m_eventsNeeded = GetComponentsInChildren<WeaponAction>().Select(x => x.actionEvent).Where(x => x != null).ToList();
		}

		void SendEvent()
		{
			var evnt = m_eventsNeeded.First().Event;
			evnt.incomingEvent.Invoke(new WeaponEvent.CallbackContext());
		}

		void OnValidate()
		{
			if (snd == true)
			{
				SendEvent();
				snd = false;
			}
		}
	}
}
