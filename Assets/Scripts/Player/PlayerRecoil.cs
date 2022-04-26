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

		float horiz = 0f;
		float verti = 0f;
		if (horizEvaluation >= 0)
		{
			horiz = Random.Range(0, horizEvaluation);
		}
		else if (horizEvaluation <= 0)
		{
			horiz = Random.Range(horizEvaluation, 0);
		}

		if (vertiEvaluation >= 0)
		{
			verti = Random.Range(0, vertiEvaluation * 3f);
		}
		else if (vertiEvaluation <= 0)
		{
			verti = Random.Range(vertiEvaluation, 0);
		}

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
