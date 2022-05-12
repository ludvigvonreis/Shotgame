using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeaponSystem
{
	[CustomEditor(typeof(WeaponInspector))]
	public class WeaponInspectorEditor : Editor
	{
		private Dictionary<string, bool> foldOuts = new Dictionary<string, bool>();

		private WeaponInspector weaponInspectorInstance;
		private List<WeaponAction> actions = new List<WeaponAction>();


		void OnEnable()
		{
			weaponInspectorInstance = (WeaponInspector)target;
			actions = weaponInspectorInstance.GetComponentsInChildren<WeaponAction>(true).ToList();
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			foreach (var item in actions)
			{
				if (!foldOuts.ContainsKey(item.name)) foldOuts.Add(item.name, false);

				foldOuts[item.name] = EditorGUILayout.Foldout(foldOuts[item.name], item.name);

				if (!foldOuts[item.name]) continue;

				SerializedObject actionSO = new SerializedObject(item);
				if (actionSO != null)
				{
					SerializedProperty prop = actionSO.GetIterator();
					while (prop.NextVisible(true))
					{
						if (prop.name != "m_Script")
						{
							EditorGUILayout.PropertyField(prop);
						}
					}
					prop.Reset();
				}
				actionSO.ApplyModifiedProperties();
			}
		}
	}
}