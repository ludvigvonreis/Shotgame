using UnityEngine;

namespace WeaponSystem.UI
{
	public class CanvasBillboard : MonoBehaviour
	{
		public Transform camTransform;

		Quaternion originalRotation;

		void Start()
		{
			originalRotation = transform.rotation;
		}

		void Update()
		{
			transform.rotation = camTransform.rotation * originalRotation;
		}
	}
}
