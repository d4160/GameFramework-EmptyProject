using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dreamteck.Blenda
{
    [System.Serializable]
    public partial class Sequence
    {
        public enum PlayTrigger {None, Awake, WithPrevious, AfterPrevious }
        public PlayTrigger playTrigger = PlayTrigger.Awake;
        public float playDelay = 0f;
        public enum WrapMode { Default, Loop, Custom }
        public WrapMode type = WrapMode.Default;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (_parent != null) _parent.name = value;
            }
        }
        [HideInInspector]
        [SerializeField]
        private string _name = "";
        public int trackCount
        {
            get { return tracks.Count; }
        }
        [HideInInspector]
        [SerializeField]
        private List<AudioTrack> tracks = new List<AudioTrack>();
        public int loopCount = 0;

        public float volume = 1f;
        public float pitch = 1f;
        private int loops = 0;
        [SerializeField]
        [HideInInspector]
        private float _time = 0f;
        public float time
        {
            get { return _time; }
            set
            {
                if (play && type != WrapMode.Custom) return;
                _time = value;
            }
        }
        public float normalizedTime
        {
            get { return Mathf.Clamp01(_time / duration); }
            set
            {
                if (play && type != WrapMode.Custom) return;
                _time = value * duration;
            }
        }
        public float duration = 3f;

        internal float fade
        {
            get
            {
                if (_done && fadeOut > 0f) return fadeOutCurve.Evaluate(1f- fadePercent);
                if (fadeIn <= 0f) return 1f;
                return fadeInCurve.Evaluate(fadePercent);
            }
        }


        public float fadeIn = 0f;
        public AnimationCurve fadeInCurve = new AnimationCurve();
        public float fadeOut = 0f;
        public AnimationCurve fadeOutCurve = new AnimationCurve();
        private float fadePercent = 0f;


        public bool panTracks = false;
        [Range(0f, 1f)]
        public float trackPanPercent = 0f;
        public AnimationCurve trackPanFalloff = new AnimationCurve();
        public float panRange = 1f;


        public bool isActive
        {
            get { return play && !_done; }
        }

        public bool isPlaying
        {
            get { return play; }
        }

        public bool isPaused
        {
            get { return pause; }
        }

        private bool play = false;

        private bool pause = false;

        private bool _done = false;
        public bool isDone
        {
            get {
                return _done;
            }
        }

        private float playDelayAccum = 0f;

        [HideInInspector]
        [SerializeField]
        private Transform _parent = null;
        public Transform parent
        {
            get { return _parent; }
        }

        public AudioBlender blender
        {
            get
            {
                return _blender;
            }
        }
        [SerializeField]
        [HideInInspector]
        private AudioBlender _blender = null;

        public Sequence()
        {
        
        }

        internal Sequence(string n, AudioBlender b)
        {
            name = n;
            _blender = b;
            CreateParent(blender.transform, n);
            AddTracks(1);
            Keyframe[] keys = new Keyframe[2];
            keys[0] = new Keyframe(0f, 0f, 2f, 2f);
            keys[1] = new Keyframe(1f, 1f, 0f, 0f);
            fadeInCurve = new AnimationCurve(keys);
            keys[0] = new Keyframe(0f, 1f, 0f, 0f);
            keys[1] = new Keyframe(1f, 0f, -2f, -2f);
            fadeOutCurve = new AnimationCurve(keys);
        }

        void CreateParent(Transform root, string n)
        {
            GameObject go = new GameObject(n);
            go.transform.parent = root;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            _parent = go.transform;
        }

        internal void Init(AudioBlender b)
        {
            _blender = b;
            if (_parent == null) CreateParent(b.transform, name);
            if (_parent.parent != blender.transform) _parent.parent = b.transform;
            for (int i = 0; i < tracks.Count; i++) tracks[i].Init(this);
        }

        public void Stop()
        {
            if(type != WrapMode.Custom) _time = 0f;
            loops = 0;
            _done = false;
            play = false;
            pause = false;
            for (int i = 0; i < tracks.Count; i++) tracks[i].Stop();
        }

        public void Finish(bool bypassFade = false)
        {
            if (_done) return;
            fadePercent = 1f;
            _done = true;
            play = false;
            pause = false;
        }

        public void Pause()
        {
            if (!play) return;
            pause = true;
            play = false;
            for (int i = 0; i < tracks.Count; i++) tracks[i].Pause();
        }

        public void Play(bool bypassFade = false)
        {
            for (int i = 0; i < tracks.Count; i++) tracks[i].Play(_time);
            if (!play) playDelayAccum = playDelay;
            play = true;
            pause = false;
            if (bypassFade) fadePercent = 1f;
            else fadePercent = 0f;
        }

        internal void Update()
        {
            if (!play)
            {
                if (fadeOut <= 0f || fadePercent <= 0f)
                {
                    for (int i = 0; i < tracks.Count; i++) tracks[i].Stop();
                    return; //Stop playing only if fade out is zero
                }
            }
            if (_done)
            {
                if (fadeOut <= 0f || fadePercent <= 0f)
                {
                    for (int i = 0; i < tracks.Count; i++) tracks[i].Stop();
                    return; //Stop playing only if fade out is zero
                }
            }
            if (playDelayAccum > 0f) playDelayAccum -= Time.deltaTime;
            if (playDelayAccum > 0f) return;

            if (_done && fadeOut > 0f) fadePercent = Mathf.MoveTowards(fadePercent, 0f, Time.deltaTime / fadeOut);
            else if (fadeIn > 0f) fadePercent = Mathf.MoveTowards(fadePercent, 1f, Time.deltaTime / fadeIn);

            if (panTracks)
            {
                float trackBlendCenter = trackPanPercent * (tracks.Count - 1);
                for (int i = 0; i < tracks.Count; i++)
                {
                    if (panRange == 0f) tracks[i].panBlend = 0f;
                    else tracks[i].panBlend = trackPanFalloff.Evaluate(1f - Mathf.Abs(i - trackBlendCenter) / panRange);
                }
            } else
            {
                for (int i = 0; i < tracks.Count; i++) tracks[i].panBlend = 1f;
            }
            if (type != WrapMode.Custom && !_done && play) 
            {
                _time += Time.deltaTime * pitch * blender.masterPitch;
                if (_time > duration)
                {
                    if (type == WrapMode.Loop)
                    {
                        while (_time > duration)
                        {
                            if (loopCount > 0 && loops == loopCount) break;
                            _time -= duration;
                            loops++;
                        }
                        if(loopCount > 0 && loops == loopCount)
                        {
                            Finish();
                        }
                        for (int i = 0; i < tracks.Count; i++) tracks[i].HandleLoop(_time);
                    }
                    else
                    {
                        _time = duration;
                        Finish();
                    }
                }
            }
            for (int i = 0; i < tracks.Count; i++) tracks[i].Update(_time, duration);
        }


        internal void Destroy()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) GameObject.Destroy(parent.gameObject);
            else Undo.DestroyObjectImmediate(parent.gameObject);
