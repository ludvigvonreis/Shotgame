using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	public interface IRaycastMessages : IEventSystemHandler
	{
		public void OnShoot();
		public void OnTimeout();
	}

	[System.Serializable]
	public class WeaponRaycast : WeaponAction
	{

		[SerializeField]
		Transform point;

		WeaponStats weaponStats;
		WeaponState weaponState;

		GameObject ownerObject;

		/*
		[SerializeField] GameObject trail;
		TrailRenderer trailRenderer;
		Rigidbody trailBody;
		*/

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

			/*
			trailRenderer = trail.GetComponent<TrailRenderer>();
			trailBody = trail.GetComponent<Rigidbody>();
			*/
		}

		protected override void ProcessInput(WeaponEvent.CallbackContext context)
		{
			Debug.Log("[WeaponRaycast] I processed a input");

			/*if (groupReference.isRunning == false) return;

			performed = context.performed;

			if (context.performed)
			{
				isNotHeld = false;
			}

			if (context.canceled)
			{
				isNotHeld = true;
			}*/
		}

		private void GroupProcess()
		{
			if (hasTimedOut)
			{
				weaponState.DecreaseHeat();

				if (!sentTimoutEvent)
				{
					MessagingUtil.ExecuteRecursive<IRaycastMessages>(transform.root.gameObject, (x, y) => x.OnTimeout());
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

				/*
				trail.transform.position = point.transform.position;
				trailBody.AddForce(point.transform.forward * 10, ForceMode.Impulse);
				*/

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
				MessagingUtil.ExecuteRecursive<IRaycastMessages>(transform.root.gameObject, (x, y) => x.OnShoot());
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