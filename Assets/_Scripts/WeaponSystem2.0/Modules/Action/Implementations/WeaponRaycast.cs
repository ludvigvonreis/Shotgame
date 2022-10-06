using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponRaycast : WeaponAction
	{

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

			groupReference.Action.OnPerfom += Action;
			groupReference.OnGroupProcess += GroupProcess;
			weaponStats = groupReference.weaponStats;
			weaponState = groupReference.weaponState;

			ownerObject = groupReference.owner.ownerObject;

			StartCoroutine(ShootTimeoutLoop());
		}

		protected override void ProcessInput(object sender, WeaponEvent.ActionContext context)
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
					MessagingUtil.ExecuteRecursive<IWeaponShootEvents>(transform.root.gameObject, (x, y) => x.OnTimeout());
					sentTimoutEvent = true;
				}
			}
		}

		// TODO: Implement rest of shooting function
		void Action()
		{
			if (performed)
			{
				weaponState.CurrentAmmo -= 1;

				weaponState.CancelDecrease();
				sentTimoutEvent = false;

				weaponState.IncreaseHeat();


				// Shoot from position
				RaycastHit hit;
				if (Physics.Raycast(
					point.transform.position, point.transform.forward, out hit, weaponStats.range
				))
				{
					DecalManager.Instance.PlaceDecal(hit.point, Quaternion.identity);
				}

				// Signal event for shooting.
				ExecuteEvents.ExecuteHierarchy<IWeaponShootEvents>(gameObject, null, (x, y) => x.OnShoot());
			}
		}

		IEnumerator ShootTimeoutLoop()
		{
			while (true)
			{
				yield return new WaitUntilForSeconds(
					() => isNotHeld, weaponStats.shootTimeoutTime, (_) => { hasTimedOut = false; }
				);
				hasTimedOut = true;

				// needed??
				yield return null;
			}
		}
	}
}