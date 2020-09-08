using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


namespace Dreamteck.Blenda.Editor
{
    public class BlendClipEditor
    {
        public Sequence.AudioTrack.BlendClip clip = null;
        public int trackId = 0;

        public enum HoverType { None, Move, TrimLeft, TrimRight, FadeLeft, FadeRight }

        public float start = 0f;
        public float duration = 0f;
        public float fadeIn = 0f;
        public float fadeOut = 0f;
        public float offset = 0f;

        public Rect containerRect = new Rect();

        public bool overlapping = false;
        protected Rect rect = new Rect();
        protected Rect fadeInRect = new Rect();
        protected Rect fadeOutRect = new Rect();
        protected Rect innerRect = new Rect();

        protected Texture2D fadeInTexture = null;
        protected Texture2D fadeOutTexture = null;

        private static Texture2D stripePattern = null;
        private static Texture2D loopIcon = null;
        private static Texture2D startIcon = null;
        private static Texture2D endIcon = null;

        private Texture2D waveform = null;
        private Color32[] waveformPixels = new Color32[0];
        private float[] waveformSamples = new float[0];

        private bool updateWaveform = true;

        public float rectStart
        {
            get { return rect.x; }
        }

        public float rectEnd
        {
            get { return rect.x + rect.width; }
        }

        public BlendClipEditor(Sequence.AudioTrack.BlendClip c, int id)
        {
            clip = c;
            trackId = id;
            if (stripePattern == null) stripePattern = ImageDB.GetImage("stripes.png", "Blenda/Editor/Images");
            if (loopIcon == null) loopIcon = ImageDB.GetImage("loop.png", "Blenda/Editor/Images");
            if (startIcon == null) startIcon = ImageDB.GetImage("start.png", "Blenda/Editor/Images");
            if (endIcon == null) endIcon = ImageDB.GetImage("end.png", "Blenda/Editor/Images");
        }

        protected string GetName()
        {
            return clip.audioClip != null ? clip.audioClip.name : "MISSING CLIP";
        }

        protected float GetPosition()
        {
            return clip.start;
        }

        protected float GetDuration()
        {
            return clip.duration;
        }

        protected float GetFadeIn()
        {
            return clip.fadeIn;
        }

        protected float GetFadeOut()
        {
            return clip.fadeOut;
        }

        protected AnimationCurve GetFadeInCurve()
        {
            return clip.fadeInCurve;
        }

        protected AnimationCurve GetFadeOutCurve()
        {
            return clip.fadeOutCurve;
        }

        public void UpdateWaveform()
        {
            updateWaveform = true;
        }

        public void ReleaseResources()
        {
            UnityEngine.Object.DestroyImmediate(waveform);
        }

        public virtual HoverType HandleLayout()
        {
            if (clip == null) return HoverType.None;
            if (clip.track == null) return HoverType.None;
            HoverType hoverType = HoverType.None;
            int totalWidth = Mathf.RoundToInt((GetDuration() / clip.track.sequence.duration) * containerRect.width);
            if (totalWidth < 5) totalWidth = 5;
            float fadeInWidth = Mathf.RoundToInt((GetFadeIn() / clip.track.sequence.duration) * containerRect.width);
            float fadeOutWidth = Mathf.RoundToInt((GetFadeOut() / clip.track.sequence.duration) * containerRect.width);

            if (updateWaveform)
            {
                int texWidth = Mathf.RoundToInt((clip.audioClip.length / clip.track.sequence.duration) * containerRect.width); 
                GenerateWaveform(texWidth, Mathf.RoundToInt(containerRect.height));
            }
            updateWaveform = false;

            float positionPercent = GetPosition() / clip.track.sequence.duration;
            GUI.BeginGroup(containerRect);

            float rectLeft = positionPercent * containerRect.width;
            float rectRight = positionPercent * containerRect.width + totalWidth;

            rect = new Rect(rectLeft, 0, totalWidth, containerRect.height - 1);
            bool outOfBounds = rect.x + rect.width > containerRect.width;
            Rect secondaryRect = new Rect((rect.x - containerRect.width), rect.y, rect.width, rect.height);

            Vector2 mousePos = Event.current.mousePosition;

            Rect leftMargin = new Rect(rectLeft - 2, 0, 8, containerRect.height - 1);
            Rect rightMargin = new Rect(rectRight - 6, 0, 8, containerRect.height - 1); 
           

            if (Event.current.alt)
            {
                leftMargin.x += fadeInWidth;
                rightMargin.x -= fadeOutWidth;
            }

            if (leftMargin.x > containerRect.width) leftMargin.x -= containerRect.width;
            if (rightMargin.x > containerRect.width) rightMargin.x -= containerRect.width;

            EditorGUIUtility.AddCursorRect(leftMargin, MouseCursor.ResizeHorizontal);
            EditorGUIUtility.AddCursorRect(rightMargin, MouseCursor.ResizeHorizontal);
            
            if (new Rect(0, 0, containerRect.width, containerRect.height).Contains(mousePos))
            {
                if (leftMargin.Contains(mousePos))
                {
                    EditorGUI.DrawRect(leftMargin, Color.black);
                    if (Event.current.alt) hoverType = HoverType.FadeLeft;
                    else hoverType = HoverType.TrimLeft;
                }
                else if (rightMargin.Contains(mousePos))
                {
                    EditorGUI.DrawRect(rightMargin, Color.black);
                    if (Event.current.alt) hoverType = HoverType.FadeRight;
                    else hoverType = HoverType.TrimRight;
                }
                else if (rect.Contains(mousePos) || (outOfBounds && secondaryRect.Contains(mousePos))) hoverType = HoverType.Move;
            }

            fadeInRect = new Rect(0, 0f, fadeInWidth, rect.height);
            fadeOutRect = new Rect(rect.width - fadeOutWidth, 0f, fadeOutWidth, rect.height);
            innerRect = new Rect(fadeInWidth, 0f, totalWidth - fadeInWidth - fadeOutWidth, rect.height);
            GUI.EndGroup();
            return hoverType;
        }

