using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AWeaponAction : MonoBehaviour
{
	public virtual void Init(WeaponObject weaponObject, WeaponLogic _logic) { }
	public virtual void Run(InputAction action) { }
}

public class WeaponLogic : MonoBehaviour
{
	[SerializeField] private Transform shootPoint;
	[SerializeField] public Camera shootCamera;

	private Player _player;

	[HideInInspector]
	public Player player
	{
		get { return _player; }
		set
		{
			if (value == null)
			{
				_player = null;
				shootCamera = null;
			}

			_player = value;
			shootCamera = value.playerCam;

			Init();
		}
	}

	[SerializeField]
	private AWeaponAction primaryAction;
	[SerializeField]
	private AWeaponAction secondaryAction;
	[SerializeField]
	private AWeaponAction reloadAction;

	private bool isInitiated = false;

	// Input
	private InputAction primaryInput;
	private InputAction secondaryInput;
	private InputAction reloadInput;

	void Init()
	{
		var weaponObject = GetComponent<WeaponObject>();

		primaryInput = player.playerInput.actions[player.shootButton.action.name];
		secondaryInput = player.playerInput.actions[player.shootButton.action.name];
		reloadInput = player.playerInput.actions[player.reloadButton.action.name];

		primaryAction.Init(weaponObject, this);
		secondaryAction.Init(weaponObject, this);
		reloadAction.Init(weaponObject, this);


		isInitiated = true;
	}

	void Update()
	{
		if (!isInitiated) return;

		primaryAction.Run(primaryInput);
		secondaryAction.Run(secondaryInput);
		reloadAction.Run(reloadInput);
	}
}
