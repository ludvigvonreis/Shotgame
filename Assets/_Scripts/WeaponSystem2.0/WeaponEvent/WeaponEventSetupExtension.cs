using UnityEngine;

namespace WeaponSystem.Events
{
	public static class WeaponEventSetupExtensions
	{
		public static WeaponEvent NewEvent(this WeaponEventAsset asset, string name = null)
		{
			var newEvent = new WeaponEvent();
			newEvent.GenerateId();
			newEvent.m_asset = asset;

			asset.weaponEvents.Add(newEvent);

			return newEvent;
		}
	}
}