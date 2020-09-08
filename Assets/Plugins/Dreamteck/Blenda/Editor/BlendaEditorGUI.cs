using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreamteck.Blenda.Editor
{
    public static class BlendaEditorGUI 
    {
        public static Texture2D bgImg
        {
            get
            {
                if (_white == null)
                {
                    _white = new Texture2D(1, 1);
                    _white.SetPixel(0, 0, Color.white);
                    _white.Apply();
                }
                return _white;
            }
        }
        private static Texture2D _white = null;

        public static readonly Color backgroundColor = new Color(0.95f, 0.95f, 0.95f);
        public static Color iconColor = Color.black;

        public static readonly Color highlightColor = new Color(0f, 0.564f, 1f, 1f);
        public static readonly Color highlightContentColor = new Color(1f, 1f, 1f, 0.95f);


        public static readonly Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);

        public static readonly Color baseColor = Color.white;
        public static readonly Color lightColor = Color.white;
        public static readonly Color lightDarkColor = Color.white;
        public static readonly Color darkColor = Color.white;
        public static readonly Color borderColor = Color.white;


        static BlendaEditorGUI()
        {
            baseColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
            lightColor = EditorGUIUtility.isProSkin ? new Color32(84, 84, 84, 255) : new Color32(222, 222, 222, 255);
            lightDarkColor = EditorGUIUtility.isProSkin ? new Color32(30, 30, 30, 255) : new Color32(180, 180, 180, 255);
            darkColor = EditorGUIUtility.isProSkin ? new Color32(15, 15, 15, 255) : new Color32(152, 152, 152, 255);
            borderColor = EditorGUIUtility.isProSkin ? new Color32(5, 5, 5, 255) : new Color32(100, 100, 100, 255);
            backgroundColor = baseColor;
            backgroundColor -= new Color(0.1f, 0.1f, 0.1f, 0f);
            iconColor = GUI.skin.label.normal.textColor;
        }

        public static bool EditorLayoutSelectableButton(GUIContent content, bool active = true, bool selected = false, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            Color prevContentColor = GUI.contentColor;
            Color prevBackgroundColor = GUI.backgroundColor;
            GUIStyle selectedStyle = GUI.skin.button;
            if (!active) GUI.color = inactiveColor;
            else
            {
                GUI.color = Color.white;
                if (selected)
                {
                    GUI.backgroundColor = highlightColor;
                    GUI.contentColor = highlightContentColor;
                    selectedStyle = new GUIStyle(selectedStyle);
                    selectedStyle.normal.textColor = Color.white;
                    selectedStyle.hover.textColor = Color.white;
                    selectedStyle.active.textColor = Color.white;
                }
                else
                {
                    GUI.contentColor = iconColor;
                    GUI.backgroundColor = Color.white;
                }
            }
            bool clicked = GUILayout.Button(content, selectedStyle, options);
            GUI.color = prevColor;
            GUI.contentColor = prevContentColor;
            GUI.backgroundColor = prevBackgroundColor;
            return clicked && active;
        }
    }
}
