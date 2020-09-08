using System;
using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(RequireTypeAttribute))]
    public class RequireTypePropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.ObjectReference)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                // Get attribute parameters.
                var requiredAttribute = this.attribute as RequireTypeAttribute;
                // Begin drawing property field.
                EditorGUI.BeginProperty(rect, label, property);
                
                Type targetType = default;

                targetType = requiredAttribute.RequiredType.IsGenericTypeDefinition ? requiredAttribute.RequiredType.MakeGenericType(requiredAttribute.GenericArguments) : requiredAttribute.RequiredType;

                // Draw property field.
                property.objectReferenceValue = EditorGUI.ObjectField(rect, label, property.objectReferenceValue, targetType, true);
                
                // Finish drawing property field.
                EditorGUI.EndProperty();
            }
            else
            {
                string message = $"{nameof(RequireTypeAttribute)} supports only Object fields";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }
        }
    }
}