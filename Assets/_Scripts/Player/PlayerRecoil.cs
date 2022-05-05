using UnityEngine;
using UnityEngine.InputSystem;

public class RecoilData
{
	public int heat;
	public int maxAmmo;

	public AnimationCurve recoilHoriz;
	public AnimationCurve recoilVerti;

	public float recoilHorizMult;
	public float recoilVertiMult;

	public RecoilData(WeaponSystem.WeaponState state, WeaponSystem.WeaponStats stats)
	{
		heat = state.heat;
		maxAmmo = stats.maxAmmo;
		recoilHoriz = stats.recoilHoriz;
		recoilVerti = stats.recoilVerti;
		recoilHorizMult = stats.recoilHorizMult;
		recoilVertiMult = stats.recoilVertiMult;
	}
}

public class PlayerRecoil : MonoBehaviour
{
	private Player player;

	public Vector2 recoil;

	// Recoil resetting
	[SerializeField, Range(1f, 100f)] private float resetSpeed = 1f;
	private Vector2 totalRecoil;
	private bool isResettingRecoil;

	private bool saveOrigin;
	private bool hasSavedOrigin;

	private Quaternion shootOriginYaw;
	private Quaternion shootOriginPitch;

	private Vector2 mousePosition;

	public InputActionReference mouseButton;

	void Start()
	{
		player = GetComponent<Player>();
		TempWeap tempWeap = GetComponent<TempWeap>();

		tempWeap.m_ShootEvent.AddListener(CalculateRecoil);
		tempWeap.m_ResetRecoil.AddListener(ResetRecoil);

		recoil = Vector2.zero;
		totalRecoil = Vector2.zero;
		hasSavedOrigin = false;

		ResetSavedRotations();

		// Very long line
		mousePosition = GetComponent<PlayerInput>().actions[mouseButton.action.name].ReadValue<Vector2>();
	}

	void ResetRecoil()
	{
		isResettingRecoil = true;
		ResetSavedRotations();
	}

	void ResetSavedRotations()
	{
		shootOriginPitch = Quaternion.identity;
		shootOriginYaw = Quaternion.identity;
	}

	public void UpdateCurrentWeapon()
	{
		//weaponObject = weaponManager.GetCurrentWeapon();
	}

	void CalculateRecoil(RecoilData recoilData)
	{
		//if (weaponObject == null) return;

		if (!saveOrigin)
		{
			saveOrigin = true;
		}

		var heat = recoilData.heat;
		var maxAmmo = recoilData.maxAmmo;

		// TODO: investigate better ways to normalize heat between 0 and 1
		float recoilPoint = (float)heat / (float)maxAmmo;

		float horizEvaluation = recoilData.recoilHoriz.Evaluate(recoilPoint) * recoilData.recoilHorizMult;
		float vertiEvaluation = recoilData.recoilVerti.Evaluate(recoilPoint) * recoilData.recoilVertiMult;

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
			verti = Random.Range(0, vertiEvaluation);
		}
		else if (vertiEvaluation <= 0)
		{
			verti = Random.Range(vertiEvaluation, 0);
		}

		//Debug.LogFormat("Horizontal recoil: {0}, Vertical recoil: {1}", horiz, verti);

		recoil = new Vector2(horiz, verti);
		totalRecoil += recoil;
	}

	public void ApplyRecoil(Transform movement, Transform camera)
	{
		bool a = saveOrigin && (shootOriginPitch == Quaternion.identity && shootOriginYaw == Quaternion.identity);
		if (a && !hasSavedOrigin)
		{
			shootOriginYaw = movement.localRotation;
			shootOriginPitch = camera.localRotation;

			hasSavedOrigin = true;
		}


		Quaternion yawRecoilRotation = Quaternion.Euler(0.0f, recoil.x, 0.0f);
		Quaternion pitchRecoilRotation = Quaternion.Euler(-recoil.y, 0.0f, 0.0f);

		movement.localRotation *= yawRecoilRotation;
		camera.localRotation *= pitchRecoilRotation;

		recoil = Vector2.zero;
	}

	public void ApplyResetRecoil(Transform movement, Transform camera)
	{
		if (!isResettingRecoil) return;

		// TODO FIXMe
		var tempPos = GetComponent<PlayerInput>().actions[mouseButton.action.name].ReadValue<Vector2>();

		if (tempPos != mousePosition)
		{
			StopRecoilReset();
			return;
		}


		movement.localRotation = Quaternion.RotateTowards(movement.localRotation, shootOriginYaw, Time.deltaTime * resetSpeed);
		camera.localRotation = Quaternion.RotateTowards(camera.localRotation, shootOriginPitch, Time.deltaTime * resetSpeed);

		if (camera.localRotation != shootOriginPitch) return;

		StopRecoilReset();
	}

	void StopRecoilReset()
	{
		ResetSavedRotations();

		saveOrigin = false;
		hasSavedOrigin = false;
		isResettingRecoil = false;

		totalRecoil = Vector2.zero;
	}
}
