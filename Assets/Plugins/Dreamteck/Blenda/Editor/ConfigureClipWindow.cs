using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreamteck.Blenda.Editor
{
    public class ConfigureClipWindow : EditorWindow
    {
        BlenderEditorWindow parentWindow = null;
        List<BlendClipEditor> clipEditors = new List<BlendClipEditor>();

        public void Init(BlendClipEditor editor, BlenderEditorWindow window)
        {
            clipEditors.Clear();
            clipEditors.Add(editor);
            parentWindow = window;
            minSize = maxSize = new Vector2(300, 350);
        }

        public void Init(List<BlendClipEditor> editors, BlenderEditorWindow window)
        {
            clipEditors.Clear();
            clipEditors = new List<BlendClipEditor>(editors);
            parentWindow = window;
            minSize = maxSize = new Vector2(300, 350);
            titleContent = new GUIContent("Configure Clip");
        }

        private void Awake()
        {
            Undo.undoRedoPerformed += Refresh;
        }

        private void OnDestroy()
        {
            Undo.undoRedoPerformed -= Refresh;
        }


        void Refresh()
        {
            if (parentWindow != null) parentWindow.Repaint();
        }

        private void OnGUI()
        {
            Undo.RecordObject(parentWindow.sequence.blender, "Change Clip Settings");
            if (parentWindow == null || clipEditors.Count == 0)
            {
                Close();
                return;
            }

            for (int i = 0; i < clipEditors.Count; i++)
            {
                if(clipEditors[i] == null)
                {
                    Close();
                    return;
                }
            }

            AudioClip clip = clipEditors[0].clip.audioClip;
            AudioClip lastClip = clip;
            bool differentClips = false;
            float volume = 0f, pitch = 0f, position = 0f, duration = 0f, startTime = 0f, fadeIn = 0f, fadeOut = 0f, minRandomStartTime = 0f, maxRandomStartTime = 0f, oneshotAnchor = 0f;
            float lastVolume = 0f, lastPitch = 0f, lastPosition = 0f, lastDuration = 0f, lastStartTime = 0f, lastFadeIn = 0f, lastFadeOut = 0f, lastMinRandomStartTime = 0f, lastMaxRandomStartTime = 0f, lastOneshotAnchor = 0f;
            bool usePitchEnvelope = true, useVolumeEnvelope = true;
            Sequence.AudioTrack.BlendClip.StartTimeMode startTimeMode = Sequence.AudioTrack.BlendClip.StartTimeMode.Default;
            Sequence.AudioTrack.BlendClip.PlayType playType = clipEditors[0].clip.playType;
            Sequence.AudioTrack.BlendClip.PlayType lastPlayType = playType;

            for (int i = 0; i < clipEditors.Count; i++)
            {
                volume += clipEditors[i].clip.volume;
                pitch += clipEditors[i].clip.pitch;
                position += clipEditors[i].clip.start;
                duration += clipEditors[i].clip.duration;
                startTime += clipEditors[i].clip.startOffset;
                fadeIn += clipEditors[i].clip.fadeIn;
                fadeOut += clipEditors[i].clip.fadeOut;
                minRandomStartTime += clipEditors[i].clip.minRandomStartTime;
                maxRandomStartTime += clipEditors[i].clip.maxRandomStartTime;
                oneshotAnchor += clipEditors[i].clip.oneshotAnchor;
                if (!differentClips) differentClips = clipEditors[i].clip.audioClip != clipEditors[0].clip.audioClip;
                if (!clipEditors[i].clip.usePitchEnvelope) usePitchEnvelope = false;
                if (!clipEditors[i].clip.useVolumeEnvelope) useVolumeEnvelope = false;
                if (i == 0) startTimeMode = clipEditors[i].clip.startTimeMode;
            }
            volume /= clipEditors.Count;
            pitch /= clipEditors.Count;
            position /= clipEditors.Count;
            duration /= clipEditors.Count;
            startTime /= clipEditors.Count;
            oneshotAnchor /= clipEditors.Count;
            minRandomStartTime /= clipEditors.Count;
            maxRandomStartTime /= clipEditors.Count;
            lastVolume = volume;
            lastPitch = pitch;
            lastPosition = position;
            lastDuration = duration;
            lastStartTime = startTime;
            lastFadeIn = fadeIn;
            lastFadeOut = fadeOut;
            lastMinRandomStartTime = minRandomStartTime;
            lastMaxRandomStartTime = maxRandomStartTime;
            lastOneshotAnchor = oneshotAnchor;

            Sequence.AudioTrack.BlendClip.StartTimeMode lastStartTimeMode = startTimeMode;
            bool lastUsePitchEnvelope = usePitchEnvelope;
            bool lastUseVolumeEnvelope = useVolumeEnvelope;

            EditorGUI.BeginChangeCheck();

            if(!differentClips)
            clip = (AudioClip)EditorGUILayout.ObjectField("Clip", clip, typeof(AudioClip), false);
            if(clip != lastClip)
            {
                for (int i = 0; i < clipEditors.Count; i++)
                {
                    clipEditors[i].clip.audioClip = clip;
                    clipEditors[i].UpdateWaveform();
                }

            }
            playType = (Sequence.AudioTrack.BlendClip.PlayType)EditorGUILayout.EnumPopup("Type", playType);
            if (playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot) oneshotAnchor = EditorGUILayout.Slider("Anchor", oneshotAnchor, 0f, 1f);
       
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
            pitch = EditorGUILayout.Slider("Pitch", pitch, 0.001f, 3f);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            position = EditorGUILayout.FloatField("Position", position);
            if (GUILayout.Button("<", GUILayout.Width(22)))
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.start = 0f;
            }
            if (GUILayout.Button(">", GUILayout.Width(22)))
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.start = parentWindow.sequence.duration - clipEditors[i].clip.duration;
            }
            
            EditorGUILayout.EndHorizontal();

            duration = EditorGUILayout.FloatField("Duration", duration);
            if (clip != null)
            {
                if (GUILayout.Button("Reset Trim"))
                {
                    for (int i = 0; i < clipEditors.Count; i++)
                    {
                        clipEditors[i].clip.duration = clipEditors[i].clip.audioClip.length / clipEditors[i].clip.pitch;
                        clipEditors[i].clip.startOffset = 0f;
                    }
                }
            }
            EditorGUILayout.Space();

            if (clipEditors[0].clip.playType != Sequence.AudioTrack.BlendClip.PlayType.Oneshot)
            {
                startTimeMode = (Sequence.AudioTrack.BlendClip.StartTimeMode)EditorGUILayout.EnumPopup("Start Time", startTimeMode);
                switch (startTimeMode)
                {
                    case Sequence.AudioTrack.BlendClip.StartTimeMode.Random: EditorGUILayout.MinMaxSlider("Random Start Range", ref minRandomStartTime, ref maxRandomStartTime, 0f, 1f); break;
                    default: startTime = EditorGUILayout.FloatField("Start offset (sec.)", startTime); break;
                }
            }
            

            if (EditorGUI.EndChangeCheck()) parentWindow.Repaint();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Fade-in");
            GUILayout.Label("Fade-out");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            fadeIn = EditorGUILayout.FloatField(fadeIn);
            fadeOut = EditorGUILayout.FloatField(fadeOut);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            clipEditors[0].clip.fadeInCurve = EditorGUILayout.CurveField(clipEditors[0].clip.fadeInCurve);
            clipEditors[0].clip.fadeOutCurve = EditorGUILayout.CurveField(clipEditors[0].clip.fadeOutCurve);
            EditorGUILayout.EndHorizontal();


            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < clipEditors.Count; i++)
                {
                    if(i > 0)
                    {
                        clipEditors[i].clip.fadeInCurve.keys = clipEditors[0].clip.fadeInCurve.keys;
                        clipEditors[i].clip.fadeOutCurve.keys = clipEditors[0].clip.fadeOutCurve.keys;
                    }
                    clipEditors[i].RegenerateTextures();
                }
                parentWindow.Repaint();
            }

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            useVolumeEnvelope = EditorGUILayout.Toggle("Volume Envelope", useVolumeEnvelope);
            if (EditorGUI.EndChangeCheck()) parentWindow.Repaint();
            if (useVolumeEnvelope)
            {
                EditorGUI.BeginChangeCheck();
                if (clipEditors[0].clip.volumeEnvelope == null) clipEditors[0].clip.volumeEnvelope = new AnimationCurve();
                clipEditors[0].clip.volumeEnvelope = EditorGUILayout.CurveField(clipEditors[0].clip.volumeEnvelope);
                if (EditorGUI.EndChangeCheck())
                {
                    for (int i = 0; i < clipEditors.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (clipEditors[i].clip.volumeEnvelope == null) clipEditors[i].clip.volumeEnvelope = new AnimationCurve();
                            clipEditors[i].clip.volumeEnvelope.keys = clipEditors[0].clip.volumeEnvelope.keys;
                        }
                        clipEditors[i].RegenerateTextures();
                        parentWindow.Repaint();
                    }
                }
            }
            EditorGUI.BeginChangeCheck();
            usePitchEnvelope = EditorGUILayout.Toggle("Pitch Envelope", usePitchEnvelope);
            if (EditorGUI.EndChangeCheck()) parentWindow.Repaint();

            if (usePitchEnvelope)
            {
                EditorGUI.BeginChangeCheck();
                if (clipEditors[0].clip.pitchEnvelope == null) clipEditors[0].clip.pitchEnvelope = new AnimationCurve();
                clipEditors[0].clip.pitchEnvelope = EditorGUILayout.CurveField(clipEditors[0].clip.pitchEnvelope);
                if (EditorGUI.EndChangeCheck())
                {
                    for (int i = 0; i < clipEditors.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (clipEditors[i].clip.pitchEnvelope == null) clipEditors[i].clip.pitchEnvelope = new AnimationCurve();
                            clipEditors[i].clip.pitchEnvelope.keys = clipEditors[0].clip.pitchEnvelope.keys;
                        }
                        clipEditors[i].RegenerateTextures();
                    }
                    parentWindow.Repaint();
                }
            }

            if (lastUsePitchEnvelope != usePitchEnvelope)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.usePitchEnvelope = usePitchEnvelope;
            }

            if(lastUseVolumeEnvelope != useVolumeEnvelope)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.useVolumeEnvelope = useVolumeEnvelope;
            }

            if (lastPosition != position)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.start = position;
            }

            if (lastDuration != duration)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.duration = duration;
            }

            if (lastVolume != volume)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.volume = volume;
            }

            if (lastPitch != pitch)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.pitch = pitch;
            }

            if (lastStartTime != startTime)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.startOffset = startTime;
            }

            if (lastFadeIn != fadeIn)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.fadeIn = fadeIn;
            }

            if (lastFadeOut != fadeOut)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.fadeOut = fadeOut;
            }

            if(lastStartTimeMode != startTimeMode)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.startTimeMode = startTimeMode;
            }

            if (lastMinRandomStartTime != minRandomStartTime)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.minRandomStartTime = minRandomStartTime;
            }

            if (lastMaxRandomStartTime != maxRandomStartTime)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.maxRandomStartTime = maxRandomStartTime;
            }

            if (lastOneshotAnchor != oneshotAnchor)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.oneshotAnchor = oneshotAnchor;
            }

            if (lastPlayType != playType)
            {
                for (int i = 0; i < clipEditors.Count; i++) clipEditors[i].clip.playType = playType;
            }
        }
    }
}
