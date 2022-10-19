using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

namespace WeaponSystem.Events
{
	public class WeaponEventReference : ScriptableObject
	{
		[SerializeField, ReadOnly] internal WeaponEventAsset m_Asset;
		[SerializeField, ReadOnly] internal string m_EventId;
		[NonSerialized] private WeaponEvent m_Event;

		public WeaponEventAsset Asset => m_Asset;
		public WeaponEvent Event
		{
			get
			{
				if (m_Event == null)
				{
					if (m_Asset == null)
						return null;

					m_Event = m_Asset.FindEvent(new Guid(m_EventId));
				}

				return m_Event;
			}
		}

		public void Set(WeaponEvent wepEvent)
		{

			var asset = wepEvent.m_asset;
			SetInternal(wepEvent, asset);
		}

		private void SetInternal(WeaponEvent wepEvent, WeaponEventAsset asset)
		{
			m_Asset = asset;
			m_EventId = wepEvent.Id.ToString();
			name = GetDisplayName(Event);
		}

		private static string GetDisplayName(WeaponEvent wepEvent)
		{
			return wepEvent?.name;
		}

		public static WeaponEventReference Create(WeaponEvent wepEvent)
		{
			if (wepEvent == null)
				return null;

			var reference = CreateInstance<WeaponEventReference>();
			reference.Set(wepEvent);
			return reference;
		}
	}
}