using UnityEditor;
using UnityEngine;

namespace WeaponSystem
{
	public static class CreateWeaponMenu
	{
		[MenuItem("GameObject/WeaponSystem/Weapon", false, 0)]
		public static void CreateWeapon()
		{
			var gameObject = new GameObject("New Weapon", typeof(Weapon), typeof(WeaponStats));

			var modules = new GameObject("Modules");
			var action = new GameObject("Action");
			var constraint = new GameObject("Constraints");
			var visual = new GameObject("Visual");

			modules.transform.parent = gameObject.transform;
			action.transform.parent = modules.transform;
			constraint.transform.parent = modules.transform;
			visual.transform.parent = gameObject.transform;

			Selection.activeGameObject = gameObject;
			SceneView.FrameLastActiveSceneView();
		}
	}
}