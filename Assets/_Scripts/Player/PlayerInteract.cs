using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gnome
{
	public class PlayerInteract : MonoBehaviour
	{
		[SerializeField] private InputActionReference interactButton;
		[SerializeField] private InputActionReference dropButton;

		[SerializeField] private Transform interactTransform;
		[SerializeField, Range(.01f, 5)] private float range = 3;
		[SerializeField, Range(.01f, 5)] private float radius = .2f;

		private Player player;

		GameObject hovering;

		void Start()
		{
			player = GetComponent<Player>();

			var interactAction = player.playerInput.actions[interactButton.action.name];
			interactAction.performed += _ => { Interact(); };

			var dropAction = player.playerInput.actions[dropButton.action.name];
			dropAction.performed += _ => { Drop(); };
		}

		void Update()
		{
			if (Physics.SphereCast(interactTransform.position, radius, interactTransform.forward, out RaycastHit raycastHit, range))
			{
				if (!raycastHit.transform.TryGetComponent<IInteractible>(out _)) return;
				if (!raycastHit.transform.TryGetComponent<WeaponSystem.UI.IHoverable>(out WeaponSystem.UI.IHoverable hoverable)) return;

				// HACK: Should be implemented much better.
				if (player.weaponManager?.GetCurrentWeapon() && player.weaponManager.GetCurrentWeapon().weaponState.isAiming)
				{
					// Hide when aiming down sights
					hoverable.OnHover(true);
				}
				else
				{
					hoverable.OnHover(false);
				}

				if (hovering != raycastHit.transform.gameObject)
				{
					// Start hover on new object
					hovering = raycastHit.transform.gameObject;
				}
			}
			else
			{
				// Stopped hovering, exit
				if (hovering != null && hovering.TryGetComponent<WeaponSystem.UI.IHoverable>(out WeaponSystem.UI.IHoverable oldHoverable))
				{
					oldHoverable.OnHover(true);
					hovering = null;
				}
			}
		}

		void Drop()
		{
			var wepMan = GetComponent<WeaponManager>();
			wepMan.DropCurrent();
		}

		void Interact()
		{
			if (hovering == null) return;

			var interactable = hovering.transform.GetComponent<IInteractible>();

			// FIXME: This is horrible.
			var type = interactable.InteractType();
			if (type == "Weapon")
			{
				var weapon = hovering.transform.GetComponent<WeaponSystem.Weapon>();
				GetComponent<WeaponManager>().EquipWeapon(weapon);
			}

			interactable.Interact();
		}
	}
}
