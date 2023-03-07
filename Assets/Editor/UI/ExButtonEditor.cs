using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;

namespace FreeTeam.BP.UI.Editors
{
    [CustomEditor(typeof(ExButton), true)]
    [CanEditMultipleObjects]
    public class ExButtonEditor : SelectableEditor
    {
        #region Private
        protected SerializedProperty idProperty = null;

        protected SerializedProperty m_OnClickProperty = null;
        protected SerializedProperty m_OnSelectedProperty = null;
        protected SerializedProperty m_OnDeselectedProperty = null;

        protected SerializedProperty normalProperty = null;
        protected SerializedProperty pressedProperty = null;
        protected SerializedProperty disabledProperty = null;
        protected SerializedProperty selectedProperty = null;
        protected SerializedProperty highlightedProperty = null;

        private ReorderableList elementsList;

        private bool isStatesFoldout = true;
        private bool isElementsFoldout = true;
        #endregion

        #region Unity methods
        protected override void OnEnable()
        {
            base.OnEnable();

            EditorApplication.update += Update;

            isStatesFoldout = EditorPrefs.GetBool($"{GetType().Name}_isStatesFoldout", isStatesFoldout);
            isElementsFoldout = EditorPrefs.GetBool($"{GetType().Name}_isElementsFoldout", isElementsFoldout);

            idProperty = serializedObject.FindProperty("id");

            m_OnClickProperty = serializedObject.FindProperty("onClick");
            m_OnSelectedProperty = serializedObject.FindProperty("onSelected");
            m_OnDeselectedProperty = serializedObject.FindProperty("onDeselected");

            normalProperty = serializedObject.FindProperty("normal");
            pressedProperty = serializedObject.FindProperty("pressed");
            disabledProperty = serializedObject.FindProperty("disabled");
            selectedProperty = serializedObject.FindProperty("selected");
            highlightedProperty = serializedObject.FindProperty("highlighted");

            InitElementsList();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EditorApplication.update -= Update;

            EditorPrefs.SetBool($"{GetType().Name}_isStatesFoldout", isStatesFoldout);
            EditorPrefs.SetBool($"{GetType().Name}_isElementsFoldout", isElementsFoldout);

            DestroyElementsList();
        }

        void Update()
        {
            UpdateElementsList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(idProperty);

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            EditorGUILayout.Space();

            serializedObject.Update();


            EditorGUILayout.BeginVertical(StyleFramework.box);
            isStatesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(isStatesFoldout, "States", StyleFramework.foldout);
            if (isStatesFoldout)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(normalProperty);
                EditorGUILayout.PropertyField(pressedProperty);
                EditorGUILayout.PropertyField(disabledProperty);
                EditorGUILayout.PropertyField(selectedProperty);
                EditorGUILayout.PropertyField(highlightedProperty);
                EditorGUI.indentLevel = 0;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();

            elementsList.DoLayoutList();

            EditorGUILayout.PropertyField(m_OnClickProperty);
            EditorGUILayout.PropertyField(m_OnSelectedProperty);
            EditorGUILayout.PropertyField(m_OnDeselectedProperty);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private methods
        private void InitElementsList()
        {
            elementsList = new ReorderableList(serializedObject, serializedObject.FindProperty("elements"), true, true, true, true);
            elementsList.elementHeightCallback += ElementsHeightCallback;
            elementsList.drawHeaderCallback += DrawHeaderElementsListHandler;
            elementsList.drawElementCallback += DrawElementsListHandler;
        }

        private void DestroyElementsList()
        {
            if (elementsList == null)
                return;

            elementsList.elementHeightCallback -= ElementsHeightCallback;
            elementsList.drawHeaderCallback -= DrawHeaderElementsListHandler;
            elementsList.drawElementCallback -= DrawElementsListHandler;
            elementsList = null;
        }

        private void UpdateElementsList()
        {
            if (elementsList == null)
                return;

            elementsList.draggable = isElementsFoldout;
            elementsList.displayAdd = isElementsFoldout;
            elementsList.displayRemove = isElementsFoldout;
        }

        private float ElementsHeightCallback(int _index)
        {
            if (!isElementsFoldout || elementsList.count == 0)
                return 0f;

            float propertyHeight = EditorGUI.GetPropertyHeight(elementsList.serializedProperty.GetArrayElementAtIndex(_index), true);
            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }

        private void DrawHeaderElementsListHandler(Rect _rect)
        {
            var x = _rect.x + 10;
            var y = _rect.y;
            var w = _rect.width;
            var h = _rect.height;

            GUIStyle s = new GUIStyle("Foldout") { fontStyle = FontStyle.Bold };
            isElementsFoldout = EditorGUI.Foldout(new Rect(x, y, w, h), isElementsFoldout, "Elements", true, s);
        }

        private void DrawElementsListHandler(Rect _rect, int _index, bool _active, bool _focused)
        {
            if (!isElementsFoldout)
                return;

            SerializedProperty element = elementsList.serializedProperty.GetArrayElementAtIndex(_index);
            _rect.y += 4;

            EditorGUI.PropertyField(new Rect(_rect.x, _rect.y, _rect.width, height: EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        }
        #endregion
    }

    static class StyleFramework
    {
        public static GUIStyle foldout = null;
        public static GUIStyle box = null;

        static StyleFramework()
        {
            foldout = new GUIStyle("FoldoutHeader");
            foldout.fontStyle = FontStyle.Bold;
            foldout.padding = new RectOffset(20, 20, 10, 10);
            foldout.margin = new RectOffset(15, 5, 0, 0);

            box = new GUIStyle("FrameBox");
            box.padding = new RectOffset(5, 5, 1, 1);
            box.margin = new RectOffset(0, 0, 0, 5);
        }
    }
}
