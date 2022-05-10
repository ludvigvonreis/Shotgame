using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
	[System.Serializable]
	public class WeaponRaycast : WeaponAction
	{
		public IShootProcessor ShootProcessor { get; protected set; }
		public interface IShootProcessor : Weapon.IProcessor
		{
			public UnityEvent m_OnShoot { get; }
			public UnityEvent m_OnTimeout { get; }
		}

		[SerializeField]
		Transform point;

		WeaponStats weaponStats;
		WeaponState weaponState;

		GameObject ownerObject;

		bool performed;
		bool isNotHeld;

		bool hasTimedOut;
		private bool sentTimoutEvent;

		public override void Init()
		{
			base.Init();

			ShootProcessor = groupReference.GetProcessor<IShootProcessor>();

			groupReference.Action.OnPerfom += Action;
			groupReference.OnGroupProcess += GroupProcess;
			weaponStats = groupReference.weaponStats;
			weaponState = groupReference.weaponState;

			ownerObject = groupReference.owner.ownerObject;

			StartCoroutine(ShootTimeoutLoop());
		}

		protected override void ProcessInput(InputAction.CallbackContext context)
		{
			if (groupReference.isRunning == false) return;

			performed = context.performed;

			if (context.performed)
			{
				isNotHeld = false;
			}

			if (context.canceled)
			{
				isNotHeld = true;
			}
		}

		private void GroupProcess()
		{
			if (hasTimedOut)
			{
				weaponState.DecreaseHeat();

				if (!sentTimoutEvent)
				{
					ShootProcessor.m_OnTimeout.Invoke();
					sentTimoutEvent = true;
				}
			}
		}

		// TODO: Implement rest of shooting function
		void Action()
		{
			if (performed)
			{
				weaponState.currentAmmo -= 1;

				weaponState.CancelDecrease();
				sentTimoutEvent = false;

				weaponState.IncreaseHeat();

				// Shoot from position
				RaycastHit hit;
				if (Physics.Raycast(
					point.transform.position, point.transform.forward, out hit, weaponStats.range
				))
				{
					//EventManager.Instance.m_HitEvent.Invoke(new Hit(temp, hit, weaponStats));
					DecalManager.Instance.PlaceDecal(hit.point, Quaternion.identity);
				}

				// Signal event for shooting.
				ShootProcessor.m_OnShoot.Invoke();
			}
		}

		IEnumerator ShootTimeoutLoop()
		{
			while (true)
			{
				yield return new WaitUntilForSeconds(() => isNotHeld, weaponStats.shootTimeoutTime, (_) => { hasTimedOut = false; });
				hasTimedOut = true;

				// needed??
				yield return null;
			}
		}
	}
}