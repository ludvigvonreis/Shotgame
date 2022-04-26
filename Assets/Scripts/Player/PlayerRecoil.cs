using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
	private WeaponManager weaponManager;
	private Player player;
	private WeaponObject weaponObject;

	public Vector2 recoil;

	void Start()
	{
		player = GetComponent<Player>();
		weaponManager = GetComponent<WeaponManager>();

		player.m_ShootEvent.AddListener(CalculateRecoil);

		recoil = Vector2.zero;
	}

	public void UpdateCurrentWeapon()
	{
		weaponObject = weaponManager.GetCurrentWeapon();
	}

	void CalculateRecoil()
	{
		if (weaponObject == null) return;

		var heat = weaponObject.state.heat;
		var maxAmmo = weaponObject.stats.maxAmmo;

		// TODO: investigate better ways to normalize heat between 0 and 1
		float recoilPoint = (float)heat / (float)maxAmmo;

		float horizEvaluation = weaponObject.stats.recoilHoriz.Evaluate(recoilPoint);
		float vertiEvaluation = weaponObject.stats.recoilVerti.Evaluate(recoilPoint);

		float horiz = Random.Range(-horizEvaluation * .2f, horizEvaluation);
		float verti = Random.Range(-vertiEvaluation * .1f, vertiEvaluation * 2f);

		Debug.LogFormat("Horizontal recoil: {0}, Vertical recoil: {1}", horiz, verti);

		recoil = new Vector2(horiz, verti);
	}

	public void ApplyRecoil(Transform movement, Transform camera)
	{
		Quaternion yawRecoilRotation = Quaternion.Euler(0.0f, recoil.x, 0.0f);
		Quaternion pitchRecoilRotation = Quaternion.Euler(-recoil.y, 0.0f, 0.0f);

		movement.localRotation *= yawRecoilRotation;
		camera.localRotation *= pitchRecoilRotation;

		recoil = Vector2.zero;
	}
}
