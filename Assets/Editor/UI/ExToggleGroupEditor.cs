using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FreeTeam.BP.UI.Editors
{
    [CustomEditor(typeof(ExToggleGroup), true)]
    public class ExToggleGroupEditor : UnityEditor.Editor
    {
        #region Private
        private ReorderableList togglesList = null;

        protected SerializedProperty m_OnChangedProperty = null;
        #endregion

        #region Unity methods
        private void OnEnable()
        {
            InitTogglesList();

            m_OnChangedProperty = serializedObject.FindProperty("onChanged");
        }

        private void OnDisable()
        {
            DestroyTogglesList();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();

            togglesList.DoLayoutList();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_OnChangedProperty);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private methods
        private void InitTogglesList()
        {
            togglesList = new ReorderableList(serializedObject, serializedObject.FindProperty("toggles"), true, true, true, true);
            togglesList.elementHeightCallback += TogglesListHeightCallback;
            togglesList.drawHeaderCallback += DrawHeaderTogglesListHandler;
            togglesList.drawElementCallback += DrawTogglesListHandler;
        }

        private void DestroyTogglesList()
        {
            if (togglesList == null)
                return;

            togglesList.elementHeightCallback -= TogglesListHeightCallback;
            togglesList.drawElementCallback -= DrawTogglesListHandler;
            togglesList.drawHeaderCallback -= DrawHeaderTogglesListHandler;
            togglesList = null;
        }

        private float TogglesListHeightCallback(int _index)
        {
            if (togglesList == null || togglesList.count == 0)
                return 0f;

            float propertyHeight = EditorGUI.GetPropertyHeight(togglesList.serializedProperty.GetArrayElementAtIndex(_index), true);
            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }

        private void DrawHeaderTogglesListHandler(Rect _rect)
        {
            GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
            EditorGUI.LabelField(_rect, "Toggles", s);
        }

        private void DrawTogglesListHandler(Rect _rect, int _index, bool _active, bool _focused)
        {
            SerializedProperty element = togglesList.serializedProperty.GetArrayElementAtIndex(_index);
            _rect.y += 4;

            EditorGUI.PropertyField(new Rect(_rect.x, _rect.y, _rect.width, height: EditorGUIUtility.singleLineHeight), element, new GUIContent(), true);
        }
        #endregion
    }
}
