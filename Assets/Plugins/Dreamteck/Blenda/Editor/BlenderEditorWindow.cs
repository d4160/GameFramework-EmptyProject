using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreamteck.Blenda.Editor
{
    public class BlenderEditorWindow : EditorWindow
    {
        internal class Snap
        {
            internal float position = 0f;
            internal float distance = Mathf.Infinity;
            internal BlendClipEditor selectedEditor = null;
            internal int selectedSide = 0;
        }

        public Sequence sequence;
        AudioTrackEditor[] trackEditors = new AudioTrackEditor[0];

        bool snapOn = false;

        const int trackHeight = 50;
        public Vector2 zoom = Vector2.one;

        public float panelWidth = 100f;
        private Snap snap = null;
        private static Vector2 dragAnchor = Vector2.one;
        private static Vector2 lastDrag = Vector2.one;

        BlendClipEditor[] clipEditors = new BlendClipEditor[0];
        private int lastClickedEditor = -1;
        private List<int> selectedClips = new List<int>();

        BlendClipEditor.HoverType editTool = BlendClipEditor.HoverType.None;

        GUIStyle trackLabelStyle = null;

        Vector2 trackScroll = Vector2.zero;
        ConfigureClipWindow clipConfigWindow = null;

        List<Sequence.AudioTrack.BlendClip> copiedClips = new List<Sequence.AudioTrack.BlendClip>();

        bool dragTracker = false;

        bool left = false;

        private int selectedTrack = -1;

        float timeTrack = 0f;
        bool isInit = false;

        private static Texture2D trackerHeadImg = null;
        private static Texture2D startHeadImg = null;

        private const float minZoomX = 1f;
        private const float maxZoomX = 15f;
        private const float minZoomY = 0.5f;
        private const float maxZoomY = 5f;

        private Rect scrollRect = new Rect();
        private Rect viewRect = new Rect();

        public void Init()
        {
            titleContent = new GUIContent("Track Editor");
            if(trackerHeadImg == null) trackerHeadImg = ImageDB.GetImage("tracker_head.png", "Blenda/Editor/Images");
            if (startHeadImg == null) startHeadImg = ImageDB.GetImage("start_time.png", "Blenda/Editor/Images");
            UpdateTrackEditors();
            UpdateClipEditors();
            FindOverlappingClips();
            trackLabelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            trackLabelStyle.alignment = TextAnchor.UpperCenter;
            trackLabelStyle.fontSize = 10;
            trackLabelStyle.fontStyle = FontStyle.Normal;
            trackLabelStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.25f) : new Color(0f, 0f, 0f, 0.25f);
            isInit = true;
        }

        private void Awake()
        {
            Undo.undoRedoPerformed += UndoRedoRefresh;
            snapOn = EditorPrefs.GetBool("Blenda.BlenderEditorWindow.snap", false);
            zoom.x = Mathf.Clamp(EditorPrefs.GetFloat("Blenda.BlenderEditorWindow.zoom.x", zoom.x), minZoomX, maxZoomX);
            zoom.y = Mathf.Clamp(EditorPrefs.GetFloat("Blenda.BlenderEditorWindow.zoom.y", zoom.y), minZoomY, maxZoomY);
        }

        void Refresh()
        {
            EditorUtility.SetDirty(sequence.blender);
            Repaint();
        }

        void UndoRedoRefresh()
        {
            int count = 0;
            for (int i = 0; i < sequence.trackCount; i++)
            {
                count += sequence.GetTrack(i).clipCount;
            }
            UpdateTrackEditors();
            UpdateClipEditors();
            Repaint();
        }

        void SelectClip(int index)
        {
            if (selectedClips.Contains(index)) return;
            if (!Event.current.shift) selectedClips.Clear();
            selectedClips.Add(index);
        }

        void DeselectClip(int index)
        {
            selectedClips.Remove(index);
        }

        void ClearSelection()
        {
            selectedClips.Clear();
        }

        void UpdateTrackEditors()
        {
            trackEditors = new AudioTrackEditor[sequence.trackCount];
            for (int i = 0; i < trackEditors.Length; i++)
            {
                if (sequence.GetTrack(i).sequence != sequence) Debug.Log("Track " + i + " has an invalid sequence");
                sequence.GetTrack(i).UpdateClips();
                trackEditors[i] = new AudioTrackEditor(sequence.GetTrack(i), sequence, this);
            }
        }

        void UpdateClipEditors()
        {
            int count = 0;
            for (int i = 0; i < sequence.trackCount; i++) count += sequence.GetTrack(i).clipCount;
            if(clipEditors.Length != count) clipEditors = new BlendClipEditor[count];
            int index = 0;
            for (int i = 0; i < sequence.trackCount; i++)
            {
                for (int j = 0; j < sequence.GetTrack(i).clipCount; j++)
                {
                    clipEditors[index++] = new BlendClipEditor(sequence.GetTrack(i).GetClip(j), i);
                    clipEditors[index-1].HandleDuration();
                }
            }
        }


        void OnSetStartTime()
        {
            sequence.time = timeTrack;
            dragTracker = false;
            Refresh();
        }

        void OnSetDuration()
        {
            sequence.duration = timeTrack;
            dragTracker = false;
            Refresh();
        }

        void SplitSelected()
        {
            bool update = false;
            for (int i = 0; i < selectedClips.Count; i++)
            {
                Sequence.AudioTrack.BlendClip clip = clipEditors[selectedClips[i]].clip;
                if(clip.start < timeTrack && clip.end > timeTrack)
                {
                    float duration = clip.duration;
                    clip.duration = timeTrack - clip.start;
                    Sequence.AudioTrack.BlendClip copy = clip.Copy();
                    copy.start = timeTrack;
                    copy.duration = (clip.start + duration) - timeTrack;
                    clip.track.AddClip(copy);
                    update = true;
                }
            }
            if (update)
            {
                Event.current.Use();
                UpdateTrackEditors();
                UpdateClipEditors();
                Repaint();
            }
        }

        private void HandleKeyboard()
        {
            if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Copy")  CopySelectedClips();
            if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Paste") PasteCopiedClips();
            if (Event.current.type != EventType.ValidateCommand || Event.current.commandName != "Save")
            {
                if (!Event.current.control)
                {
                    if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S) SplitSelected();
                }
            }
            

            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Delete:
                        DeleteSelectedClips();
                        Event.current.Use();
                        break;
                    case KeyCode.LeftBracket:
                        for (int i = 0; i < selectedClips.Count; i++)
                        {
                            clipEditors[selectedClips[i]].clip.start = timeTrack;
                            clipEditors[selectedClips[i]].UpdateValues();
                        }
                        Event.current.Use();
                        break;

                    case KeyCode.RightBracket:
                        for (int i = 0; i < selectedClips.Count; i++)
                        {
                            clipEditors[selectedClips[i]].clip.start = timeTrack - clipEditors[selectedClips[i]].clip.duration;
                            clipEditors[selectedClips[i]].UpdateValues();
                        }
                        Event.current.Use();
                        break;
                }
            }
        }

        void CloseWindow()
        {
            Close();
            sequence.blender.serializeObject.edit = false;
        }

        private void OnGUI()
        {
            if(sequence == null)
            {
                CloseWindow();
                return;
            }
            if (sequence.blender == null)
            {
                CloseWindow();
                return;
            }
            if (!isInit) Init();
            

            bool repaint = false;
            bool rightDown = Event.current.type == EventType.MouseDown && Event.current.button == 1;
            bool rightUp = Event.current.type == EventType.MouseUp && Event.current.button == 1;
            bool leftDown = Event.current.type == EventType.MouseDown && Event.current.button == 0;
            bool leftUp = Event.current.type == EventType.MouseUp && Event.current.button == 0;

            if (!left) left = leftDown;
            else if (leftUp) left = false;
            if (leftDown || leftUp) repaint = true;

            if(left) Undo.RecordObject(sequence.blender, "Edit Tracks");


            scrollRect = new Rect(panelWidth, 25, Screen.width - panelWidth, Screen.height - 50);
            if (Event.current.type == EventType.ScrollWheel)
            {
                if (scrollRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.control)
                    {
                        zoom.y -= Event.current.delta.y * 0.15f;
                        zoom.y = Mathf.Clamp(zoom.y, minZoomY, maxZoomY);
                    }
                }
            }

            float trackWidth = (Screen.width - panelWidth - 2) * zoom.x;
            viewRect = new Rect(0, 0, trackWidth, trackEditors.Length * trackHeight * zoom.y);
            if (viewRect.height > Screen.height - 12) trackWidth -= 30;

            float trackHover = 0f;

            GUI.BeginGroup(new Rect(panelWidth + 1, 0, (Screen.width - panelWidth - 2), 25));
            if (TimeTrack.Draw(new Rect(-trackScroll.x, 0, trackWidth, 25), sequence.duration, 5, Mathf.FloorToInt((sequence.duration / zoom.x) / 15), BlendaEditorGUI.borderColor, out trackHover, 30, trackLabelStyle))
            {
                if (leftDown) dragTracker = true;
                if (rightDown)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Set Time To"), false, OnSetStartTime);
                    menu.AddItem(new GUIContent("Set Duration To"), false, OnSetDuration);
                    menu.ShowAsContext();
                }
            }
            GUI.EndGroup();
            if (dragTracker)
            {
                timeTrack = sequence.duration * trackHover;
                if (snapOn) SnapTracker();
                repaint = true;
                if (leftUp) dragTracker = false;
            }

            trackScroll = GUI.BeginScrollView(scrollRect, trackScroll, viewRect);


            if (trackEditors.Length != sequence.trackCount)
            {
                UpdateTrackEditors();
                UpdateClipEditors();
                ClearSelection();
                FindOverlappingClips();
            }

            HandleKeyboard();

            for (int i = 0; i < trackEditors.Length; i++)
            {
                trackEditors[i].trackRect = new Rect(1, i * trackHeight * zoom.y, trackWidth, trackHeight * zoom.y);
                if (trackEditors[i].trackRect.Contains(Event.current.mousePosition) && (leftDown || rightDown))
                {
                    selectedTrack = i;
                    if(rightDown)
                    {
                        if (rightDown)
                        {
                            GenericMenu menu = new GenericMenu();
                            if (copiedClips.Count > 0) menu.AddItem(new GUIContent("Paste"), false, PasteCopiedClips);
                            else menu.AddDisabledItem(new GUIContent("Paste"));
                            menu.ShowAsContext();
                        }
                    }
                }
                trackEditors[i].DrawTrack();
                if (i == 0) trackEditors[i].DrawSeparatorTop();
                trackEditors[i].DrawSeparatorBottom();
                bool hasDrag = false;
                if (trackEditors[i].GetDraggedClip(out hasDrag))
                {
                    UpdateClipEditors();
                    for (int j = 0; j < clipEditors.Length; j++)
                    {
                        if(clipEditors[j].clip == trackEditors[i].track.GetClip(trackEditors[i].track.clipCount - 1))
                        {
                            selectedClips.Clear();
                            selectedClips.Add(j);
                            break;
                        }
                    }
                }
                if (hasDrag) repaint = true;
            }

            BlendClipEditor.HoverType hoverType = BlendClipEditor.HoverType.None;
            for (int i = 0; i < clipEditors.Length; i++)
            {
                clipEditors[i].containerRect = trackEditors[clipEditors[i].trackId].trackRect;
                BlendClipEditor.HoverType h = clipEditors[i].HandleLayout();
                if (h != BlendClipEditor.HoverType.None)
                {
                    if (leftDown)
                    {
                        lastClickedEditor = i;
                        SelectClip(i);
                        repaint = true;
                    }
                    hoverType = h;
                    if (rightDown)
                    {
                        lastClickedEditor = i;
                        SelectClip(i);
                        repaint = true;
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Select GameObject"), false, OnSelectClipObject, i);
                        menu.AddItem(new GUIContent("Select AudioClip"), false, OnSelectAudioClip, i);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Fit/Fit Sequence To"), false, OnFitSequence);
                        menu.AddItem(new GUIContent("Fit/Fit To Sequence"), false, OnFitClips);
                        menu.AddItem(new GUIContent("Fit/Set Duration At End"), false, OnDurationSetAtEnd, i);
                        menu.AddItem(new GUIContent("Fit/Set Time At Beginning"), false, OnTimeSetAtBeginning, i);
                        menu.AddItem(new GUIContent("Fit/Set Time At End"), false, OnTimeSetAtEnd, i);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Settings"), false, OnClipSettings, i);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Copy"), false, CopySelectedClips);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Delete"), false, DeleteSelectedClips);
                        menu.ShowAsContext();
                    }
                }
                if (leftDown)
                {
                    clipEditors[i].UpdateValues();
                    repaint = true;
                }
            }

            if (leftDown)
            {
                dragAnchor = Event.current.mousePosition;
                editTool = hoverType;
                if (hoverType == BlendClipEditor.HoverType.None && !Event.current.control) ClearSelection();
            }
            if (leftUp) editTool = BlendClipEditor.HoverType.None;
            Vector2 dragDelta = Event.current.mousePosition - dragAnchor;
            if (Event.current.shift) dragDelta.x = 0f;
            int trackShift = Mathf.FloorToInt(dragDelta.y / (trackHeight * zoom.y * 0.9f));
            if(dragDelta.y < 0f) trackShift = Mathf.CeilToInt(dragDelta.y / (trackHeight * zoom.y));

            if(editTool != BlendClipEditor.HoverType.None && lastDrag != dragDelta)
            {
                lastDrag = dragDelta;
                repaint = true;
            }

            if (editTool != BlendClipEditor.HoverType.None) {
                for (int i = 0; i < selectedClips.Count; i++)
                {
                    BlendClipEditor editor = clipEditors[selectedClips[i]];
                    if (trackShift != 0 && editTool == BlendClipEditor.HoverType.Move)
                    {
                        int targetTrack = editor.trackId + trackShift;
                        if (targetTrack < 0) targetTrack = 0;
                        if (targetTrack > trackEditors.Length - 1) targetTrack = trackEditors.Length - 1;
                        if (targetTrack != editor.trackId)
                        {
                            sequence.GetTrack(targetTrack).AddClip(editor.clip);
                            sequence.GetTrack(editor.trackId).UnlinkClip(editor.clip);
                            editor.trackId = targetTrack;
                            editor.containerRect = trackEditors[editor.trackId].trackRect;
                        }
                    }
                    switch (editTool)
                    {
                        case BlendClipEditor.HoverType.Move: clipEditors[selectedClips[i]].Move(dragDelta.x); break;
                        case BlendClipEditor.HoverType.TrimLeft: clipEditors[selectedClips[i]].TrimLeft(dragDelta.x); break;
                        case BlendClipEditor.HoverType.TrimRight: clipEditors[selectedClips[i]].TrimRight(dragDelta.x); break;
                        case BlendClipEditor.HoverType.FadeLeft: clipEditors[selectedClips[i]].FadeLeft(dragDelta.x); break;
                        case BlendClipEditor.HoverType.FadeRight: clipEditors[selectedClips[i]].FadeRight(dragDelta.x); break;
                    }
                }
            }

            if (leftUp || rightUp)
            {
                if (lastClickedEditor >= 0 && dragDelta.magnitude <= 0.5f && !Event.current.shift)
                {
                    selectedClips.Clear();
                    SelectClip(lastClickedEditor);
                    lastClickedEditor = -1;
                }
            }

            if (trackShift != 0) dragAnchor.y = Event.current.mousePosition.y;
            if (editTool != BlendClipEditor.HoverType.None)
            {
                FindOverlappingClips();
                if (snapOn)
                {
                    SnapClips();
                    if (snap != null)
                    {
                        float selectedAnchor = snap.selectedSide == 0 ? snap.selectedEditor.clip.start : snap.selectedEditor.clip.end;
                        float snapDelta = selectedAnchor - snap.position;
                        for (int i = 0; i < selectedClips.Count; i++)
                        {
                            clipEditors[selectedClips[i]].clip.start -= snapDelta;
                            clipEditors[selectedClips[i]].HandleLayout();
                        }
                    }

                }
            }

            for (int i = 0; i < clipEditors.Length; i++) clipEditors[i].Draw(selectedClips.Contains(i));
            
            for (int i = 0; i < trackEditors.Length; i++)
            {
                if (trackEditors[i].track.mute) EditorGUI.DrawRect(trackEditors[i].trackRect, new Color(0f, 0f, 0f, 0.3f));
            }

            GUI.EndScrollView();

            GUI.BeginGroup(new Rect(panelWidth, 0, Screen.width - panelWidth - 2, Screen.height - 25));

            EditorGUI.DrawRect(new Rect((Screen.width - panelWidth) * sequence.normalizedTime * zoom.x - 1 - trackScroll.x, 25, 2f, scrollRect.height), BlendaEditorGUI.borderColor);
            GUI.color = BlendaEditorGUI.borderColor;
            GUI.DrawTexture(new Rect((Screen.width - panelWidth) * sequence.normalizedTime * zoom.x - 1 - trackScroll.x, 13, 8, 8), startHeadImg, ScaleMode.StretchToFill);
            GUI.color = Color.white;

            EditorGUI.DrawRect(new Rect((Screen.width - panelWidth) * (timeTrack / sequence.duration) * zoom.x - 1 - trackScroll.x, 25, 2f, scrollRect.height), BlendaEditorGUI.highlightColor);
            GUI.color = BlendaEditorGUI.highlightColor;
            GUI.DrawTexture(new Rect((Screen.width - panelWidth) * (timeTrack / sequence.duration) * zoom.x - 4 - trackScroll.x, 13, 8, 8), trackerHeadImg, ScaleMode.StretchToFill);
            GUI.color = Color.white;

            GUI.EndGroup();


            EditorGUI.DrawRect(new Rect(0, 25, panelWidth, Screen.height - 25), BlendaEditorGUI.lightDarkColor);
            GUI.BeginGroup(new Rect(0, 25, panelWidth, Screen.height - 51));
            float trackBlendCenter = sequence.trackPanPercent * (trackEditors.Length-1);
            for (int i = 0; i < trackEditors.Length; i++)
            {
                trackEditors[i].panelRect = new Rect(0f, i * trackHeight * zoom.y - trackScroll.y, panelWidth, trackHeight * zoom.y);
                if (trackEditors[i].panelRect.Contains(Event.current.mousePosition) && (leftDown || rightDown)) selectedTrack = i;
                if (sequence.panTracks)
                {
                    trackEditors[i].blendAmount = sequence.trackPanFalloff.Evaluate(1f-Mathf.Abs(i - trackBlendCenter) / sequence.panRange);
                } else
                {
                    trackEditors[i].blendAmount = 0f;
                }
                
                trackEditors[i].DrawPanel(selectedTrack == i);
            }
            GUI.EndGroup();

            Footer();
            if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp) repaint = true;
            if(repaint || Application.isPlaying) Repaint();
        }

        void Footer()
        {
            Rect footerRect = new Rect(panelWidth, Screen.height - 150, Screen.width - panelWidth, 25);
            EditorGUI.DrawRect(footerRect, BlendaEditorGUI.lightDarkColor);

            if (GUI.Button(new Rect(4, Screen.height - 147, panelWidth - 8, 20f), "Add Track"))
            {
                sequence.AddTracks(1);
                sequence.GetTrack(sequence.trackCount - 1).name = "Track " + sequence.trackCount;
                UpdateTrackEditors();
            }
            GUI.BeginGroup(footerRect);

            float left = 0f;
            float right = 1f;

            float scrollWidth = scrollRect.width;
            float contentWidth = viewRect.width;
            if (viewRect.height > scrollRect.height) scrollWidth -= 15;

            float span = Mathf.InverseLerp(maxZoomX, minZoomX, zoom.x);
            float scrollPercent = Mathf.Clamp01(contentWidth <= scrollWidth ? 0 : trackScroll.x / (contentWidth - scrollWidth));
            float scrollCenter = Mathf.Lerp(span/ 2f, 1f- span/2f, scrollPercent);
            left = scrollCenter - span / 2f;
            right = scrollCenter + span / 2f;
            EditorGUI.MinMaxSlider(new Rect(10, 5, 200, 20f), ref left, ref right, 0f, 1f);
            span = right - left;
            zoom.x = Mathf.Lerp(maxZoomX, minZoomX, span);
            trackScroll.x = Mathf.InverseLerp(span/2f, 1f-span/2f, Mathf.Lerp(left, right, 0.5f)) * (contentWidth - scrollWidth);
            snapOn = GUI.Toggle(new Rect(footerRect.width - 60, 5,  60, 20f), snapOn, "Snap");
            GUI.EndGroup();
        }

        void FindOverlappingClips()
        {
            for (int i = 0; i < clipEditors.Length; i++)
            {
                clipEditors[i].overlapping = false;
                for (int j = 0; j < clipEditors.Length; j++)
                {
                    if (j == i) continue;
                    if (clipEditors[j].trackId != clipEditors[i].trackId) continue;
                    if(clipEditors[j].clip.start > clipEditors[i].clip.start && clipEditors[j].clip.start < clipEditors[i].clip.end)
                    {
                        clipEditors[j].overlapping = clipEditors[i].overlapping = true;
                        break;
                    }
                    else if (clipEditors[j].clip.end > clipEditors[i].clip.start && clipEditors[j].clip.end < clipEditors[i].clip.end)
                    {
                        clipEditors[j].overlapping = clipEditors[i].overlapping = true;
                        break;
                    }
                }
            }
        }

        void SnapTracker(float range = 15f)
        {
            if (dragTracker && trackEditors.Length > 0)
            {
                float closestDist = 100f;
                float snapPos = 0f;
                float trackerPos = (timeTrack / sequence.duration) * trackEditors[0].trackRect.width;
                for (int i = 0; i < clipEditors.Length; i++)
                {
                    float distance = Mathf.Abs(clipEditors[i].rectStart - trackerPos);
                    if (distance < closestDist)
                    {
                        closestDist = distance;
                        snapPos = clipEditors[i].rectStart;
                    }
                    distance = Mathf.Abs(clipEditors[i].rectEnd - trackerPos);
                    if (distance < closestDist)
                    {
                        closestDist = distance;
                        snapPos = clipEditors[i].rectEnd;
                    }
                }
                if (closestDist <= range) timeTrack = sequence.duration * (snapPos / (trackEditors[0].trackRect.width ));
            }
        }

        void SnapClips(float range = 15f)
        {
            snap = null;
            Snap closest = new Snap();
            float trackerPos = (timeTrack / sequence.duration) * trackEditors[0].trackRect.width;
            for (int i = 0; i < selectedClips.Count; i++)
            {
                float distance = 0f;
                for (int j = 0; j < clipEditors.Length; j++)
                {
                    if (selectedClips.Contains(j)) continue;
                    clipEditors[selectedClips[i]].HandleLayout();
                    distance  = Mathf.Abs(clipEditors[selectedClips[i]].rectStart - clipEditors[j].rectEnd);
                    if(distance < closest.distance)
                    {
                        closest.selectedEditor = clipEditors[selectedClips[i]];
                        closest.selectedSide = 0;
                        closest.position = clipEditors[j].clip.end;
                        closest.distance = distance;
                    }
                    distance = Mathf.Abs(clipEditors[selectedClips[i]].rectEnd - clipEditors[j].rectStart);
                    if (distance < closest.distance)
                    {
                        closest.selectedEditor = clipEditors[selectedClips[i]];
                        closest.selectedSide = 1;
                        closest.position = clipEditors[j].clip.start;
                        closest.distance = distance;
                    }
                    distance = Mathf.Abs(clipEditors[selectedClips[i]].rectStart - clipEditors[j].rectStart);
                    if (distance < closest.distance)
                    {
                        closest.selectedEditor = clipEditors[selectedClips[i]];
                        closest.selectedSide = 0;
                        closest.position = clipEditors[j].clip.start;
                        closest.distance = distance;
                    }
                    distance = Mathf.Abs(clipEditors[selectedClips[i]].rectEnd - clipEditors[j].rectEnd);
                    if (distance < closest.distance)
                    {
                        closest.selectedEditor = clipEditors[selectedClips[i]];
                        closest.selectedSide = 1;
                        closest.position = clipEditors[j].clip.end;
                        closest.distance = distance;
                    }
                }
                distance = Mathf.Abs(clipEditors[selectedClips[i]].rectStart - trackerPos);
                if (distance < closest.distance)
                {
                    closest.selectedEditor = clipEditors[selectedClips[i]];
                    closest.selectedSide = 0;
                    closest.position =  timeTrack;
                    closest.distance = distance;
                }
                distance = Mathf.Abs(clipEditors[selectedClips[i]].rectEnd - trackerPos);
                if (distance < closest.distance)
                {
                    closest.selectedEditor = clipEditors[selectedClips[i]];
                    closest.selectedSide = 1;
                    closest.position = timeTrack;
                    closest.distance = distance;
                }
            }
            if (closest.distance <= range) snap = closest;

        }

        void OnClipSettings(object index)
        {
            clipConfigWindow = GetWindow<ConfigureClipWindow>(true);
            List<BlendClipEditor> editors = new List<BlendClipEditor>();
            for (int i = 0; i < selectedClips.Count; i++) editors.Add(clipEditors[selectedClips[i]]);
            if(editors.Count == 0) clipConfigWindow.Init(clipEditors[(int)index], this);
            else clipConfigWindow.Init(editors, this);
        }

        void OnSelectClipObject(object index)
        {
            Selection.activeGameObject = clipEditors[(int)index].clip.gameObject;
        }

        void OnSelectAudioClip(object index)
        {
            Selection.activeObject = clipEditors[(int)index].clip.audioClip;
        }

        void OnDurationSetAtEnd(object index)
        {
            sequence.duration = clipEditors[(int)index].clip.end;
            Refresh();
        }

        void OnFitSequence()
        {
            float minStart = Mathf.Infinity;
            float maxEnd = 0f;
            for (int i = 0; i < selectedClips.Count; i++)
            {
                if(minStart > clipEditors[selectedClips[i]].clip.start) minStart = clipEditors[selectedClips[i]].clip.start;
                if (maxEnd < clipEditors[selectedClips[i]].clip.end) maxEnd = clipEditors[selectedClips[i]].clip.end;
            }
            for (int i = 0; i < selectedClips.Count; i++)
            {
                clipEditors[selectedClips[i]].clip.start -= minStart;
                clipEditors[selectedClips[i]].UpdateValues();
            }
            sequence.duration = maxEnd - minStart;
            Refresh();
        }

        void OnFitClips()
        {
            for (int i = 0; i < selectedClips.Count; i++)
            {
                clipEditors[selectedClips[i]].clip.start = 0f;
                clipEditors[selectedClips[i]].clip.duration = sequence.duration;
                clipEditors[selectedClips[i]].UpdateValues();
            }
        } 

        void OnTimeSetAtBeginning(object index)
        {
            sequence.time = Mathf.Clamp(clipEditors[(int)index].clip.start, 0f, sequence.duration);
            Refresh();
        }

        void OnTimeSetAtEnd(object index)
        {
            sequence.time = Mathf.Clamp(clipEditors[(int)index].clip.end, 0f, sequence.duration);
            Refresh();
        }

        void DeleteSelectedClips()
        {
            for (int i = 0; i < selectedClips.Count; i++) clipEditors[selectedClips[i]].clip.track.RemoveClip(clipEditors[selectedClips[i]].clip);
            UpdateClipEditors();
            ClearSelection();
            FindOverlappingClips();
        }


        void CopySelectedClips()
        {
            copiedClips.Clear();
            for (int i = 0; i < selectedClips.Count; i++)
            {
                copiedClips.Add(clipEditors[selectedClips[i]].clip);
            }
        }

        void OnChangeClip(object index)
        {

        }

        void PasteCopiedClips()
        {
            if (copiedClips.Count == 0) return;
            Undo.RegisterFullObjectHierarchyUndo(sequence.blender, "Duplicate clips");
            float minTime = sequence.duration;
            for (int i = 0; i < copiedClips.Count; i++)
            {
                if(copiedClips[i].start < minTime) minTime = copiedClips[i].start;
            }
            float delta = timeTrack - minTime;
            for (int i = 0; i < copiedClips.Count; i++)
            {
                if (copiedClips[i] == null) continue;
                sequence.GetTrack(selectedTrack).AddClip(copiedClips[i].Copy());
                sequence.GetTrack(selectedTrack).GetClip(sequence.GetTrack(selectedTrack).clipCount - 1).start += delta;
            }
            UpdateTrackEditors();
            UpdateClipEditors();
        }

        private void OnDestroy()
        {
            Undo.undoRedoPerformed -= UndoRedoRefresh;
            EditorPrefs.SetBool("Blenda.BlenderEditorWindow.snap", snapOn);
            EditorPrefs.SetFloat("Blenda.BlenderEditorWindow.zoom.x", zoom.x);
            EditorPrefs.SetFloat("Blenda.BlenderEditorWindow.zoom.y", zoom.y);
            for (int i = 0; i < clipEditors.Length; i++) clipEditors[i].ReleaseResources();
            if (clipConfigWindow != null) clipConfigWindow.Close();
        }
    }
}
