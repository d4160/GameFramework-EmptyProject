using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamteck.Blenda
{
    public partial class Sequence
    {
        [System.Serializable]
        public partial class AudioTrack
        {
            [SerializeField]
            [HideInInspector]
            private List<BlendClip> clips = new List<BlendClip>();
            public string name = "Track";
            public bool mute = false;

            [Range(0f, 1f)]
            public float volume = 1f;

            public int clipCount
            {
                get { return clips.Count; }
            }

            [System.NonSerialized]
            [HideInInspector]
            internal Sequence _sequence = null;

            public Sequence sequence
            {
                get { return _sequence; }
            }

            private float lastTime = 0f;
            internal float panBlend = 1f;

            internal AudioTrack(Sequence s)
            {
                _sequence = s;
            }

            public AudioTrack()
            {
            }

            internal AudioTrack(Sequence s, string n)
            {
                _sequence = s;
                name = n;
            }

            internal void Init(Sequence s)
            {
                _sequence = s;
                for (int i = 0; i < clips.Count; i++) clips[i].Init(this, false);
            }

            public void UpdateClips()
            {
                for (int i = 0; i < clips.Count; i++) clips[i]._track = this;
            }

            internal void HandleLoop(float newTime)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i].playType == BlendClip.PlayType.Oneshot) HandleOneshot(clips[i], _sequence.duration, lastTime);
                    if (mute) clips[i].source.volume = 0f;
                }

                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i].playType == BlendClip.PlayType.Oneshot) HandleOneshot(clips[i], newTime, 0f);
                    if (mute) clips[i].source.volume = 0f;
                }
                lastTime = newTime;
            }

            internal void Update(float time, float duration)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    switch (clips[i].playType)
                    {
                        case BlendClip.PlayType.Default:
                            HandleClip(clips[i], time);
                            break;

                        case BlendClip.PlayType.Loop:
                            HandleClip(clips[i], time);
                            break;

                        case BlendClip.PlayType.Oneshot:
                            HandleOneshot(clips[i], time, lastTime);
                            break;
                    }
                    if (mute) clips[i].source.volume = 0f;
                }
                lastTime = time;
            }

            void HandleClip(BlendClip clip, float time)
            {
                float percent = 0f;
                if (clip.Evaluate(time, out percent))
                {
                    if (!clip.isPlaying) clip.Play(percent);
                    clip.HandleVolume(sequence.fade * sequence.volume * sequence.blender.masterVolume * panBlend * volume, percent);
                    clip.HandlePitch(sequence.pitch * sequence.blender.masterPitch, percent);
                }
                else
                {
                    if (clip.isPlaying) clip.Pause();
                }
            }

            void HandleOneshot(BlendClip clip, float time, float lastTime)
            {
                float anchor = Mathf.Lerp(clip.start, clip.end, clip.oneshotAnchor);
                if (!clip.isPlaying)
                {
                    if ((lastTime < anchor && time >= anchor) || (lastTime > anchor && time <= anchor)) clip.Play(0f);
                }
                else
                {
                    clip.HandleVolume(sequence.fade * sequence.volume * sequence.blender.masterVolume * panBlend, clip.source.time / clip.audioClip.length * volume);
                    clip.HandlePitch(sequence.pitch * sequence.blender.masterPitch, clip.source.time / clip.audioClip.length);
                }
            }

            public void ClearClips()
            {
                for (int i = 0; i < clips.Count; i++) clips[i].Destroy();
                clips.Clear();
            }

            /// <summary>
            /// Adds an existing clip to this track. If the clip belongs to another track it will be removed from the other track
            /// </summary>
            /// <param name="clip"></param>
            public void AddClip(BlendClip clip)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i] == clip) return;
                }
                if (clip.track != null) clip.track.RemoveClip(clip);
                clip.Init(this, false, true);
                clips.Add(clip);
            }

            /// <summary>
            /// Creates a new clip from an AudioClip and adds it to the track
            /// </summary>
            public BlendClip AddClip(AudioClip clip, float time)
            {
                BlendClip c = new BlendClip(clip, time, this);
                clips.Add(c);
                return c;
            }

            /// <summary>
            /// Removes a clip by index permanently
            /// </summary>
            /// <param name="index"></param>
            public void RemoveClip(int index)
            {
                if (index < 0 || index >= clips.Count) throw new UnityException("Clip " + index + " does not exist");
#if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(sequence.blender, "Delete Clips");
#endif
                BlendClip clip = clips[index];
                clips.RemoveAt(index);
#if UNITY_EDITOR
                clip.Destroy();
#endif

            }

            /// <summary>
            /// Removes a clip by reference permanently
            /// </summary>
            /// <param name="clip"></param>
            public void RemoveClip(BlendClip clip)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i] == clip)
                    {
                        RemoveClip(i);
                        break;
                    }
                }
            }

            /// <summary>
            /// Removes a clip by audio clip reference permanently
            /// </summary>
            /// <param name="clip"></param>
            public void RemoveClip(AudioClip clip)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i].audioClip == clip)
                    {
                        RemoveClip(i);
                        break;
                    }
                }
            }

            /// <summary>
            /// Removes a clip from the track without destroying it. Used when moving clips to other tracks
            /// </summary>
            /// <param name="index"></param>
            public void UnlinkClip(int index)
            {
                if (index < 0 || index >= clips.Count) throw new UnityException("Clip " + index + " does not exist");
                clips.RemoveAt(index);
            }

            /// <summary>
            /// Removes a clip from the track without destroying it. Used when moving clips to other tracks
            /// </summary>
            /// <param name="index"></param>
            public void UnlinkClip(BlendClip clip)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i] == clip)
                    {
                        UnlinkClip(i);
                        break;
                    }
                }
            }

            /// <summary>
            /// Removes a clip from the track without destroying it. Used when moving clips to other tracks
            /// </summary>
            /// <param name="index"></param>
            public void UnlinkClip(AudioClip clip)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i].audioClip == clip)
                    {
                        UnlinkClip(i);
                        break;
                    }
                }
            }

            public BlendClip GetClip(int index)
            {
                return clips[index];
            }

            public BlendClip GetClip(AudioClip audioClip)
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i].audioClip == audioClip) return clips[i];
                }
                return null;
            }

            internal void Stop()
            {
                for (int i = 0; i < clips.Count; i++) clips[i].Stop();
            }

            internal void Pause()
            {
                for (int i = 0; i < clips.Count; i++) clips[i].Pause();
            }

            internal void Play(float time)
            {
                lastTime = time;
            }

            internal AudioTrack Copy()
            {
                AudioTrack target = new AudioTrack();
                target.name = name;
                target.mute = mute;
                target.panBlend = panBlend;
                target._sequence = _sequence;
                target.clips = new List<BlendClip>();
                for (int i = 0; i < clips.Count; i++)
                {
                    target.AddClip(clips[i].Copy());
                   // target.clips[target.clips.Count - 1].Init(target, false, true);
                }
                return target;
            }
        }
    }
}
