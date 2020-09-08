using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.Integer)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                // Draw the layer field
                property.intValue = EditorGUI.LayerField(rect, label, property.intValue);
            }
            else
            {
                string message = $"{nameof(LayerAttribute)} supports only int fields";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }
        }
    }
}