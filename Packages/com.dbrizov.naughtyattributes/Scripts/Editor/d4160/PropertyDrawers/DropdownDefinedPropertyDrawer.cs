using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DropdownDefinedAttribute))]
	public class DropdownDefinedPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
            DropdownDefinedAttribute dropdownAttribute = (DropdownDefinedAttribute)attribute;
			object[] values = dropdownAttribute.ValuesArray;
			FieldInfo fieldInfo = ReflectionUtility.GetField(PropertyUtility.GetTargetObjectWithProperty(property), property.name);

			float propertyHeight = AreValuesValid(values, fieldInfo)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();

			return propertyHeight;
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

            DropdownDefinedAttribute dropdownAttribute = (DropdownDefinedAttribute)attribute;
			object target = PropertyUtility.GetTargetObjectWithProperty(property);

			object[] valuesObject = dropdownAttribute.ValuesArray;
			FieldInfo dropdownField = ReflectionUtility.GetField(target, property.name);

            if (AreValuesValid(valuesObject, dropdownField))
            {
                if (valuesObject is IList && dropdownField.FieldType == GetElementType(valuesObject))
                {
                    // Selected value
                    object selectedValue = dropdownField.GetValue(target);

                    // Values and display options
                    IList valuesList = (IList) valuesObject;
                    object[] values = new object[valuesList.Count];
                    string[] displayOptions = new string[valuesList.Count];

                    for (int i = 0; i < values.Length; i++)
                    {
                        object value = valuesList[i];
                        values[i] = value;
                        displayOptions[i] = value == null ? "<null>" : value.ToString();
                    }

                    // Selected value index
                    int selectedValueIndex = Array.IndexOf(values, selectedValue);
                    if (selectedValueIndex < 0)
                    {
                        selectedValueIndex = 0;
                    }

                    NaughtyEditorGUI.Dropdown(
                        rect, property.serializedObject, target, dropdownField, label.text, selectedValueIndex, values,
                        displayOptions);
                }
            }
            else
			{
				string message =
                    $"Invalid values provided to '{dropdownAttribute.GetType().Name}'. The types of the target field and the values provided to the attribute don't match";

				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

        private bool AreValuesValid(object[] values, FieldInfo dropdownField)
		{
			if (values == null || dropdownField == null)
			{
				return false;
			}

			return dropdownField.FieldType == GetElementType(values);
        }

		private Type GetElementType(object[] values)
		{
			return values[0].GetType();
		}
	}
}