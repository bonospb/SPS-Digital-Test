using UnityEditor;
using UnityEngine;

using static FreeTeam.BP.UI.ExButton;

namespace FreeTeam.BP.UI
{
    [CustomPropertyDrawer(typeof(BaseButtonElement), true)]
    public class BaseButtonElementDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position.width += 40;

            var width = (position.width - 125) / 5 - 10;
            var posX = position.x;

            var graphicRect = new Rect(posX, position.y, 125, position.height);

            var normalColorRect = new Rect(posX += 130, position.y, width, position.height);
            var pressedColorRect = new Rect(posX += width + 2, position.y, width, position.height);
            var disabledColorRect = new Rect(posX += width + 2, position.y, width, position.height);
            var selectedColorRect = new Rect(posX += width + 2, position.y, width, position.height);
            var highlightedColorRect = new Rect(posX += width + 2, position.y, width, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(graphicRect, property.FindPropertyRelative("graphic"), GUIContent.none);

            EditorGUI.PropertyField(normalColorRect, property.FindPropertyRelative("normalColor"), GUIContent.none);
            EditorGUI.PropertyField(pressedColorRect, property.FindPropertyRelative("pressedColor"), GUIContent.none);
            EditorGUI.PropertyField(disabledColorRect, property.FindPropertyRelative("disabledColor"), GUIContent.none);
            EditorGUI.PropertyField(selectedColorRect, property.FindPropertyRelative("selectedColor"), GUIContent.none);
            EditorGUI.PropertyField(highlightedColorRect, property.FindPropertyRelative("highlightedColor"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
