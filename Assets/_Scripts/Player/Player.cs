using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Gnome
{
	[System.Serializable]
	public class HealthChange
	{
		public float health;
		public float maxHealth;

		public HealthChange(float _h, float _mh)
		{
			health = _h;
			maxHealth = _mh;
		}
	}

	[System.Serializable]
	public class HealthChangeEvent : UnityEvent<HealthChange> { }


	[RequireComponent(typeof(WeaponManager))]
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(PlayerInteract))]
	public class Player : MonoBehaviour
	{
		public Camera playerCam;
		public Camera weaponCam;
		public PlayerInput playerInput;

		[SerializeField]
		public WeaponManager weaponManager;

		[Header("Player stats")]
		public float health;
		public float maxHealth;

		void OnValidate()
		{
			playerInput = GetComponent<PlayerInput>();
		}

		void Start()
		{
			health = maxHealth;

			weaponManager.Setup(this);
		}

		// TODO: Refactor this in a better way
		/*public void DealDamage(float damage)
		{
			health = Mathf.Max(health - damage, 0);

			m_HealthChange.Invoke(new HealthChange(health, maxHealth));
		}*/
	}
}
