using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace WeaponSystem
{
	[CustomEditor(typeof(Weapon))]
	class WeaponInspector : Editor
	{
		public VisualTreeAsset m_InspectorXML;

		Weapon script;
		GameObject scriptObject;

		void OnEnable()
		{
			script = (Weapon)target;
			scriptObject = script.gameObject;
		}

		public override VisualElement CreateInspectorGUI()
		{
			// Create a new VisualElement to be the root of our inspector UI
			VisualElement myInspector = new VisualElement();

			// Load from default reference
			m_InspectorXML.CloneTree(myInspector);

			var weaponModuleGroups = scriptObject.GetComponentsInChildren<WeaponModuleGroup>(true).ToList();

			ListView modulePane = myInspector.Q("groupList") as ListView;

			modulePane.makeItem = () => new Label();
			modulePane.bindItem = (item, index) => { (item as Label).text = weaponModuleGroups[index].name; };
			modulePane.itemsSource = weaponModuleGroups;

			VisualElement inspectorFoldout = myInspector.Q("Default_Inspector");
			InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

			// Return the finished inspector UI
			return myInspector;
		}
	}
}