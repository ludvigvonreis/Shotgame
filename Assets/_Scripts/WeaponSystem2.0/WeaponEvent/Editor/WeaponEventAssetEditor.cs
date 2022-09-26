using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace WeaponSystem.Events
{
	[CustomEditor(typeof(WeaponEventAsset))]
	[CanEditMultipleObjects]
	public class WeaponEventAssetEditor : Editor
	{
		private ReorderableList eventList;

		private SerializedProperty weaponEventReferences;

		private WeaponEventAsset asset;

		public void OnEnable()
		{
			asset = target as WeaponEventAsset;

			weaponEventReferences = serializedObject.FindProperty("weaponEventReferences");

			eventList = new ReorderableList(
					serializedObject,
					weaponEventReferences,
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
				var obj = element.objectReferenceValue as WeaponEventReference;

				AssetDatabase.RemoveObjectFromAsset(obj);

				DestroyImmediate(obj, true);

				asset.RemoveEvent(obj.Event);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				ReorderableList.defaultBehaviours.DoRemoveButton(l);
			};

			eventList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				if (eventList.count <= 0) return;

				SerializedProperty element = weaponEventReferences.GetArrayElementAtIndex(index);

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
				SerializedProperty element = weaponEventReferences.GetArrayElementAtIndex(index);
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
				CreateNewEvent();
			};
		}

		private void ChangeName(SerializedProperty element, string newName)
		{
			WeaponEventReference obj = element.objectReferenceValue as WeaponEventReference;
			//obj.name = newName;

			asset.RenameEvent(obj.Event, newName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(obj);
		}

		private void CreateNewEvent()
		{
			var index = eventList.serializedProperty.arraySize;
			eventList.serializedProperty.arraySize++;
			eventList.index = index;
			var element = eventList.serializedProperty.GetArrayElementAtIndex(index);

			// TODO: Add a proper event creation
			var newEvent = asset.NewEvent("New event");
			var newEventReference = WeaponEventReference.Create(newEvent);

			var cardData = (WeaponEventAsset)target;
			AssetDatabase.AddObjectToAsset(newEventReference, cardData);
			AssetDatabase.SaveAssets();
			element.objectReferenceValue = newEventReference;
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
					if (assets[i] is WeaponEventReference)
					{
						Object.DestroyImmediate(assets[i], true);
					}
				}

				weaponEventReferences.ClearArray();

				AssetDatabase.SaveAssets();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}