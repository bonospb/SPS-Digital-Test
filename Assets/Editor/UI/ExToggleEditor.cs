using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FreeTeam.BP.UI.Editors
{
    [CustomEditor(typeof(ExToggle), true)]
    public class ExToggleEditor : UnityEditor.Editor
    {
        #region Private
        private ReorderableList groupOnList = null;
        private ReorderableList groupOffList = null;
        #endregion

        #region Unity methods
        private void OnEnable()
        {
            InitGroupOnList();
            InitGroupOffList();
        }

        private void OnDisable()
        {
            DestroyGroupOnList();
            DestroyGroupOffList();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            GUILayout.Space(10f);

            groupOnList.DoLayoutList();

            GUILayout.Space(10f);

            groupOffList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private methods
        private void InitGroupOnList()
        {
            groupOnList = new ReorderableList(serializedObject, serializedObject.FindProperty("groupOn"), true, true, true, true);
            groupOnList.elementHeightCallback += GroupOnHeightCallback;
            groupOnList.drawElementCallback += DrawGroupOnListHandler;
            groupOnList.drawHeaderCallback += DrawHeaderGroupOnListHandler;
        }

        private void DestroyGroupOnList()
        {
            if (groupOnList == null)
                return;

            groupOnList.elementHeightCallback -= GroupOnHeightCallback;
            groupOnList.drawElementCallback -= DrawGroupOnListHandler;
            groupOnList.drawHeaderCallback -= DrawHeaderGroupOnListHandler;
            groupOnList = null;
        }

        private float GroupOnHeightCallback(int _index)
        {
            if (groupOnList == null || groupOnList.count == 0)
                return 0f;

            float propertyHeight = EditorGUI.GetPropertyHeight(groupOnList.serializedProperty.GetArrayElementAtIndex(_index), true);
            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }

        private void DrawHeaderGroupOnListHandler(Rect _rect)
        {
            GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
            EditorGUI.LabelField(_rect, "GroupOn", s);
        }

        private void DrawGroupOnListHandler(Rect _rect, int _index, bool _active, bool _focused)
        {
            if (groupOnList == null)
                return;

            SerializedProperty element = groupOnList.serializedProperty.GetArrayElementAtIndex(_index);
            _rect.y += 4;

            EditorGUI.PropertyField(new Rect(_rect.x += 10, _rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), element, new GUIContent(), true);
        }

        private void InitGroupOffList()
        {
            groupOffList = new ReorderableList(serializedObject, serializedObject.FindProperty("groupOff"), true, true, true, true);
            groupOffList.elementHeightCallback += GroupOffHeightCallback;
            groupOffList.drawHeaderCallback += DrawHeaderGroupOffListHandler;
            groupOffList.drawElementCallback += DrawGroupOffListHandler;
        }

        private void DestroyGroupOffList()
        {
            if (groupOffList == null)
                return;

            groupOffList.elementHeightCallback -= GroupOffHeightCallback;
            groupOffList.drawElementCallback -= DrawGroupOffListHandler;
            groupOffList.drawHeaderCallback -= DrawHeaderGroupOffListHandler;
            groupOffList = null;
        }

        private float GroupOffHeightCallback(int _index)
        {
            if (groupOffList == null || groupOffList.count == 0)
                return 0f;

            float propertyHeight = EditorGUI.GetPropertyHeight(groupOffList.serializedProperty.GetArrayElementAtIndex(_index), true);
            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }

        private void DrawHeaderGroupOffListHandler(Rect _rect)
        {
            GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
            EditorGUI.LabelField(_rect, "GroupOff", s);
        }

        private void DrawGroupOffListHandler(Rect _rect, int _index, bool _active, bool _focused)
        {
            if (groupOffList == null)
                return;

            SerializedProperty element = groupOffList.serializedProperty.GetArrayElementAtIndex(_index);
            _rect.y += 4;

            EditorGUI.PropertyField(new Rect(_rect.x += 10, _rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), element, new GUIContent(), true);
        }
        #endregion
    }
}