        public virtual void Draw(bool selected)
        {
            GUI.BeginGroup(containerRect);
            DrawRegular(rect, selected);
            if(clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Loop)
            {
                if (rect.x + rect.width > containerRect.width) DrawRegular(new Rect((rect.x-containerRect.width), rect.y, rect.width, rect.height), selected);
            }
            GUI.EndGroup();
        }

        void DrawRegular(Rect r, bool selected)
        {
            EditorGUI.DrawRect(r, overlapping ? new Color(1f, 1f, 1f, 0.5f) : BlendaEditorGUI.lightColor);
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot)
            {
                GUI.color = BlendaEditorGUI.lightDarkColor;
                GUI.DrawTexture(new Rect(rect.x + rect.width * clip.oneshotAnchor, rect.y + rect.height / 2f - 8, 16, 16), startIcon, ScaleMode.StretchToFill);
                GUI.color = Color.white;
            }
            GUI.BeginGroup(r);
            Rect localRect = new Rect(0, 0, r.width, r.height);

            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot)
            {
                Rect lineRect = new Rect(rect.width * clip.oneshotAnchor - 1f, 0f, 2f, rect.height);
                EditorGUI.DrawRect(lineRect, BlendaEditorGUI.lightDarkColor);
            }
            GUI.color = new Color(1f, 1f, 1f, 0.4f * GUI.color.a);
            GUI.Label(new Rect(5f, 3f, r.width * 0.7f, 16f), GetName());
            GUI.color = Color.white;

            float avgPitch = 1f;
            float maxPitch = 0f;
            float minPitch = Mathf.Infinity;

            if (clip.usePitchEnvelope && clip.pitchEnvelope != null)
            {

                int iterations = Mathf.CeilToInt(r.width / 5f);
                for (int i = 0; i < r.width / 5f; i++)
                {
                    float dividor = (r.width / 5f - 1f);
                    float val = clip.pitchEnvelope.Evaluate(dividor <= 0f ? 0f : (float)i / dividor);
                    avgPitch += val;
                    if (maxPitch < val) maxPitch = val;
                    if (minPitch > val) minPitch = val;
                }
                if (iterations > 0) avgPitch /= iterations;

            }

            //Draw the waveform
            if (waveform != null)
            {
                Color prev = GUI.color;

                GUI.color = Color.Lerp(BlendaEditorGUI.darkColor, BlendaEditorGUI.highlightColor, 0.5f);
                if (EditorGUIUtility.isProSkin) GUI.color = Color.Lerp(BlendaEditorGUI.lightColor, BlendaEditorGUI.highlightColor, 0.5f);
                GUI.DrawTextureWithTexCoords(localRect, waveform, new Rect(-clip.startOffset / clip.audioClip.length * clip.pitch * avgPitch, 0, clip.duration / clip.audioClip.length * clip.pitch * avgPitch, 1));
                GUI.color = prev;
            }

