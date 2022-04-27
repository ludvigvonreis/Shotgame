using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	private Dictionary<string, WeaponObject> equippedWeapons = new Dictionary<string, WeaponObject>();
	private List<string> equippedWeaponIds = new List<string>();
	private string currentWeaponID;

	[SerializeField, Range(1, 10)] private int maxWeapons = 1;

	[SerializeField] private WeaponHolder weaponHolder;

	private Player player;
	private Camera playerCamera;
	private PlayerRecoil playerRecoil;

	public bool CanEquip => equippedWeapons.Count < maxWeapons;

	void Start()
	{
		player = transform.root.GetComponent<Player>();
		playerRecoil = transform.root.GetComponent<PlayerRecoil>();
		playerCamera = player.playerCam;

		player.m_Interact.AddListener(InteractListener);
	}

	// TODO: Implement weapon switching


	void InteractListener(Interaction interaction)
	{
		Debug.LogFormat("{0} interacted with {1}, distance={2}",
			interaction.from.name, interaction.to.name, interaction.distance);

		var reciver = interaction.to;

		WeaponObject obj;
		if (reciver.TryGetComponent<WeaponObject>(out obj))
		{
			obj.Interact();
			obj.PostInteract();

			AddWeapon(obj);
		}
	}

	public WeaponObject GetWeapon(string wepId) => equippedWeapons[wepId];

	public WeaponObject GetCurrentWeapon() => equippedWeapons[currentWeaponID];

	public void AddWeapon(WeaponObject wepObj)
	{
		weaponHolder.AddWeapon(wepObj);
		equippedWeapons.Add(wepObj.ID, wepObj);
		equippedWeaponIds.Add(wepObj.ID);

		// FIXME: Temporary until weapon switching is implemented
		SelectWeapon(wepObj.ID);

		wepObj.logic.SetPlayer(player);

		playerRecoil.UpdateCurrentWeapon();
	}

	public void RemoveWeapon(WeaponObject wepObj)
	{
		equippedWeapons.Remove(wepObj.ID);
		equippedWeaponIds.Remove(wepObj.ID);
		wepObj.logic.UnsetPlayer();
	}

	public void RemoveWeapon(string wepId)
	{
		equippedWeapons[wepId].logic.UnsetPlayer();
		equippedWeapons.Remove(wepId);
		equippedWeaponIds.Remove(wepId);
	}

	public void SelectWeapon(string wepId)
	{
		currentWeaponID = wepId;
	}

	public void SelectWeapon(int index)
	{
		currentWeaponID = equippedWeaponIds[index];
	}
}
