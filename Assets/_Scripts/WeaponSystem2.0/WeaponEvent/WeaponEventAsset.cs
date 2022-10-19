using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeaponSystem.Events
{
	[CreateAssetMenu(fileName = "Event asset", menuName = "Weapon system/Event asset", order = 1)]
	public class WeaponEventAsset : ScriptableObject
	{
		[HideInInspector]
		public List<WeaponEvent> weaponEvents = new List<WeaponEvent>();

		[HideInInspector]
		public List<WeaponEventReference> weaponEventReferences = new List<WeaponEventReference>();


		public WeaponEvent FindEvent(Guid eventId)
		{
			if (weaponEvents.Count <= 0) return null;

			return weaponEvents.Where(x => x.Id == eventId).First();
		}

		public void RemoveEvent(WeaponEvent @event)
		{
			if (weaponEvents.Count <= 0) return;
			if (weaponEventReferences.Count <= 0) return;

			var referenceIdx = weaponEventReferences.FindIndex(x => x.m_EventId == @event.Id.ToString());
			weaponEventReferences.RemoveAt(referenceIdx);
			weaponEvents.Remove(@event);
		}

		public void RenameEvent(WeaponEvent @event, string name)
		{
			if (weaponEvents.Count < 0) return;
			if (weaponEventReferences.Count < 0) return;

			var referenceIdx = weaponEventReferences.FindIndex(x => x.m_EventId == @event.Id.ToString());

			weaponEventReferences[referenceIdx].name = name;
			@event.m_Name = name;
		}
	}
}