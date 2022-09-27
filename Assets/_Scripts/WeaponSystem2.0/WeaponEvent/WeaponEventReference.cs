using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

namespace WeaponSystem.Events
{
	public class WeaponEventReference : ScriptableObject
	{
		[SerializeField, ReadOnly] internal string m_name;
		[SerializeField, ReadOnly] internal string m_id;
		[SerializeField, ReadOnly] internal WeaponEventAsset m_Asset;


		public WeaponEventAsset Asset => m_Asset;
		public string Id
		{
			get
			{
				MakeSureIdIsInPlace();
				return m_id;
			}
		}

		public void Set(WeaponEventAsset asset, string _name = null)
		{
			m_Asset = asset;
			m_name = _name;

			name = _name;

			MakeSureIdIsInPlace();
		}

		public static WeaponEventReference Create(WeaponEventAsset asset, string name = null)
		{
			var reference = CreateInstance<WeaponEventReference>();
			reference.Set(asset, name);
			return reference;
		}

		internal string MakeSureIdIsInPlace()
		{
			if (string.IsNullOrEmpty(m_id))
				GenerateId();
			return m_id;
		}

		internal void GenerateId()
		{
			m_id = Guid.NewGuid().ToString();
		}
	}
}