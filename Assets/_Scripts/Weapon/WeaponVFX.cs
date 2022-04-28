using UnityEngine;
using UnityEngine.VFX;

public class WeaponVFX : MonoBehaviour
{
	[SerializeField]
	private VisualEffect muzzleFlash;

	public void PlayMuzzleflash()
	{
		muzzleFlash.Play();
	}
}
