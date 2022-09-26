using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem.Events
{
	[CreateAssetMenu(fileName = "EventContainer", menuName = "Weapon system/Event container", order = 1)]
	public class EventContainer : ScriptableObject
	{
		[HideInInspector]
		public List<WeaponEvent> weaponEvents = new List<WeaponEvent>();
	}
}