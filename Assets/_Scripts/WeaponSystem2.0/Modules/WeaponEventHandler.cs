using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
	public class WeaponEventHandler : MonoBehaviour
	{
		public UnityEvent m_primaryAction;
		public UnityEvent m_secondaryAction;
		public UnityEvent m_reloadAction;


		void Start()
		{
			m_primaryAction.Invoke();
		}
	}
}