            //Generate fade in - fade out textures if needed
            if (GetFadeIn() > 0f)
            {
                if (fadeInTexture == null || Mathf.Abs(fadeInTexture.width - fadeInRect.width) >= 10 || Mathf.Abs(fadeInTexture.height - fadeInRect.height) >= 10)
                {
                    GenerateFadeTexture(Mathf.CeilToInt(fadeInRect.width), Mathf.CeilToInt(fadeInRect.height), ref fadeInTexture, GetFadeInCurve());
                }
            }
            if (GetFadeOut() > 0f)
            {
                if (fadeOutTexture == null || Mathf.Abs(fadeOutTexture.width - fadeOutRect.width) >= 10 || Mathf.Abs(fadeOutTexture.height - fadeOutRect.height) >= 10)
                {
                    GenerateFadeTexture(Mathf.CeilToInt(fadeOutRect.width), Mathf.CeilToInt(fadeOutRect.height), ref fadeOutTexture, GetFadeOutCurve());
                }
            }

            GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.4f * GUI.color.a);
            if (GetFadeIn() > 0f)
            {
                GUI.DrawTexture(fadeInRect, fadeInTexture, ScaleMode.StretchToFill);
                if (Event.current.alt) EditorGUI.DrawRect(new Rect(fadeInRect.x + fadeInRect.width - 2f, fadeInRect.y, 2f, fadeInRect.height), GUI.color);
            }
            if (GetFadeOut() > 0f)
            {
                GUI.DrawTexture(fadeOutRect, fadeOutTexture, ScaleMode.StretchToFill);
                if (Event.current.alt) EditorGUI.DrawRect(new Rect(fadeOutRect.x, fadeOutRect.y, 2f, fadeOutRect.height), GUI.color);
            }
            GUI.color = Color.white;

            if (selected) EditorGUI.DrawRect(localRect, new Color(0.1f, 0.5f, 1f, 0.3f));

            //Draw overlay stuff
            if (clip.audioClip == null)
            {
                EditorGUI.DrawRect(localRect, new Color(BlendaEditorGUI.highlightColor.r, BlendaEditorGUI.highlightColor.g, BlendaEditorGUI.highlightColor.b, 0.4f));
                clip.audioClip = (AudioClip)EditorGUI.ObjectField(new Rect(5f, localRect.height - 21, localRect.width * 0.7f, 16f), clip.audioClip, typeof(AudioClip), false);
            }


            float clipLength = clip.audioClip.length / (clip.pitch * avgPitch);
            float current = clip.startOffset;
            if (clip.startOffset > 0f) current -= clipLength * Mathf.CeilToInt(clip.startOffset / clipLength);

            switch (clip.playType)
            {
                case Sequence.AudioTrack.BlendClip.PlayType.Loop:
                    if (clipLength > 0f)
                    {
                        while (current < duration)
                        {
                            current += clipLength;
                            if (current > 0f && current < clip.duration) DrawMark(r, current / clip.duration, loopIcon);
                        }
                    }
                    break;

                case Sequence.AudioTrack.BlendClip.PlayType.Default:

                    if (clip.startOffset > 0f)
                    {
                        GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                        if (clip.startOffset < clip.duration) DrawMark(r, Mathf.Clamp01(clip.startOffset / clip.duration), startIcon);
                        GUI.color = Color.white;
                    }

                    if (clip.startOffset + clipLength < clip.duration)
                    {
                        float percent = (clip.startOffset + clipLength) / clip.duration;
                        GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                        Rect stripeRect = new Rect(r.width * percent + 2, 1, r.width * (1f - percent) - 2, r.height - 2);
                        GUI.DrawTextureWithTexCoords(stripeRect, stripePattern, new Rect(0, 0, stripeRect.width / stripePattern.width, stripeRect.height / stripePattern.height));
                        DrawMark(r, Mathf.Clamp01(percent), endIcon);
                        GUI.color = Color.white;
                    }

                    if(clip.startOffset > 0f)
                    {
                        float percent = clip.startOffset / clip.duration;
                        GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                        Rect stripeRect = new Rect(0, 1, r.width * percent - 2, r.height - 2);
                        GUI.DrawTextureWithTexCoords(stripeRect, stripePattern, new Rect(0, 0, stripeRect.width / stripePattern.width, stripeRect.height / stripePattern.height));
                        DrawMark(r, Mathf.Clamp01(percent), endIcon);
                        GUI.color = Color.white;
                    }
                    break;
            }

            

            if (clip.useVolumeEnvelope && clip.volumeEnvelope != null) DrawCurve(r, clip.volumeEnvelope, Color.cyan, 0f, 1f, 0.05f, 0.6f);
            if (clip.usePitchEnvelope && clip.pitchEnvelope != null) DrawCurve(r, clip.pitchEnvelope, Color.magenta, minPitch, maxPitch, 0.05f, 0.6f);
            GUI.EndGroup();
        }

        public void RegenerateTextures()
        {
            GenerateFadeTexture(Mathf.CeilToInt(fadeInRect.width), Mathf.CeilToInt(fadeInRect.height), ref fadeInTexture, GetFadeInCurve());
            GenerateFadeTexture(Mathf.CeilToInt(fadeOutRect.width), Mathf.CeilToInt(fadeOutRect.height), ref fadeOutTexture, GetFadeOutCurve());
        }

        public void DrawCurve(Rect r, AnimationCurve curve, Color color, float minValue, float maxValue, float minHeight, float maxHeight)
        {
            Handles.BeginGUI();
            Handles.color = color;
            Vector2 last = Vector2.zero;
            int iterations = Mathf.CeilToInt(r.width / 5f);
            for (int i = 0; i < iterations; i++)
            {
                float percent = (float)i / (iterations > 1 ? (iterations - 1) : 1f);
                Vector2 current = new Vector2(r.width * percent, r.y + r.height - Mathf.Lerp(minHeight * r.height, maxHeight * r.height, Mathf.InverseLerp(minValue, maxValue, curve.Evaluate(percent))));
                if (i > 0) Handles.DrawLine(last, current);
                last = current;
            }
            Handles.EndGUI();
        }


        private void DrawMark(Rect r, float percent, Texture2D tex)
        {
            Color color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
            EditorGUI.DrawRect(new Rect(r.width * percent - 1, 0, 1f, r.height / 2f - 9), color);
            EditorGUI.DrawRect(new Rect(r.width * percent - 1, r.height - (r.height / 2f - 9), 1f, r.height / 2f - 9), color);
            GUI.color = color;
            GUI.DrawTexture(new Rect(r.width * percent - tex.width/2, r.height/2 - tex.height/2, tex.width, tex.height), tex, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }

        public void UpdateValues()
        {
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Loop) LoopPosition();
            else if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot) ClampPosition();
            start = clip.start;
            duration = clip.duration;
            fadeIn = clip.fadeIn;
            fadeOut = clip.fadeOut;
            offset = clip.startOffset;
        }

        protected float ToTime(float pixels)
        {
            return (pixels / containerRect.width) * clip.track.sequence.duration;
        }

        public void Move(float delta)
        {
            clip.start = start + ToTime(delta);
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Loop) LoopPosition();
            else if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot) ClampPosition();
            HandleDuration();
        }

        void LoopPosition()
        {
            if (clip.start < 0f)
            {
                clip.start += clip.track.sequence.duration;
                start += clip.track.sequence.duration;
            }
            if (clip.start > clip.track.sequence.duration)
            {
                clip.start -= clip.track.sequence.duration;
                start -= clip.track.sequence.duration;
            }
        }

        void ClampPosition()
        {
            if (clip.start < 0f) clip.start = 0f;
            if (clip.start > clip.track.sequence.duration) clip.start = clip.track.sequence.duration;
        }

        void LoopPositionRight()
        {

        }

        public void TrimLeft(float delta)
        {
            float trimAmount = ToTime(delta);
            if (trimAmount > duration) trimAmount = duration;
            if(clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot)
            {
                if (duration - trimAmount > clip.audioClip.length) trimAmount = clip.audioClip.length - duration;
            }
            clip.start = start + trimAmount;
            clip.startOffset = offset - trimAmount;
            clip.duration = duration - trimAmount;
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Loop) LoopPosition();
            else if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot) ClampPosition();
            HandleDuration();
        }

        public void TrimRight(float delta)
        {
            clip.duration = duration + ToTime(delta);
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Loop) LoopPosition();
            else if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot) ClampPosition();
            HandleDuration();
        }

        public void FadeLeft(float delta)
        {
            clip.fadeIn = fadeIn + ToTime(delta);
            HandleDuration();
        }

        public void FadeRight(float delta)
        {
            clip.fadeOut = fadeOut - ToTime(delta);
            HandleDuration();
        }

        public void HandleDuration()
        {
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Oneshot && clip.duration > clip.audioClip.length) clip.duration = clip.audioClip.length;
            if (clip.playType == Sequence.AudioTrack.BlendClip.PlayType.Loop && clip.duration > clip.track.sequence.duration) clip.duration = clip.track.sequence.duration;
        }

        private static void GenerateFadeTexture(int width, int height, ref Texture2D tex, AnimationCurve curve)
        {
            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            if (tex == null) tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            else tex.Resize(width, height);
            if (tex.width <= 1)
            {
                tex.Apply();
                return;
            }
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < width; i++)
            {
                float eval = curve.Evaluate((float)i / (width - 1));
                for (int j = 0; j < height; j++)
                {
                    float heightPercent = (float)j / (height - 1);
                    if (heightPercent > eval) pixels[j * width + i] = Color.white;
                    else pixels[j * width + i] = Color.clear;
                }
            }
            tex.SetPixels(pixels);
            tex.Apply();
        }

        private void GenerateWaveform(int width, int height)
        {

            if (width > Screen.width) width = Screen.width;
            if (height > Screen.height) height = Screen.height;
            if(width <= 0 || height <= 0)
            {
                if (waveform != null)
                {
                    UnityEngine.Object.DestroyImmediate(waveform);
                    waveform = new Texture2D(1, 1, TextureFormat.ARGB32, false);


                    waveform.SetPixel(0, 0, Color.clear);
                    waveform.Apply();
                }

                return;
            }
            AudioClip audioClip = clip.audioClip;
            int sampleCount = audioClip.samples * audioClip.channels;
            if(waveformSamples.Length != sampleCount) waveformSamples = new float[sampleCount];
            audioClip.LoadAudioData();
            audioClip.GetData(waveformSamples, 0);
            if (waveform != null)
            {
                if (waveform.width != width || waveform.height != height)
                {
                    UnityEngine.Object.DestroyImmediate(waveform);
                    waveform = new Texture2D(width, height, TextureFormat.ARGB32, false);
                    waveform.filterMode = FilterMode.Point;
                }
            }
            else
            {
                waveform = new Texture2D(width, height, TextureFormat.ARGB32, false);
                waveform.filterMode = FilterMode.Point;
            }
            if (waveformPixels.Length != width * height) waveformPixels = new Color32[width * height];
            for (int i = 0; i < waveformPixels.Length; i++)
            {
                waveformPixels[i].r = waveformPixels[i].g = waveformPixels[i].b = 255;
                waveformPixels[i].a = 0;
            }

            int half = (height-1) / 2;

            for (int x = 0; x < width; x++)
            {
                float percent = (float)x / (width - 1);
                int currentIndex = Mathf.RoundToInt(percent * (waveformSamples.Length - 1));
                float currentSample = Mathf.Abs(waveformSamples[currentIndex]);
                if(x > 0)
                {
                    float previousPercent = (float)(x-1) / (width - 1);
                    int previousIndex = Mathf.RoundToInt(previousPercent * (waveformSamples.Length - 1));
                    for (int i = previousIndex; i < currentIndex; i++)
                    {
                        float sample = Mathf.Abs(waveformSamples[i]);
                        if (sample > currentSample) currentSample = sample;
                    }
                }

                int amplitude = Mathf.RoundToInt(currentSample * half);
                if (amplitude > half) amplitude = half;
                if (amplitude < 1) amplitude = 1;
                for (int y = half - amplitude; y <= half + amplitude; y++)
                {
                    int index = width * y + x;
                    waveformPixels[index].a = 255;
                }
            }

            audioClip.UnloadAudioData();
            waveform.SetPixels32(waveformPixels);
            waveform.Apply();
        }


        private static Texture2D GetWaveForm(AudioClip clip, int channel, float width, float height)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("GetWaveForm", BindingFlags.Static | BindingFlags.Public);
            string path = AssetDatabase.GetAssetPath(clip);
            AudioImporter importer = (AudioImporter)AssetImporter.GetAtPath(path);
            if (method == null) Debug.Log("Null Method:");
            if (importer == null) Debug.Log("Null importer");
            Texture2D texture = (Texture2D)method.Invoke(null, new object[] { clip, importer, channel, width, height });
            return texture;
        }

        private static Texture2D GetWaveFormFast(AudioClip clip, int channel, int fromSample, int toSample, float width, float height)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("GetWaveFormFast", BindingFlags.Static | BindingFlags.Public);
            Texture2D texture = (Texture2D)method.Invoke(null, new object[] { clip, channel, fromSample, toSample, width, height });
            return texture;
        }

        private static void ClearWaveForm(AudioClip clip)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("ClearWaveForm", BindingFlags.Static | BindingFlags.Public);
            method.Invoke(null, new object[] { clip });
        }
    }
}
