using UnityEngine;
using WeaponSystem.Events;

namespace WeaponSystem.Actions
{
	[System.Serializable]
	public class WeaponKickback : WeaponAction
	{
		[SerializeField] private Transform kickbackHolder;
		private WeaponModelTransform weaponMover;
		[SerializeField] private float kickbackForce;
		[SerializeField, Range(0f, 10f)] private float maxForce;

		private bool isShooting;
		private bool hasReset;

		public override void Init()
		{
			base.Init();

			groupReference.Action.OnPerfom += Action;
			weaponMover = kickbackHolder.GetComponent<WeaponModelTransform>();
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

		void Action()
		{
			if (isShooting)
			{
				var kickbackPos = -new Vector3(0, 0, kickbackForce * Random.value);
				weaponMover.AddPosition(kickbackPos);
			}
		}
	}
}