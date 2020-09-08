using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyInPlayAttribute))]
    public class ReadOnlyInPlayPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property);
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (Application.isPlaying) GUI.enabled = false;
            EditorGUI.PropertyField(rect, property, label, true);
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
    }
}