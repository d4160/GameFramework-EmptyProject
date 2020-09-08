using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DropdownConstantsAttribute))]
	public class DropdownConstantsPropertyDrawer : PropertyDrawerBase
	{
        private readonly List<MemberInfo> _constants = new List<MemberInfo>();

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
            DropdownConstantsAttribute dropdownAttribute = (DropdownConstantsAttribute)attribute;
			Type selectFromType = dropdownAttribute.SelectFromType;
			FieldInfo fieldInfo = ReflectionUtility.GetField(PropertyUtility.GetTargetObjectWithProperty(property), property.name);

			float propertyHeight = AreValuesValid(selectFromType, fieldInfo)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();

			return propertyHeight;
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			DropdownConstantsAttribute dropdownAttribute = (DropdownConstantsAttribute)attribute;
			object target = PropertyUtility.GetTargetObjectWithProperty(property);

			Type selectFromType = dropdownAttribute.SelectFromType;
			FieldInfo dropdownField = ReflectionUtility.GetField(target, property.name);

			if (AreValuesValid(selectFromType, dropdownField))
			{
                var searchFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
                var allPublicStaticFields = dropdownAttribute.SelectFromType.GetFields(searchFlags);
                var allPublicStaticProperties = dropdownAttribute.SelectFromType.GetProperties(searchFlags);

                // IsLiteral determines if its value is written at compile time and not changeable
                // IsInitOnly determines if the field can be set in the body of the constructor
                // for C# a field which is readonly keyword would have both true but a const field would have only IsLiteral equal to true
                foreach (FieldInfo field in allPublicStaticFields)
                {
                    if ((field.IsInitOnly || field.IsLiteral) && field.FieldType == selectFromType)
                        _constants.Add(field);
                }
                foreach (var prop in allPublicStaticProperties)
                {
                    if (prop.PropertyType == selectFromType) _constants.Add(prop);
                }


                if (IsNullOrEmpty(_constants)) return;

                string[] names = new string[_constants.Count];
                object[] values = new object[_constants.Count];
                for (var i = 0; i < _constants.Count; i++)
                {
                    names[i] = _constants[i].Name;
                    values[i] = GetValue(i);
                }

                // Selected value
                object selectedValue = dropdownField.GetValue(target);

                int selectedValueIndex = -1;
                bool valueFound = false;

                if (selectedValue != null)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        if (selectedValue.Equals(values[i]))
                        {
                            valueFound = true;
                            selectedValueIndex = i;
                        }
                    }
                }

                if (!valueFound)
                {
                    names = InsertAt(names, 0);
                    values = InsertAt(values, 0);
                    var actualValue = selectedValue;
                    var value = actualValue != null ? actualValue : "NULL";
                    names[0] = "NOT FOUND: " + value;
                    values[0] = actualValue;
                }

                NaughtyEditorGUI.Dropdown(
                    rect, property.serializedObject, target, dropdownField, label.text, selectedValueIndex, values, names);
            }
			else
			{
				string message =
					$"Invalid values provided to '{dropdownAttribute.GetType().Name}'. The types of the target field and the type provided to the attribute don't match";

				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private bool AreValuesValid(Type type, FieldInfo dropdownField)
		{
			if (type == null || dropdownField == null)
			{
				return false;
			}

			return dropdownField.FieldType == type;
		}

        private bool IsNullOrEmpty<T>(IList<T> collection)
        {
            if (collection == null) return true;

            return collection.Count == 0;
        }

        private object GetValue(int index)
        {
            var member = _constants[index];
            if (member.MemberType == MemberTypes.Field) return ((FieldInfo)member).GetValue(null);
            if (member.MemberType == MemberTypes.Property) return ((PropertyInfo)member).GetValue(null);
            return null;
        }

        private T[] InsertAt<T>(T[] array, int index)
        {
            if (index < 0)
            {
                Debug.LogError("Index is less than zero. Array is not modified");
                return array;
            }

            if (index > array.Length)
            {
                Debug.LogError("Index exceeds array length. Array is not modified");
                return array;
            }

            T[] newArray = new T[array.Length + 1];
            int index1 = 0;
            for (int index2 = 0; index2 < newArray.Length; ++index2)
            {
                if (index2 == index) continue;

                newArray[index2] = array[index1];
                ++index1;
            }

            return newArray;
        }
    }
}