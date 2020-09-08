using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreamteck.Blenda.Editor
{
    public static class TimeTrack
    {

        public static bool Draw(Rect rect, float duration, int divide, int aggregation, Color color, out float hoverPercent, float minPixelDistance = 10, GUIStyle style = null)
        {
            if (aggregation < 1) aggregation = 1;
            if (divide < 1) divide = 1;
            int intDuration = Mathf.FloorToInt(duration);
            float lastX = rect.x;
            for (int i = 0; i < intDuration + 1; i += aggregation)
            {
                float percent = (float)i / (duration);
                Rect barRect = new Rect(Mathf.Lerp(rect.x, rect.x + rect.width, percent), rect.y + rect.height * 0.5f, 1, rect.height * 0.5f);
                if (i > 0 && barRect.x - lastX < minPixelDistance) continue;
                lastX = barRect.x;
                EditorGUI.DrawRect(barRect, color);
                Rect labelRect = new Rect(Mathf.Lerp(rect.x, rect.x + rect.width, percent), rect.y, 40f, rect.height);

                labelRect.x -= 20f;
                string label = i.ToString();

                if (i >= 3600) label = string.Format("{0:D2}:{1:D2}:{2:D2}", Mathf.FloorToInt(i / 3600), Mathf.FloorToInt(i % 3600 / 60), i);
                else if (i >= 60) label = string.Format("{0:D2}:{1:D2}", Mathf.FloorToInt(i % 3600 / 60), i % 60);
                if (style != null) GUI.Label(labelRect, label, style);
                else GUI.Label(labelRect, label, EditorStyles.miniLabel);
            }
            hoverPercent = Mathf.InverseLerp(rect.x, rect.x + rect.width, Event.current.mousePosition.x);
            return rect.Contains(Event.current.mousePosition);
        }
    }
}
