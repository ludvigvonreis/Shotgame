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
		public List<WeaponEventReference> weaponEventReferences = new List<WeaponEventReference>();


		public WeaponEventReference FindEvent(string eventId)
		{
			if (weaponEventReferences.Count <= 0) return null;

			return weaponEventReferences.Where(x => x.Id == eventId).First();
		}

		public void RenameEvent(WeaponEventReference eventReference, string newName)
		{
			if (weaponEventReferences.Count < 0) return;

			eventReference.m_name = newName;
			eventReference.name = newName;
		}
	}
}