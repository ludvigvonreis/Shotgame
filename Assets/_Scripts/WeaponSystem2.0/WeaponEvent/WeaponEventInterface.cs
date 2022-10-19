using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Events;
using System.Linq;
using UnityEngine.InputSystem;
using System;

namespace WeaponSystem
{
	public class WeaponEventInterface : MonoBehaviour, Weapon.IOwner, WeaponSystem.IProcessor
	{
		[SerializeField] private Weapon m_weapon;
		[SerializeField] private WeaponEventAsset m_eventAsset;

		[SerializeField] private List<WeaponEventReference> m_eventsNeeded = new List<WeaponEventReference>();
		[SerializeField] private List<WeaponEvent> m_eventsHosted = new List<WeaponEvent>();
		public bool snd;

		public Weapon Weapon => m_weapon;

		GameObject Weapon.IOwner.ownerObject => this.gameObject;

		public List<Weapon.IProcessor> Processors { get; protected set; } = new List<Weapon.IProcessor>();

		void Start()
		{
			m_weapon = transform.GetComponentInChildren<Weapon>();
			m_eventAsset = m_weapon.weaponEventAsset;

			Processors = GetComponentsInChildren<Weapon.IProcessor>(true).ToList();

			m_eventsNeeded =
			GetComponentsInChildren<WeaponAction>()
			.Select(x => x.actionEvent)
			.Where(x => x != null)
			.ToList();

			m_eventsHosted = m_eventsNeeded
			.Select(x => WeaponEvent.Create(x)).ToList();

			m_weapon.Setup(this);
		}

		public class TestString : EventArgs
		{
			public string value;

			public TestString(string v)
			{
				value = v;
			}
		}

		void SendEvent()
		{
			var evnt = m_eventsHosted.First();

			// Invoke as function trigger
			evnt.Invoke();

			// Invoke with data
			evnt.Invoke<TestString>(new TestString("123"));
		}

		void OnValidate()
		{
			if (snd == true)
			{
				SendEvent();
				snd = false;
			}
		}

		public WeaponEvent FindEvent(string id)
		{
			return m_eventsHosted.Where(x => x.m_Id == id)?.First();
		}
	}
}
