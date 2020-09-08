using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dreamteck.Blenda
{
    public partial class Sequence
    {
        public partial class AudioTrack
        {
            [System.Serializable]
            public class BlendClip
            {
                public AudioClip audioClip
                {
                    get { return _clip; }
                    set
                    {
                        if (_clip == null && value != null && playType == PlayType.Oneshot) duration = value.length;
                        _clip = value;
                        if (source != null)
                        {
                            source.clip = _clip;
                            source.name = _clip.name;
                        }
                    }
                }

                public float start = 0f;
                public float end
                {
                    get { return start + _duration; }
                }

                public float duration
                {
                    get { return _duration; }
                    set
                    {
                        if (value < 0f) value = 0f;
                        if (value < _fadeIn + _fadeOut)
                        {
                            _fadeIn = (_fadeIn / duration) * value;
                            _fadeOut = (_fadeOut / duration) * value;
                        }
                        _duration = value;
                    }
                }

                public float fadeIn
                {
                    get { return _fadeIn; }
                    set
                    {
                        if (value < 0f) value = 0f;
                        _fadeIn = value;
                        if (_fadeIn > _duration) _fadeIn = _duration;
                        if (_duration - _fadeIn < _fadeOut) _fadeOut = _duration - _fadeIn;
                    }
                }

                public float fadeOut
                {
                    get { return _fadeOut; }
                    set
                    {
                        if (value < 0f) value = 0f;
                        _fadeOut = value;
                        if (_fadeOut > _duration) _fadeOut = _duration;
                        if (_duration - _fadeOut < _fadeIn) _fadeIn = _duration - _fadeOut;
                    }
                }

                public float volume = 1f;
                public float pitch = 1f;

                [SerializeField]
                [HideInInspector]
                protected float _duration = 0;
                [SerializeField]
                [HideInInspector]
                protected float _fadeIn = 0f;
                [SerializeField]
                [HideInInspector]
                protected float _fadeOut = 0f;
                public AnimationCurve fadeInCurve = new AnimationCurve();
                public AnimationCurve fadeOutCurve = new AnimationCurve();

                public enum PlayType { Default, Loop, Oneshot }
                public PlayType playType
                {
                    get { return _playType; }
                    set
                    {
                        if (source != null) source.loop = value == PlayType.Loop;
                        if (value == PlayType.Oneshot && audioClip != null && duration > audioClip.length) duration = audioClip.length;
                        _playType = value;
                    }
                }

                public enum StartTimeMode { Default, Custom, Random }
                public StartTimeMode startTimeMode = StartTimeMode.Default;
                public float startOffset = 0f;
                public float minRandomStartTime = 0f;
                public float maxRandomStartTime = 1f;
                [SerializeField]
                [HideInInspector]
                internal AudioSource source;

                public bool usePitchEnvelope = false;
                public bool useVolumeEnvelope = false;
                public AnimationCurve pitchEnvelope = new AnimationCurve();
                public AnimationCurve volumeEnvelope = new AnimationCurve();
                public float oneshotAnchor = 0f;
                private bool paused = false;

                public GameObject gameObject
                {
                    get { return source.gameObject; }
                }

                [System.NonSerialized]
                [HideInInspector]
                internal AudioTrack _track = null;
                public AudioTrack track
                {
                    get { return _track; }
                }

                [SerializeField]
                [HideInInspector]
                private AudioClip _clip;
                [SerializeField]
                [HideInInspector]
                private PlayType _playType = PlayType.Default;

                public BlendClip()
                {

                }

                internal BlendClip(AudioTrack t)
                {
                    Init(t, true);
                }

                internal BlendClip(AudioClip clip, float pos, AudioTrack t)
                {
                    _clip = clip;
                    start = pos;
                    Keyframe[] keys = new Keyframe[2];
                    keys[0] = new Keyframe(0f, 0f, 2f, 2f);
                    keys[1] = new Keyframe(1f, 1f, 0f, 0f);
                    fadeInCurve = new AnimationCurve(keys);
                    keys[0] = new Keyframe(0f, 1f, 0f, 0f);
                    keys[1] = new Keyframe(1f, 0f, -2f, -2f);
                    fadeOutCurve = new AnimationCurve(keys);
                    _track = t;
                    Init(t, true);
                }

                internal void Init(AudioTrack t, bool resetDuration, bool forceRefresh = false)
                {
                    _track = t;
                    if (_clip == null || forceRefresh)
                    {
                        if (source != null)
                        {

                            if (!HasOtherComponents(source.gameObject))
                            {
#if UNITY_EDITOR
                                if (Application.isPlaying) UnityEngine.Object.Destroy(source);
                                else UnityEngine.Object.DestroyImmediate(source);
#else
                        UnityEngine.Object.Destroy(source);    
#endif
                            }
                            else
                            {
#if UNITY_EDITOR
                                if (Application.isPlaying) UnityEngine.Object.Destroy(source.gameObject);
                                else UnityEngine.Object.DestroyImmediate(source.gameObject);
#else
                        UnityEngine.Object.Destroy(source.gameObject);    
#endif
                            }
                        }
                        return;
                    }
                    if (source == null || forceRefresh)
                    {
                        GameObject go = new GameObject(_clip.name);
#if UNITY_EDITOR
                        if(!Application.isPlaying) Undo.RegisterCreatedObjectUndo(go, "Add Clip " + _clip.name);
#endif
                        go.transform.parent = _track._sequence.parent;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = Vector3.one;
                        go.transform.localRotation = Quaternion.identity;
                        source = go.AddComponent<AudioSource>();
                    }
                    source.clip = _clip;
                    source.playOnAwake = false;
                    source.loop = _playType == PlayType.Loop;
                    if (resetDuration) _duration = audioClip.length;
                }

                public BlendClip Copy()
                {
                    BlendClip newClip = new BlendClip(_track);
                    newClip.audioClip = audioClip;
                    newClip.start = start;
                    newClip.duration = _duration;
                    newClip.fadeIn = _fadeIn;
                    newClip.fadeOut = _fadeOut;
                    newClip.playType = _playType;
                    newClip.startOffset = startOffset;
                    newClip.minRandomStartTime = minRandomStartTime;
                    newClip.maxRandomStartTime = maxRandomStartTime;
                    newClip.startTimeMode = startTimeMode;
                    newClip.volume = volume;
                    newClip.pitch = pitch;
                    newClip.usePitchEnvelope = usePitchEnvelope;
                    newClip.useVolumeEnvelope = useVolumeEnvelope;

                    if (fadeInCurve != null) newClip.fadeInCurve = DuplicateUtility.DuplicateCurve(fadeInCurve);
                    if (fadeOutCurve != null) newClip.fadeOutCurve = DuplicateUtility.DuplicateCurve(fadeOutCurve);
                    if (pitchEnvelope != null) newClip.pitchEnvelope = DuplicateUtility.DuplicateCurve(pitchEnvelope);
                    if (volumeEnvelope != null) newClip.volumeEnvelope = DuplicateUtility.DuplicateCurve(volumeEnvelope);
                    return newClip;
                }

                bool HasOtherComponents(GameObject go)
                {
                    Component[] components = source.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        if (component is Transform) continue;
                        if (component is AudioSource) continue;
                        return true;
                    }
                    return false;
                }

                internal bool Evaluate(float time, out float percent)
                {
                    bool result = false;
                    percent = 0f;
                    float offsettedStart = start;
                    float offsettedEnd = end;
                    if (playType == PlayType.Default)
                    {
                        offsettedStart += startOffset;
                        if (offsettedStart < start) offsettedStart = start;
                        if (offsettedStart > end) offsettedStart = end;
                        offsettedEnd = start + startOffset + audioClip.length / pitch;
                        if (offsettedEnd > end) offsettedEnd = end;
                        if (offsettedEnd < start) offsettedEnd = start;
                    }
                    if (offsettedEnd == offsettedStart) return false;
                    if (end > track.sequence.duration && playType == PlayType.Loop)
                    {
                        if (time >= offsettedStart || time <= offsettedEnd - track.sequence.duration)
                        {
                            if (time > start) percent = Mathf.InverseLerp(start, end, time);
                            else percent = Mathf.InverseLerp(start - track.sequence.duration, end - track.sequence.duration, time);
                            result = true;
                        }
                    }
                    else
                    {
                        if (time >= offsettedStart && time <= offsettedEnd)
                        {
                            percent = Mathf.InverseLerp(start, end, time);
                            result = true;
                        }
                    }
                    return result;
                }

                internal void HandleVolume(float inputVolume, float t)
                {
                    t *= duration;
                    source.volume = inputVolume * volume;
                    if (t < _fadeIn) source.volume *= Mathf.Clamp01(fadeInCurve.Evaluate(Mathf.InverseLerp(0, _fadeIn, t)));
                    else if (t > duration - _fadeOut) source.volume *= Mathf.Clamp01(fadeOutCurve.Evaluate(Mathf.InverseLerp(duration - _fadeOut, duration, t)));
                    if (useVolumeEnvelope) source.volume *= volumeEnvelope.Evaluate(t/duration);
                }

                internal void HandlePitch(float inputPitch, float t)
                {
                    source.pitch = inputPitch * pitch;
                    if (usePitchEnvelope) source.pitch *= pitchEnvelope.Evaluate(t);
                }

                internal void Pause()
                {
                    paused = true;
                    source.Pause();
                }

                internal void Play(float percent)
                {
                    if (paused)
                    {
                        source.UnPause();
                        paused = false;
                    } else source.Play();
                    float t = 0f;
                    float loopedOffset = startOffset;
                    if (playType == PlayType.Loop)
                    {
                        while (loopedOffset < -source.clip.length) loopedOffset += source.clip.length;
                        while (loopedOffset > source.clip.length) loopedOffset -= source.clip.length;
                        if (loopedOffset > 0f) loopedOffset = source.clip.length - loopedOffset;
                        else loopedOffset *= -1f;
                    }

                    if (playType != PlayType.Oneshot)
                    {
                        switch (startTimeMode)
                        {
                            case StartTimeMode.Default:
                                if (playType == PlayType.Loop)
                                {
                                    t = duration * percent * pitch + loopedOffset;
                                    while (t < -source.clip.length) t += source.clip.length;
                                    while (t > source.clip.length) t -= source.clip.length;
                                    source.time = Mathf.Clamp(t, 0f, source.clip.length);
                                }
                                else source.time = Mathf.Clamp(duration * percent * pitch - startOffset, 0f, source.clip.length);
                                break;
                            case StartTimeMode.Custom:
                                if (playType == PlayType.Loop) source.time = Mathf.Clamp(loopedOffset, 0f, source.clip.length);
                                else source.time = Mathf.Clamp(-startOffset, 0f, source.clip.length);
                                break;
                            case StartTimeMode.Random: source.time = UnityEngine.Random.Range(source.clip.length * minRandomStartTime, source.clip.length * maxRandomStartTime); break;
                        }
                    }
                    source.loop = _playType == PlayType.Loop;
                }

                internal void Stop()
                {
                    paused = false;
                    source.Stop();
                }

                internal bool isPlaying
                {
                    get { return source.isPlaying; }
                }

                internal void Destroy()
                {
                    if (source != null) source.Stop();
#if UNITY_EDITOR
                    if (HasOtherComponents(source.gameObject))
                    {
                        if (Application.isPlaying) UnityEngine.Object.Destroy(source);
                        else Undo.DestroyObjectImmediate(source);
                    }
                    else
                    {
                        if (Application.isPlaying) UnityEngine.Object.Destroy(source.gameObject);
                        else Undo.DestroyObjectImmediate(source.gameObject);
                    }
#else
                 if (HasOtherComponents(source.gameObject)) UnityEngine.Object.Destroy(source);
                else UnityEngine.Object.Destroy(source.gameObject);
#endif
                }
            }
           
        }
    }
}
