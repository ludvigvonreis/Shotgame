using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

namespace WeaponSystem.Events
{
	[Serializable]
	public class WeaponEvent
	{
		[SerializeField] internal string m_Id;
		[SerializeField] internal string m_Name;
		[SerializeField] internal WeaponEventAsset m_asset;

		public string name => m_Name;

		public Guid Id
		{
			get
			{
				MakeSureIdIsInPlace();
				return new Guid(m_Id);
			}
		}
		public IncomingBase<string> incomingEvent;
		public OutgoingBase<string> outgoingEvent;

		public WeaponEvent()
		{ }

		public WeaponEvent(string name = null)
		{
			m_Name = name;
		}

		internal string MakeSureIdIsInPlace()
		{
			if (string.IsNullOrEmpty(m_Id))
				GenerateId();
			return m_Id;
		}

		internal void GenerateId()
		{
			m_Id = Guid.NewGuid().ToString();
		}
	}
}