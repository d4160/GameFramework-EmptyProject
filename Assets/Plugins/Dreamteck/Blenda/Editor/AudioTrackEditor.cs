using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreamteck.Blenda.Editor
{
    public class AudioTrackEditor
    {
        Sequence sequence = null;
        public Sequence.AudioTrack track = null;
        public Rect panelRect = new Rect();
        public Rect trackRect = new Rect();
        private bool rename = false;
        public float blendAmount = 1f;
        BlenderEditorWindow editorWindow = null;

        public AudioTrackEditor(Sequence.AudioTrack t, Sequence s, BlenderEditorWindow w)
        {
            sequence = s;
            track = t;
            editorWindow = w;
        }

        public bool ContainsMouse()
        {
            return panelRect.Contains(Event.current.mousePosition) || trackRect.Contains(Event.current.mousePosition);
        }

        public void DrawPanel(bool selected)
        {
            EditorGUI.DrawRect(panelRect, BlendaEditorGUI.baseColor);
            if (panelRect.Contains(Event.current.mousePosition))
            {
                if(Event.current.type == EventType.MouseDown && Event.current.button == 1)
                {
                    GenericMenu menu = new GenericMenu();
                    if(rename) menu.AddDisabledItem(new GUIContent("Rename"));
                    else menu.AddItem(new GUIContent("Rename"), false, OnRenameSelected);
                    menu.AddItem(new GUIContent("Settings"), false, OnSettingsSelected);
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete"), false, OnDeleteSelected);
                    menu.ShowAsContext();
                }
            }
            if (selected) EditorGUI.DrawRect(panelRect, BlendaEditorGUI.lightColor);
            GUI.BeginGroup(panelRect);
            if (rename)
            {
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) rename = false;
                else track.name = GUI.TextField(new Rect(3f, 5f, panelRect.width - 10, 18f), track.name);
            }
            else GUI.Label(new Rect(3f, 5f, panelRect.width - 10, 18f), track.name);

            if(blendAmount > 0f) EditorGUI.DrawRect(new Rect(0f, panelRect.height - 5, panelRect.width * blendAmount, 5f), BlendaEditorGUI.highlightColor);


            Rect volumeRect = new Rect(5, panelRect.height - 5 - 9, panelRect.width - 10, 4);
            EditorGUI.DrawRect(volumeRect, BlendaEditorGUI.darkColor);
            EditorGUI.DrawRect(new Rect(volumeRect.x, volumeRect.y, volumeRect.width * track.volume, volumeRect.height), Color.Lerp(track.mute ? Color.red : Color.green, Color.grey, 0.5f));

            GUI.EndGroup();
        }

        void OnSettingsSelected()
        {
            ConfigureTrackWindow window = EditorWindow.GetWindow<ConfigureTrackWindow>(true);
            window.Init(track, editorWindow);
        }

        void OnRenameSelected()
        {
            rename = true;
        }

        void OnDeleteSelected()
        {
            sequence.RemoveTrack(track);
        }

        public void DrawTrack()
        {
            EditorGUI.DrawRect(trackRect, BlendaEditorGUI.darkColor);
        }

        public void DrawSeparatorTop()
        {
            EditorGUI.DrawRect(new Rect(trackRect.x, trackRect.y-1, trackRect.width, 1f), BlendaEditorGUI.borderColor);
        }

        public void DrawSeparatorBottom()
        {
            EditorGUI.DrawRect(new Rect(trackRect.x, trackRect.y + trackRect.height -1 , trackRect.width, 1f), BlendaEditorGUI.borderColor);
        }

        public bool GetDraggedClip(out bool hasDrag)
        {
            hasDrag = false;
            if (trackRect.Contains(Event.current.mousePosition))
            {
                Object obj = null;
                hasDrag = GetDraggedObject(out obj);
                if (hasDrag)
                {
                    Object dragObj = DragAndDrop.objectReferences[0];
                    if (dragObj != null && dragObj is AudioClip)
                    {
                        GUI.color = new Color(1f, 1f, 1f, 0.5f);
                        GUI.Box(trackRect, "+");
                        GUI.color = Color.white;
                        if (obj != null && obj is AudioClip)
                        {
                            Undo.RecordObject(track.sequence.blender, "Add Clip " + obj.name);
                            track.AddClip((AudioClip)obj, Mathf.InverseLerp(trackRect.x, trackRect.x + trackRect.width, Event.current.mousePosition.x) * sequence.duration);
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        bool GetDraggedObject(out Object dragged)
        {
            dragged = null;
            if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (Event.current.type == EventType.DragPerform)
                {
                    dragged = DragAndDrop.objectReferences[0];
                    DragAndDrop.AcceptDrag();
                }
            }
            if (DragAndDrop.objectReferences.Length == 0) return false;
            return DragAndDrop.objectReferences[0] != null;
        }
    }
}
