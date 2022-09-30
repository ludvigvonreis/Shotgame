using UnityEngine;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponKickback : WeaponAction
	{
		[SerializeField] private Transform kickbackHolder;
		[SerializeField] private float kickbackForce;

		[SerializeField] private float resetSmoothing;

		private bool isShooting;
		private bool hasReset;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;
			groupReference.OnGroupProcess += ResetKickback;
		}

		protected override void ProcessInput(object sender, WeaponEvent.ActionContext context)
		{
			if (groupReference.isRunning == false) return;

			if (context.performed)
			{
				isShooting = true;
			}

			if (context.canceled)
			{
				isShooting = false;
			}
		}

		void ResetKickback()
		{
			/*kickbackHolder.localPosition = Vector3.Lerp(
					kickbackHolder.localPosition, Vector3.zero, Time.deltaTime * resetSmoothing
				);*/
		}

		void Action()
		{
			if (isShooting)
			{
				kickbackHolder.localPosition -= new Vector3(0, 0, kickbackForce * Random.value);
			}
		}
	}
}