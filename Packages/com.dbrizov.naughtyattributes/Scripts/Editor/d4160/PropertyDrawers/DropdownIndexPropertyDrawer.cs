using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DropdownIndexAttribute))]
	public class DropdownIndexPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			float propertyHeight = property.propertyType == SerializedPropertyType.Integer
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();

			return propertyHeight;
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			DropdownIndexAttribute dropdownAttribute = (DropdownIndexAttribute)attribute;
			object valuesObject = GetValues(property, dropdownAttribute.ValuesName);

			if (property.propertyType == SerializedPropertyType.Integer)
			{
				if (valuesObject is IList)
				{
					// Selected value index
					int selectedValueIndex = property.intValue;

					// Values and display options
					IList valuesList = (IList)valuesObject;
					string[] displayOptions = new string[valuesList.Count];

					for (int i = 0; i < valuesList.Count; i++)
					{
						object value = valuesList[i];
						displayOptions[i] = value == null ? "<null>" : value.ToString();
					}

                    property.intValue = EditorGUI.Popup(rect, label.text, selectedValueIndex, displayOptions);
                }
				else if (valuesObject is IDropdownList)
				{
                    // Selected value index
                    int selectedValueIndex = property.intValue;

					// display options
					List<string> displayOptions = new List<string>();
					IDropdownList dropdown = (IDropdownList)valuesObject;

					using (IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator())
					{
						while (dropdownEnumerator.MoveNext())
						{
							KeyValuePair<string, object> current = dropdownEnumerator.Current;

							if (current.Key == null)
							{
								displayOptions.Add("<null>");
							}
							else if (string.IsNullOrWhiteSpace(current.Key))
							{
								displayOptions.Add("<empty>");
							}
							else
							{
								displayOptions.Add(current.Key);
							}
						}
					}

					if (selectedValueIndex < 0)
					{
						selectedValueIndex = 0;
					}

                    property.intValue = EditorGUI.Popup(rect, label.text, selectedValueIndex, displayOptions.ToArray());
                }
			}
			else
			{
                string message = $"{nameof(DropdownIndexAttribute)} supports only int fields";

				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private object GetValues(SerializedProperty property, string valuesName)
		{
			object target = PropertyUtility.GetTargetObjectWithProperty(property);

			FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, valuesName);
			if (valuesFieldInfo != null)
			{
				return valuesFieldInfo.GetValue(target);
			}

			PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, valuesName);
			if (valuesPropertyInfo != null)
			{
				return valuesPropertyInfo.GetValue(target);
			}

			MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, valuesName);
			if (methodValuesInfo != null &&
				methodValuesInfo.ReturnType != typeof(void) &&
				methodValuesInfo.GetParameters().Length == 0)
			{
				return methodValuesInfo.Invoke(target, null);
			}

			return null;
		}
    }
}