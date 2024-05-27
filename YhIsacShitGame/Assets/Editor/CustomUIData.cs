using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using YhProj.Game.UI;

namespace YhProj.Game.YhEditor
{
    [CustomEditor(typeof(UIData))]
    public class CustomUIData : Editor
    {
        UIData uiData;

        ReorderableList mainUIList;
        ReorderableList popupUIList;
        ReorderableList tooltipUIList;
        ReorderableList contextualUIList;
        //ReorderableList testUIList;

        private void OnEnable()
        {
            mainUIList = new ReorderableList(serializedObject, serializedObject.FindProperty("mainUIDataList"), true, true, true, true);
            popupUIList = new ReorderableList(serializedObject, serializedObject.FindProperty("popupUIDataList"), true, true, true, true);
            tooltipUIList = new ReorderableList(serializedObject, serializedObject.FindProperty("tooltipUIDataList"), true, true, true, true);
            contextualUIList = new ReorderableList(serializedObject, serializedObject.FindProperty("contextualUIDataList"), true, true, true, true);
            // testUIList = new ReorderableList(serializedObject, serializedObject.FindProperty("testUIDataList"), true, true, true, true);

            // main ui
            //DrawHeaderCallback(UIRootType.MAIN_UI, mainUIList);
            //DrawElementCallback(mainUIList);
            //OnSelectCallback(mainUIList);
            //OnRemoveCallback(mainUIList);
            //OnAddCallback(UIRootType.MAIN_UI, mainUIList);

            // popup ui
            DrawHeaderCallback(UIRootType.Popup, popupUIList);
            DrawElementCallback(popupUIList);
            OnSelectCallback(popupUIList);
            OnRemoveCallback(popupUIList);
            OnAddCallback(UIRootType.Popup, popupUIList);

            // tooltip ui
            DrawHeaderCallback(UIRootType.Tooltip, tooltipUIList);
            DrawElementCallback(tooltipUIList);
            OnSelectCallback(tooltipUIList);
            OnRemoveCallback(tooltipUIList);
            OnAddCallback(UIRootType.Tooltip, tooltipUIList);

            // contextual ui
            DrawHeaderCallback(UIRootType.Contextual, contextualUIList);
            DrawElementCallback(contextualUIList);
            OnSelectCallback(contextualUIList);
            OnRemoveCallback(contextualUIList);
            OnAddCallback(UIRootType.Contextual, contextualUIList);

            // test ui
            //DrawHeaderCallback(UIRootType.TEST_UI, testUIList);
            //DrawElementCallback(testUIList);
            //OnSelectCallback(testUIList);
            //OnRemoveCallback(testUIList);
            //OnAddCallback(UIRootType.TEST_UI, testUIList);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (uiData == null)
            {
                uiData = target as UIData;
            }

            serializedObject.Update();

            mainUIList.DoLayoutList();
            popupUIList.DoLayoutList();
            tooltipUIList.DoLayoutList();
            contextualUIList.DoLayoutList();
            // testUIList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        void DrawHeaderCallback(UIRootType _rootType, ReorderableList _reorderableList)
        {
            string header = "";

            switch (_rootType)
            {
                //case UIRootType.MAIN_UI:
                //    header = "Main UI";
                //    break;
                case UIRootType.Popup:
                    header = "Popup UI";
                    break;
                //case UIRootType.TEST_UI:
                //    header = "Test UI";
                //break;
                case UIRootType.Tooltip:
                    header = "Tooltip UI";
                    break;
                case UIRootType.Contextual:
                    header = "Contextual UI";
                    break;
            }

            // header callback
            _reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, header);
            };
        }
        void DrawElementCallback(ReorderableList _reorderableList)
        {
            // 그냥 이쁘게 layout하는 callback
            /*
             * ReorderableList에서 특정 필드를 읽기 전용으로 만들기 위해서는 해당 필드의 GUIStyle을 변경하거나 EditorGUI.PropertyField를 사용하여 필드를 직접 그릴 때 수정을 막아야 합니다.
             */
            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("name"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(rect.x + 200, rect.y, rect.width - 200 - 100, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("prefabName"), GUIContent.none);

                // 수정을 막기위한 조치
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(new Rect(rect.x + rect.width - 100, rect.y, 100, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("uiRootType"), GUIContent.none);
                EditorGUI.EndDisabledGroup();
            };
        }

        void OnSelectCallback(ReorderableList _reorderableList)
        {
            // 선택한 인덱스의 하이라이트 callback
            _reorderableList.onSelectCallback = (ReorderableList l) =>
            {
                var nameProperty = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("name");
                var prefabName = nameProperty.stringValue;

                // prefabName이 유효한지 확인 후 Pinging
                if (!string.IsNullOrEmpty(prefabName))
                {
                    var prefab = Resources.Load<GameObject>(prefabName);
                    if (prefab)
                    {
                        EditorGUIUtility.PingObject(prefab);
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab with name {prefabName} not found.");
                    }
                }
                else
                {
                    Debug.LogWarning("Prefab name is empty.");
                }
            };
        }

        void OnRemoveCallback(ReorderableList _reorderableList)
        {
            // 정말로 삭제할 것인지에 대한 alert callback
            _reorderableList.onRemoveCallback = (ReorderableList l) =>
            {
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the UIData?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                }
            };
        }

        void OnAddCallback(UIRootType _rootType, ReorderableList _reorderableList)
        {
            // +버튼을 눌러 새로운 요소들을 추가 했을 때 초기화 callback

            _reorderableList.onAddCallback = (ReorderableList l) =>
            {
                var index = l.serializedProperty.arraySize; l.serializedProperty.arraySize++;
                l.index = index;
                var element = l.serializedProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative("name").stringValue = "(UIName)";
                element.FindPropertyRelative("prefabName").stringValue = "(PrefabName)";
                element.FindPropertyRelative("uiRootType").enumValueIndex = (int)_rootType;
            };
        }
    }
}