#else
            GameObject.Destroy(parent.gameObject);
#endif
        }

        public AudioTrack AddTrack(string name)
        {
            tracks.Add(new AudioTrack(this, name));
            return tracks[tracks.Count - 1];
        }

        public AudioTrack[] AddTracks(int count)
        {
            if (count <= 0) throw new UnityException("Cannot add a negative number of tracks: " + count);
            AudioTrack[] addTracks = new AudioTrack[count];
            for (int i = 0; i < count; i++) addTracks[i] = new AudioTrack(this);
            tracks.AddRange(new List<AudioTrack>(addTracks));
            return addTracks;
        }

        public void RemoveTrack(string name)
        {
            for (int i = 0; i < tracks.Count; i++)
            {
                if (tracks[i].name == name)
                {
                    RemoveTrack(i);
                    break;
                }
            }
        }

        public void RemoveTrack(int index)
        {
            if (index < 0 || index >= tracks.Count) throw new UnityException("Track " + index + " does not exist");
            tracks[index].ClearClips();
            tracks.RemoveAt(index);
        }

        public void RemoveTrack(AudioTrack track)
        {
            for (int i = 0; i < tracks.Count; i++)
            {
                if (track == tracks[i])
                {
                    RemoveTrack(i);
                    break;
                }
            }
        }

        public AudioTrack GetTrack(int index)
        {
            if (index < 0 || index >= tracks.Count) return null;
            return tracks[index];
        }

        public AudioTrack GetTrack(string name)
        {
            for (int i = 0; i < tracks.Count; i++)
            {
                if (tracks[i].name == name) return tracks[i];
            }
            return null;
        }

        public Sequence Copy()
        {
            Sequence target = new Sequence();
            target.name = name;
            target.duration = duration;
            target.fadeIn = fadeIn;
            target.fadeInCurve = DuplicateUtility.DuplicateCurve(fadeInCurve);
            target.fadePercent = fadePercent;
            target.loopCount = loopCount;
            target.panRange = panRange;
            target.panTracks = panTracks;
            target.pitch = pitch;
            target.playDelay = playDelay;
            target.playTrigger = playTrigger;
            target.trackPanFalloff = DuplicateUtility.DuplicateCurve(trackPanFalloff);
            target.trackPanPercent = trackPanPercent;
            target.type = type;
            target.volume = volume;
            blender.sequences.Add(target);
            for (int i = 0; i < tracks.Count; i++) target.tracks.Add(tracks[i].Copy());
            target.Init(_blender);
            return target;
        }
    }
}
