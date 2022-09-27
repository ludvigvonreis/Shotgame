using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using WeaponSystem;

public class PlayerToWeaponInterface : MonoBehaviour
{
	Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();

	[SerializeField]
	WeaponEventInterface weaponEventInterface;

	// Start is called before the first frame update
	void Start()
	{
		var playerInput = GetComponent<PlayerInput>();
		_inputActions = playerInput.actions.ToDictionary(x => x.name, x => x);

		//weaponEventInterface.
	}
}
