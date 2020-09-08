using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;

namespace Dreamteck.Blenda.Editor
{
    [CustomEditor(typeof(AudioBlender))]
    public class AudioBlenderEditor : UnityEditor.Editor
    {
        BlenderEditorWindow editorWindow = null;

        static Texture2D playIcon;
        static Texture2D pauseIcon;
        static Texture2D stopIcon;

        GUIContent playContent;
        GUIContent pauseContent;
        GUIContent stopContent;

        Sequence renameSequence = null;

        GUIStyle boxStyle = null;

 

        private void OnEnable()
        {
            AudioBlender blender = (AudioBlender)target;
            blender.EditorAwake();
            if (blender.serializeObject == null) blender.serializeObject = CreateInstance<EditorSerializeObject>();
            if (blender.serializeObject.edit && blender.serializeObject.selection >= 0 && blender.serializeObject.selection < blender.sequenceCount && editorWindow == null) OpenEditorWindow(blender.GetSequence(blender.serializeObject.selection));
            if (playIcon == null) playIcon = ImageDB.GetImage("play.png", "Blenda/Editor/Images");
            if (pauseIcon == null) pauseIcon = ImageDB.GetImage("pause.png", "Blenda/Editor/Images");
            if (stopIcon == null) stopIcon = ImageDB.GetImage("stop.png", "Blenda/Editor/Images");
            playContent = new GUIContent(playIcon);
            pauseContent = new GUIContent(pauseIcon);
            stopContent = new GUIContent(stopIcon);
            Undo.undoRedoPerformed += UndoRedo;
        }

        void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoRedo;
            AudioBlender blender = (AudioBlender)target;
            blender.serializeObject.edit = false;
        }

