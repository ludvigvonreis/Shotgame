using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace WeaponSystem
{
	class WeaponBuilder : EditorWindow
	{
		private VisualElement m_RightPane;

		[MenuItem("Window/Weapon Builder")]
		public static void ShowMyEditor()
		{
			// This method is called when the user selects the menu item in the Editor
			EditorWindow wnd = GetWindow<WeaponBuilder>();
			wnd.titleContent = new GUIContent("Weapon Builder");

			// Limit size of the window
			wnd.minSize = new Vector2(800, 600);
			wnd.maxSize = new Vector2(1920, 720);
		}

		public void CreateGUI()
		{
			List<GameObject> go = GameObject.FindObjectsOfType<Weapon>().Select(e => e.gameObject).ToList();

			// Create a two-pane view with the left pane being fixed with
			var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

			// Add the view to the visual tree by adding it as a child to the root element
			rootVisualElement.Add(splitView);

			// A TwoPaneSplitView always needs exactly two child elements
			var leftPane = new ListView();
			splitView.Add(leftPane);
			m_RightPane = new VisualElement();
			splitView.Add(m_RightPane);

			if (go != null)
			{
				leftPane.makeItem = () => new Label();
				leftPane.bindItem = (item, index) => { (item as Label).text = go[index].name; };
				leftPane.itemsSource = go;
				leftPane.onSelectionChange += OnWeaponSelectionChange;
			}
		}

		private void OnWeaponSelectionChange(IEnumerable<object> selectedItems)
		{
			m_RightPane.Clear();

			var weapon = (selectedItems.First() as GameObject).GetComponent<Weapon>();
			var weaponModuleGroups = weapon.GetComponentsInChildren<WeaponModuleGroup>(true).ToList();

			var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
			m_RightPane.Add(splitView);

			var groupPane = new ListView();
			splitView.Add(groupPane);
			var tempPane = new VisualElement();
			splitView.Add(tempPane);

			groupPane.makeItem = () => new Label();
			groupPane.bindItem = (item, index) => { (item as Label).text = weaponModuleGroups[index].name; };
			groupPane.itemsSource = weaponModuleGroups;
		}
	}
}