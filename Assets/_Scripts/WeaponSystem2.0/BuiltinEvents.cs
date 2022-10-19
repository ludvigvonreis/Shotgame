using UnityEngine;
using UnityEngine.EventSystems;

namespace WeaponSystem
{
	/// <summary>
	/// Events sent by weapon aiming modules.
	/// </summary>
	public interface IWeaponADSEvents : IEventSystemHandler
	{
		/// <summary>
		/// Event that gets executed when weapon aims in.
		/// </summary>
		void OnAimIn();

		/// <summary>
		/// Event that gets executed when weapon aims out.
		/// </summary>
		void OnAimOut();

		/// <summary>
		/// Event that gets executed when changing FOV.
		/// </summary>
		/// <param name="newFOV">New FOV value</param>
		void OnFOVChange(float newFOV);
	}

	/// <summary>
	/// Events sent when weapon fires.
	/// </summary>
	public interface IWeaponShootEvents : IEventSystemHandler
	{
		/// <summary>
		/// Event that gets executed when weapon is fired.
		/// </summary>
		void OnShoot();

		/// <summary>
		/// Event that gets executed when weapon has stopped firing for some time.
		/// </summary>
		void OnTimeout();
	}

	/// <summary>
	/// Events sent when weapon reloads.
	/// </summary>
	public interface IWeaponReloadEvents : IEventSystemHandler
	{
		/// <summary>
		/// Event that gets executed when weapon starts reloading.
		/// </summary>
		void OnReloadStart();

		/// <summary>
		/// Event that gets executed when weapon stops reloading. I.e is finished.
		/// </summary>
		void OnReloadStop();
	}

	/// <summary>
	/// Events sent when weapon state changes.
	/// </summary>
	public interface IWeaponStateEvents : IEventSystemHandler
	{
		/// <summary>
		/// Event that gets executed when weapon heat increases.
		/// </summary>
		void OnHeatIncrease();

		/// <summary>
		/// Event that gets executed when weapon heat decreases.
		/// </summary>
		void OnHeatDecrease();

		/// <summary>
		/// Event that gets executed when weapon ammo changes.
		/// </summary>
		/// <param name="newAmmo">Ammo after change</param>
		void OnAmmoChange(int newAmmo);

		/// <summary>
		/// Event that gets executed when weapon ammo reserve changes.
		/// </summary>
		/// <param name="newAmmoReserve">Ammo reserve after change</param>
		void OnAmmoReserveChange(int newAmmoReserve);
	}
}