﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2016/04/13 11:11
// License Copyright (c) Daniele Giardini

using System;
using DG.DeAudio;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;

namespace DG.DeAudioEditor
{
    /// <summary>
    /// GUILayout methods for drawing DeAudio data objects
    /// </summary>
    public static class DeAudioGUILayout
    {
        #region Public Methods
        
        public static DeAudioClipData DeAudioClip(string label, DeAudioClipData value, bool allowGroupChange = true, DeAudioClipGUIMode mode = DeAudioClipGUIMode.Full)
        { return DeAudioClip(new GUIContent(label, ""), value, allowGroupChange, mode); }
        public static DeAudioClipData DeAudioClip(GUIContent label, DeAudioClipData value, bool allowGroupChange = true, DeAudioClipGUIMode mode = DeAudioClipGUIMode.Full)
        {
            Styles.Init();

            GUIStyle style;
            switch (mode) {
            case DeAudioClipGUIMode.ClipOnly:
            case DeAudioClipGUIMode.Compact:
            case DeAudioClipGUIMode.CompactPreviewOnly:
            case DeAudioClipGUIMode.CompactWithGroupAndPreview:
                style = Styles.oneRow;
                break;
            case DeAudioClipGUIMode.VolumeWithPreview:
            case DeAudioClipGUIMode.VolumeAndLoopsWithPreview:
                style = Styles.twoRows;
                break;
            default:
                style = Styles.threeRows;
                break;
            }

            Rect r = GUILayoutUtility.GetRect(GUIContent.none, style);
            return DeAudioGUI.DeAudioClip(r, label, value, allowGroupChange, mode);

//            using (new GUILayout.HorizontalScope()) {
//                value.clip = EditorGUILayout.ObjectField(label, value.clip, typeof(AudioClip), false) as AudioClip;
//                if (mode != DeAudioClipGUIMode.ClipOnly) {
//                    if (mode != DeAudioClipGUIMode.CompactPreviewOnly) {
//                        using (new EditorGUI.DisabledGroupScope(!allowGroupChange)) {
//                            DeAudioGroupId newGroupId = (DeAudioGroupId)EditorGUILayout.EnumPopup(value.groupId, GUILayout.Width(78));
//                            if (allowGroupChange) value.groupId = newGroupId;
//                        }
//                    }
//                    if (mode == DeAudioClipGUIMode.CompactPreviewOnly || mode == DeAudioClipGUIMode.CompactWithGroupAndPreview) {
//                        DeAudioGUI.PlayButton(value);
//                        DeAudioGUI.StopButton();
//                    }
//                }
//            }
//            if (mode == DeAudioClipGUIMode.CompactPreviewOnly) GUILayout.Space(4);
//            if (mode == DeAudioClipGUIMode.VolumeWithPreview || mode == DeAudioClipGUIMode.VolumeAndLoopsWithPreview || mode == DeAudioClipGUIMode.Full) {
//                // Volume, pitch, play/pause/loop buttons
//                GUILayout.Space(-1);
//                using (new GUILayout.HorizontalScope()) {
//                    value.volume = EditorGUILayout.Slider("└ Volume", value.volume, 0, 1);
//                    if (mode == DeAudioClipGUIMode.VolumeAndLoopsWithPreview) DeAudioGUI.LoopToggle(value);
//                    DeAudioGUI.PlayButton(value);
//                    DeAudioGUI.StopButton();
//                }
//                if (mode == DeAudioClipGUIMode.Full) {
//                    GUILayout.Space(-1);
//                    using (new GUILayout.HorizontalScope()) {
//                        value.pitch = EditorGUILayout.Slider("└ Pitch", value.pitch, 0, 3);
//                        DeAudioGUI.LoopToggle(value);
//                    }
//                } else GUILayout.Space(4);
//            } else if (mode == DeAudioClipGUIMode.CompactWithGroupAndPreview) GUILayout.Space(4);
//            return value;
        }

        #endregion

//        #region GUI Helpers
//
//        static class DeAudioGUI
//        {
//            public static void LoopToggle(DeAudioClipData value)
//            {
//                value.loop = DeGUILayout.ToggleButton(value.loop, "Loop", GUILayout.Width(44), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight - 1));
//            }
//
//            public static void PlayButton(DeAudioClipData value)
//            {
//                if (GUILayout.Button("►", DeGUI.styles.button.tool, GUILayout.Width(22), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight))) {
//                    DeEditorSoundUtils.StopAll();
//                    if (value.clip != null) DeEditorSoundUtils.Play(value.clip);
//                }
//            }
//
//            public static void StopButton()
//            {
//                if (GUILayout.Button("■", DeGUI.styles.button.tool, GUILayout.Width(22), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight))) {
//                    DeEditorSoundUtils.StopAll();
//                }
//            }
//        }
//
//        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        static class Styles
        {
            static bool _initialized;
            public static GUIStyle oneRow, twoRows, threeRows;

            public static void Init()
            {
                if (_initialized) return;

                _initialized = true;

                oneRow = new GUIStyle().StretchWidth().Height((int)EditorGUIUtility.singleLineHeight);
                twoRows = oneRow.Clone().Height((int)EditorGUIUtility.singleLineHeight * 2 + DeAudioGUI.VSpace);
                threeRows = oneRow.Clone().Height((int)EditorGUIUtility.singleLineHeight * 3 + DeAudioGUI.VSpace * 2);
            }
        }
    }
}