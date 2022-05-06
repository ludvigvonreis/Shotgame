using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem
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

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;
			weaponStats = groupReference.weaponStats;
			weaponState = groupReference.weaponState;

			ownerObject = groupReference.owner.ownerObject;
		}

		protected override void ProcessInput(InputAction.CallbackContext context)
		{
			if (groupReference.isRunning == false) return;

			performed = context.performed;
		}

		// TODO: Implement rest of shooting function
		void Action()
		{
			if (performed)
			{
				weaponState.currentAmmo -= 1;

				weaponState.IncreaseHeat();

				// Shoot from camera
				RaycastHit hit;
				if (Physics.Raycast(
					point.transform.position, point.transform.forward, out hit, weaponStats.range
				))
				{
					//EventManager.Instance.m_HitEvent.Invoke(new Hit(temp, hit, weaponStats));
					DecalManager.Instance.PlaceDecal(hit.point, Quaternion.identity);
				}

				// Step 3 apply recoil to player
				// FIXME: Temporary
				//ownerObject.GetComponent<TempWeap>().m_ShootEvent.Invoke(new RecoilData(weaponState, weaponStats));
			}
		}
	}
}