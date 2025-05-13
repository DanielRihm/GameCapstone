# if UNITY_EDITOR
namespace LCPS.SlipForge.Engine.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.EditorTools;
    using System;
    using System.Collections.Generic;
    using UnityEditor.ShortcutManagement;

    public static class PersistantSingletonLauncher
    {
        [MenuItem("Tools/Persistant Singleton Inspector")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<PersistantSingletonWindow>();
            window.titleContent = new GUIContent("Singleton Inspector");
            window.Show();
        }

        [Shortcut("Activate Persistant Singleton Tool", typeof(SceneView), KeyCode.P)]
        public static void ActivatePersistantSingletonTool()
        {
            ShowWindow();
        }
    }

    public class PersistantSingletonWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        private List<MonoBehaviour> _singletonInstances;
        private int _selectedTab;

        private void OnEnable()
        {
            FindSingletons();
        }

        private void OnHierarchyChange()
        {
            FindSingletons();
        }

        private void OnGUI()
        {
            if (_singletonInstances == null || _singletonInstances.Count == 0)
            {
                EditorGUILayout.LabelField("No Persistant Singletons found in the scene.");
                return;
            }

            // Create tabs for each singleton instance
            string[] tabNames = new string[_singletonInstances.Count];
            for (int i = 0; i < _singletonInstances.Count; i++)
            {
                if (_singletonInstances[i] != null)
                {
                    tabNames[i] = _singletonInstances[i].GetType().Name;
                }
                else
                {
                    tabNames[i] = "Unknown Singleton";
                }
            }

            _selectedTab = GUILayout.Toolbar(_selectedTab, tabNames);

            if (_selectedTab >= 0 && _selectedTab < _singletonInstances.Count)
            {
                MonoBehaviour selectedSingleton = _singletonInstances[_selectedTab];
                if (selectedSingleton != null)
                {
                    _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField(selectedSingleton.GetType().Name, EditorStyles.boldLabel);

                    // Draw the individual fields of the selected singleton as it would appear in the inspector
                    SerializedObject serializedObject = new SerializedObject(selectedSingleton);
                    SerializedProperty property = serializedObject.GetIterator();
                    property.NextVisible(true); // Skip script reference
                    while (property.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }
                    serializedObject.ApplyModifiedProperties();

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndScrollView();
                }
            }
        }

        private void FindSingletons()
        {
            _singletonInstances = new List<MonoBehaviour>();

            // Find all objects of type MonoBehaviour in the scene
            MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();
            foreach (var monoBehaviour in allMonoBehaviours)
            {
                Type type = monoBehaviour.GetType();
                // Check if the type inherits from PersistantSingleton<T>
                while (type != null && type != typeof(MonoBehaviour))
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PersistantSingleton<>))
                    {
                        _singletonInstances.Add(monoBehaviour);
                        break;
                    }
                    type = type.BaseType;
                }
            }
        }
    }
}
#endif