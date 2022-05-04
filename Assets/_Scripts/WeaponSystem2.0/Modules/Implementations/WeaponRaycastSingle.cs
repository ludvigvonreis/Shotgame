using UnityEngine;

namespace WeaponSystem
{
	public class WeaponRaycastSingle : WeaponAction
	{
		public override void Init()
		{
			base.Init();
			actionHandler.m_performed.AddListener(Perform);
		}

		void Perform(bool isPerformed)
		{
			if (isPerformed)
			{
				Debug.Log("hello");
			}
		}
	}
}