        void UndoRedo()
        {
            AudioBlender blender = (AudioBlender)target;
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            AudioBlender blender = (AudioBlender)target;
            Undo.RecordObject(blender, "Edit Audio Blender");
            blender.serializeObject.edit = editorWindow != null;
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            if (Application.isPlaying)
            {
                EditorGUILayout.BeginHorizontal();
                if (BlendaEditorGUI.EditorLayoutSelectableButton(new GUIContent(playContent), true, blender.isPlaying)) blender.Play();
                if (BlendaEditorGUI.EditorLayoutSelectableButton(new GUIContent(pauseContent), true, blender.isPaused)) blender.Pause();
                if (BlendaEditorGUI.EditorLayoutSelectableButton(new GUIContent(stopContent))) blender.Stop();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();

            for (int i = 0; i < blender.sequenceCount; i++)
            {
                if (SequenceField(blender.GetSequence(i), blender.serializeObject.selection == i))
                {
                    Repaint();
                    renameSequence = null;
                    blender.serializeObject.selection = i;
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.Space();
            if (GUILayout.Button("Add Sequence"))
            {
                blender.AddSequence("Sequence " + (blender.sequenceCount + 1));
            }
            if (EditorGUI.EndChangeCheck() && editorWindow != null) editorWindow.Repaint();
        }

        void OnRename(object o)
        {
            renameSequence = (Sequence)o;
        }

        void OnDuplicate(object o)
        {
            Sequence source = (Sequence)o;
            source.Copy();
            Repaint();
        }

        void OnDelete(object o)
        {
            Sequence sequence = (Sequence)o;
            if (!EditorUtility.DisplayDialog("Delete Sequence", "Are you sure you want to delete sequence " + sequence.name + "?", "Yes", "No")) return;
            AudioBlender blender = (AudioBlender)target;
            Undo.RegisterCompleteObjectUndo(blender, "Delete sequence");
            blender.RemoveSequence(sequence);
            Undo.FlushUndoRecordObjects();
            Repaint();
        }


        bool SequenceField(Sequence sequence, bool expand)
        {
            GUI.backgroundColor = BlendaEditorGUI.lightColor;
            bool clicked = false;


            if (renameSequence == sequence && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                renameSequence = null;
                Repaint();
            }
            
            if(boxStyle == null)
            {
                boxStyle = new GUIStyle(GUI.skin.GetStyle("box"));
                boxStyle.normal.background = BlendaEditorGUI.bgImg;
            }

            EditorGUILayout.BeginVertical(boxStyle);
            if (renameSequence == sequence) sequence.name = EditorGUILayout.TextField(sequence.name);
            else
            {
                if(expand) GUILayout.Label(sequence.name, EditorStyles.boldLabel);
                else GUILayout.Label(sequence.name);

                if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Rename"), false, OnRename, sequence);
                    menu.AddItem(new GUIContent("Duplicate"), false, OnDuplicate, sequence);
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete"), false, OnDelete, sequence);
                    menu.ShowAsContext();
                }
            }

            Rect progressRect = GUILayoutUtility.GetRect(GUILayoutUtility.GetLastRect().width, 4f);
            EditorGUI.DrawRect(progressRect, BlendaEditorGUI.darkColor);
            EditorGUI.DrawRect(new Rect(progressRect.x, progressRect.y, progressRect.width * (sequence.time / sequence.duration), progressRect.height), BlendaEditorGUI.highlightColor);

            if (expand)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                sequence.time = EditorGUILayout.Slider("Time", sequence.time, 0f, sequence.duration);
                sequence.duration = EditorGUILayout.FloatField("Duration", sequence.duration);
                EditorGUILayout.Space();

                sequence.playTrigger = (Sequence.PlayTrigger)EditorGUILayout.EnumPopup("Play Trigger", sequence.playTrigger);
                sequence.playDelay = EditorGUILayout.FloatField("Delay", sequence.playDelay);
                EditorGUILayout.Space();

                sequence.type = (Sequence.WrapMode)EditorGUILayout.EnumPopup("Type", sequence.type);
                if (sequence.type == Sequence.WrapMode.Loop)
                {
                    sequence.loopCount = EditorGUILayout.IntField("Loops " + (sequence.loopCount == 0 ? "(infinite)" : ""), sequence.loopCount);
                    if (sequence.loopCount < 0) sequence.loopCount = 0;
                }
                EditorGUILayout.Space();
                sequence.volume = EditorGUILayout.Slider("Volume", sequence.volume, 0f, 1f);
                sequence.pitch = EditorGUILayout.Slider("Pitch", sequence.pitch, 0.01f, 3f);
                EditorGUILayout.Space();


                sequence.fadeIn = EditorGUILayout.FloatField("Fade-in", sequence.fadeIn);
                if (sequence.fadeIn > 0f) sequence.fadeInCurve = EditorGUILayout.CurveField("Fade-in Curve", sequence.fadeInCurve);

                sequence.fadeOut = EditorGUILayout.FloatField("Fade-out", sequence.fadeOut);
                if (sequence.fadeOut > 0f) sequence.fadeOutCurve = EditorGUILayout.CurveField("Fade-out Curve", sequence.fadeOutCurve);

                EditorGUILayout.Space();
                sequence.panTracks = EditorGUILayout.Toggle("Pan Tracks Volume", sequence.panTracks);
                if (sequence.panTracks)
                {
                    sequence.trackPanPercent = EditorGUILayout.Slider("Percent", sequence.trackPanPercent, 0f, 1f);
                    sequence.panRange = EditorGUILayout.FloatField("Range", sequence.panRange);
                    sequence.trackPanFalloff = EditorGUILayout.CurveField("Falloff", sequence.trackPanFalloff);
                }

                if (GUILayout.Button("Open Editor")) OpenEditorWindow(sequence);

                if (Application.isPlaying)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (BlendaEditorGUI.EditorLayoutSelectableButton(new GUIContent(playContent), true, sequence.isPlaying))
                    {
                        sequence.Play();
                    }
                    if (BlendaEditorGUI.EditorLayoutSelectableButton(new GUIContent(pauseContent), true, sequence.isPaused))
                    {
                        sequence.Pause();
                    }
                    if (BlendaEditorGUI.EditorLayoutSelectableButton(new GUIContent(stopContent)))
                    {
                        sequence.Stop();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            } 

            EditorGUILayout.EndVertical();
            if (!expand)
            {
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    if(Event.current.type == EventType.MouseDown && Event.current.button == 0) clicked = true;
                }
            }
            GUI.backgroundColor = Color.white;
            return clicked;
        }

        void OpenEditorWindow(Sequence sequence)
        {
            if (editorWindow != null) editorWindow.Close();
            editorWindow = EditorWindow.GetWindow<BlenderEditorWindow>(true);
            editorWindow.sequence = sequence;
        }
    }
}
