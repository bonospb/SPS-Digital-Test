using UnityEditor;
using UnityEngine;

namespace FreeTeam.BP.UI
{
    [CustomPropertyDrawer(typeof(ExButton.StateInfo), true)]
    public class StateInfoElementDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var width = (position.width - 115);
            var posX = position.x;

            var durationRect = new Rect(posX, position.y, 105, position.height);
            var offsetRect = new Rect(position.x += 115, position.y, width, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(durationRect, property.FindPropertyRelative("transitionDuration"), GUIContent.none);
            EditorGUI.PropertyField(offsetRect, property.FindPropertyRelative("offset"), GUIContent.none);

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
