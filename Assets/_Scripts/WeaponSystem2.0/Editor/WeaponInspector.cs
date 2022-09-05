using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeaponSystem
{
	class WeaponInspector : EditorWindow
	{
		Vector2 scroll = new Vector2();
		Dictionary<string, bool> foldState = new Dictionary<string, bool>();

		[MenuItem("Window/My Window")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(WeaponInspector));
		}

		bool RecursiveFoldout(GameObject obj, int indent = 0)
		{
			if (!foldState.TryGetValue(obj.name, out _)) foldState[obj.name] = false;

			EditorGUI.indentLevel = indent;
			var indentedRect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect());
			//Rect itemRect = EditorGUILayout.GetControlRect();
			//var textDimensions = GUI.skin.label.CalcSize(new GUIContent(obj.name));
			//itemRect.x += textDimensions.x + 20f;
			//itemRect.x -= textDimensions.x + 20f;
			//GUI.Button(itemRect, obj.name);

			//foldState[obj.name] = EditorGUI.Foldout(itemRect, foldState[obj.name], "");
			foldState[obj.name] =
				EditorGUI.BeginFoldoutHeaderGroup(indentedRect, foldState[obj.name], obj.name, null, delegate (Rect a)
				{
					Debug.Log("Test!");
				});
			EditorGUI.EndFoldoutHeaderGroup();

			if (foldState[obj.name] == false) return foldState[obj.name];
			if (obj.transform.childCount == 0) return foldState[obj.name];

			foreach (Transform child in obj.transform)
			{
				RecursiveFoldout(child.gameObject, indent + 1);
			}

			return foldState[obj.name];
		}

		void OnGUI()
		{

			EditorGUILayout.BeginHorizontal();
			{
				scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Width(200), GUILayout.Height(500));
				{
					//GameObject[] gameObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
					//gameObjects = gameObjects.Where(e => e.GetComponent<Weapon>() != null).ToArray();

					var go = Selection.activeGameObject;
					if (go != null) foldState[go.name] = RecursiveFoldout(go);
					//foreach (GameObject go in gameObjects)
					//{}
				}
				EditorGUILayout.EndScrollView();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}