using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

namespace WeaponSystem.Events
{
	[Serializable]
	public class WeaponEvent
	{
		[SerializeField, ReadOnly] internal string m_Id;
		[SerializeField, ReadOnly] internal string m_Name;
		//[SerializeField] internal WeaponEventAsset m_asset;

		public string name => m_Name;

		public event EventHandler action;

		public static WeaponEvent Create(WeaponEventReference reference)
		{
			var weaponEvent = new WeaponEvent();
			weaponEvent.m_Id = reference.Id;
			weaponEvent.m_Name = reference.name;

			return weaponEvent;
		}

		public void Invoke()
		{
			Debug.LogFormat("[{0}] Invoking as trigger", name);
			action.Invoke(this, EventArgs.Empty);
		}

		public void Invoke<T>(T eventData) where T : EventArgs
		{
			Debug.LogFormat("[{0}] Invoking with data: {1}", name, eventData);

			action.Invoke(this, eventData);
		}
	}
}