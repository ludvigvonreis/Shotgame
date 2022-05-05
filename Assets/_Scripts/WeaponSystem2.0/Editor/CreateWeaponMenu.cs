using UnityEditor;
using UnityEngine;

namespace WeaponSystem
{
	public static class CreateWeaponMenu
	{
		[MenuItem("GameObject/Weapon System/Weapon", false, -1)]
		public static void CreateWeapon()
		{
			var gameObject = new GameObject("New Weapon", typeof(Weapon), typeof(WeaponState));

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

		[MenuItem("GameObject/Weapon System/Module Group", false, -1)]
		public static void CreateModuleGroup()
		{
			var gameObject = new GameObject("New Module group", typeof(WeaponModuleGroup));

			gameObject.transform.parent = Selection.activeTransform;
			Selection.activeGameObject = gameObject;

			var action = new GameObject("Action");
			var constraint = new GameObject("Constraints", typeof(WeaponConstraint));

			action.transform.parent = gameObject.transform;
			constraint.transform.parent = gameObject.transform;
		}
	}
}