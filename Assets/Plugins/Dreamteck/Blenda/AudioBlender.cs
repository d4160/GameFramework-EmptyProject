using UnityEngine;
using System.Collections.Generic;


namespace Dreamteck.Blenda
{
    [AddComponentMenu("Dreamteck/Blenda/AudioBlender")]
    public class AudioBlender : MonoBehaviour
    {
#if UNITY_EDITOR
        [HideInInspector]
        public Editor.EditorSerializeObject serializeObject;
#endif
        [HideInInspector]
        [SerializeField]
        internal List<Sequence> sequences = new List<Sequence>();
        [Range(0f, 1f)]
        public float masterVolume = 1f;
        [Range(0.01f, 3f)]
        public float masterPitch = 1f;
        public bool playOnAwake = true;

        public int sequenceCount
        {
            get { return sequences.Count; }
        }

        public bool isPlaying
        {
            get {
                if (!play) return false;
                for (int i = 0; i < sequences.Count; i++)
                {
                    if (sequences[i].isPlaying) return true;
                }
                return false;
            }
        }

        public bool finished
        {
            get
            {
                for (int i = 0; i < sequences.Count; i++)
                {
                    if (!sequences[i].isDone) return false;
                }
                return true;
            }
        }

        public bool isPaused
        {
            get { return paused; }
        }

        private bool play = false;
        private bool paused = false;

        private void Start()
        {
            Init();
            if (playOnAwake) Play();
        }

        private void Reset()
        {
            AddSequence("Sequence");
        }

#if UNITY_EDITOR
        public void EditorAwake()
        {
            Init();
        }
#endif

        private void Init()
        {
            for (int i = 0; i < sequences.Count; i++) sequences[i].Init(this);
        }

        public void AddSequence(string name)
        {
            sequences.Add(new Sequence(name, this));
            if (sequences.Count == 1) sequences[0].playTrigger = Sequence.PlayTrigger.Awake;
            else sequences[sequences.Count - 1].playTrigger = Sequence.PlayTrigger.AfterPrevious;
        }

        public void RemoveSequence(Sequence sequence)
        {
            sequence.Destroy();
            sequences.Remove(sequence);
        }

        public void RemoveSequence(string name)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i].name == name)
                {
                    sequences[i].Destroy();
                    sequences.RemoveAt(i);
                    i--;
                }
            }
        }

        public void RemoveSequence(int index)
        {
            if (index < 0 || index >= sequences.Count) return;
            sequences[index].Destroy();
            sequences.RemoveAt(index);
        }

        void Update()
        {
            if (!play) return;
            for (int i = 0; i < sequences.Count; i++)
            {
                switch (sequences[i].playTrigger)
                {
                    case Sequence.PlayTrigger.WithPrevious: if(!sequences[i].isPlaying && !sequences[i].isPaused && !sequences[i].isDone && (sequences[i-1].isPlaying || sequences[i - 1].isDone)) sequences[i].Play(); break;
                    case Sequence.PlayTrigger.AfterPrevious: if (!sequences[i].isPlaying && !sequences[i].isPaused && !sequences[i].isDone && sequences[i - 1].isDone) sequences[i].Play(); break;
                }
                sequences[i].Update();
            }
        }

        public Sequence GetSequence(int index)
        {
            if(index < 0 || index >= sequences.Count) return null;
            return sequences[index];
        }

        public Sequence GetSequence(string name)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i].name == name) return sequences[i];
            }
            return null;
        }

        public void PlaySequence(string name)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                if(sequences[i].name == name)
                {
                    if (sequences[i].isPlaying) continue;
                    if(sequences[i].isDone) sequences[i].Stop();
                    sequences[i].Play();
                }
            }
        }

        public void PlaySequence(int index)
        {
            if (index < 0) return;
            if (index >= sequences.Count) return;
            if (sequences[index].isPlaying) return;
            if (sequences[index].isDone) sequences[index].Stop();
            sequences[index].Play();
        }

        public void PauseSequence(string name)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i].name == name)
                {
                    if (!sequences[i].isPlaying) continue;
                    sequences[i].Pause();
                }
            }
        }

        public void PauseSequence(int index)
        {
            if (index < 0) return;
            if (index >= sequences.Count) return;
            if (!sequences[index].isPlaying) return;
            sequences[index].Pause();
        }

        public void StopSequence(string name)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i].name == name) sequences[i].Stop();
                
            }
        }

        public void StopSequence(int index)
        {
            if (index < 0) return;
            if (index >= sequences.Count) return;
            sequences[index].Stop();
        }

        public void Play()
        {
            play = true; 
            if (paused)
            {
                for (int i = 0; i < sequences.Count; i++)
                {
                    if (!sequences[i].isDone) sequences[i].Play();
                }
                paused = false;
                return;
            }
            if (sequences.Count > 0 && sequences[0].playTrigger != Sequence.PlayTrigger.None) sequences[0].Play();
            for (int i = 1; i < sequences.Count; i++)
            {
                if (sequences[i].playTrigger == Sequence.PlayTrigger.Awake) sequences[i].Play();
            }
        }

        public void Pause()
        {
            play = false;
            paused = true;
            for (int i = 0; i < sequences.Count; i++) sequences[i].Pause();
        }

        public void Stop()
        {
            play = false;
            paused = false;
            for (int i = 0; i < sequences.Count; i++) sequences[i].Stop();
        }

        private void OnEnable()
        {
            if (paused) Play();
        }

        private void OnDisable()
        {
            Pause();
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}
