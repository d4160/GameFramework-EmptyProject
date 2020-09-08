using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dreamteck.Blenda;

namespace Dreamteck.Blenda.Editor
{
    public class ConfigureTrackWindow : EditorWindow
    {
        Sequence.AudioTrack track = null;
        BlenderEditorWindow editorWindow;
        public void Init(Sequence.AudioTrack input, BlenderEditorWindow editorWindow)
        {
            track = input;
            titleContent = new GUIContent(track.name + " Settings");
            minSize = maxSize = new Vector2(300, 120);
            this.editorWindow = editorWindow;
        }

        private void OnGUI()
        {
            if (editorWindow == null)
            {
                Close();
                return;
            }

            EditorGUIUtility.labelWidth = 60;
            track.mute = EditorGUILayout.Toggle("Mute", track.mute);
            track.volume = EditorGUILayout.Slider("Volume", track.volume, 0f, 1f);
            if (GUI.changed) editorWindow.Repaint();
            EditorGUIUtility.labelWidth = 0;
        }

    }
}
