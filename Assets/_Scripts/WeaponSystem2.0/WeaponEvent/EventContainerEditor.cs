using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace WeaponSystem.Events
{
	[CustomEditor(typeof(EventContainer))]
	[CanEditMultipleObjects]
	public class EventContainerEditor : Editor
	{
		private ReorderableList eventList;

		private SerializedProperty weaponEvents;

		public void OnEnable()
		{
			weaponEvents = serializedObject.FindProperty("weaponEvents");

			eventList = new ReorderableList(
					serializedObject,
					weaponEvents,
					draggable: true,
					displayHeader: true,
					displayAddButton: true,
					displayRemoveButton: true);

			eventList.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "WeaponEvents");
			};

			eventList.onRemoveCallback = (ReorderableList l) =>
			{
				var element = l.serializedProperty.GetArrayElementAtIndex(l.index);
				var obj = element.objectReferenceValue;

				AssetDatabase.RemoveObjectFromAsset(obj);

				DestroyImmediate(obj, true);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();


				ReorderableList.defaultBehaviours.DoRemoveButton(l);
			};

			eventList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				if (eventList.count <= 0) return;

				SerializedProperty element = weaponEvents.GetArrayElementAtIndex(index);

				rect.y += 2;
				rect.width -= 10;
				rect.height = EditorGUIUtility.singleLineHeight;

				if (element.objectReferenceValue == null)
				{
					return;
				}
				string label = element.objectReferenceValue.name;

				var newName = EditorGUI.DelayedTextField(rect, label, EditorStyles.textField);
				if (newName != label)
				{
					ChangeName(element, newName);
				}

				// Convert this element's data to a SerializedObject so we can iterate
				// through each SerializedProperty and render a PropertyField.
				SerializedObject nestedObject = new SerializedObject(element.objectReferenceValue);

				// Loop over all properties and render them
				SerializedProperty prop = nestedObject.GetIterator();
				float y = rect.y;
				while (prop.NextVisible(true))
				{
					if (prop.name == "m_Script")
					{
						continue;
					}

					rect.y += EditorGUIUtility.singleLineHeight;
					EditorGUI.PropertyField(rect, prop);
				}

				nestedObject.ApplyModifiedProperties();

				// Mark edits for saving
				if (GUI.changed)
				{
					EditorUtility.SetDirty(target);
				}

			};

			eventList.elementHeightCallback = (int index) =>
			{
				if (eventList.count <= 0) return 0;

				float baseProp = EditorGUI.GetPropertyHeight(
					eventList.serializedProperty.GetArrayElementAtIndex(index), true);

				float additionalProps = 0;
				SerializedProperty element = weaponEvents.GetArrayElementAtIndex(index);
				if (element.objectReferenceValue != null)
				{
					SerializedObject ability = new SerializedObject(element.objectReferenceValue);
					SerializedProperty prop = ability.GetIterator();
					while (prop.NextVisible(true))
					{
						// XXX: This logic stays in sync with loop in drawElementCallback.
						if (prop.name == "m_Script")
						{
							continue;
						}
						additionalProps += EditorGUIUtility.singleLineHeight;
					}
				}

				float spacingBetweenElements = EditorGUIUtility.singleLineHeight / 2;

				return baseProp + spacingBetweenElements + additionalProps;
			};

			eventList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
			{
				addClickHandler();
			};
		}

		private void ChangeName(SerializedProperty element, string newName)
		{
			WeaponEvent obj = element.objectReferenceValue as WeaponEvent;
			obj.name = newName;

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(obj);
		}

		private void addClickHandler()
		{
			// Make room in list
			var index = eventList.serializedProperty.arraySize;
			eventList.serializedProperty.arraySize++;
			eventList.index = index;
			var element = eventList.serializedProperty.GetArrayElementAtIndex(index);

			var newAbility = ScriptableObject.CreateInstance<WeaponEvent>();
			newAbility.name = "test";

			// Add it to CardData
			var cardData = (EventContainer)target;
			AssetDatabase.AddObjectToAsset(newAbility, cardData);
			AssetDatabase.SaveAssets();
			element.objectReferenceValue = newAbility;
			serializedObject.ApplyModifiedProperties();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawDefaultInspector();

			eventList.DoLayoutList();

			if (GUILayout.Button("Delete All Events"))
			{
				var path = AssetDatabase.GetAssetPath(target);
				Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
				for (int i = 0; i < assets.Length; i++)
				{
					if (assets[i] is WeaponEvent)
					{
						Object.DestroyImmediate(assets[i], true);
					}
				}

				weaponEvents.ClearArray();

				AssetDatabase.SaveAssets();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}