using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeaponSystem.Events
{
	public class BaseEventData { }

	public interface IMessageReciver
	{
		public void ProcessMessage<T>(T eventData) where T : BaseEventData;
	}

	public class Messanger : MonoBehaviour
	{
		public delegate void EventFunction<T1>(T1 handler, BaseEventData eventData);

		[SerializeField] private List<WeaponEventReference> m_eventsDiscovered = new List<WeaponEventReference>();
		private Dictionary<WeaponEventReference, System.Delegate> eventTable = new Dictionary<WeaponEventReference, System.Delegate>();

		void Start()
		{
			// Gets all weaponEventReferences the current weapon requires
			m_eventsDiscovered =
			GetComponentsInChildren<WeaponAction>()
			.Select(x => x.actionEvent)
			.Where(x => x != null)
			.ToList();
		}

		public void Subscribe(WeaponEventReference target, BaseEventData eventData, EventFunction<IMessageReciver> functor)
		{

		}

		public void Broadcast<T>(WeaponEventReference target) where T : BaseEventData
		{

		}
	}
}