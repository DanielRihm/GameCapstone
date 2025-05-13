
using UnityEngine;
using LCPS.SlipForge.Engine;
using System.Reflection;



#if UNITY_EDITOR
using UnityEditor;

namespace LCPS.SlipForge
{
    //[CustomEditor(typeof(MonoBehaviour), true)]
    public class ObservableAwareEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw all properties normally
            DrawDefaultInspector();

            // Update serialized object (to ensure we have the latest values)
            serializedObject.Update();

            // Iterate over all fields in the target object
            var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                // Check if the field is an IObservable<T>
                if (typeof(IObservable).IsAssignableFrom(field.FieldType))
                {
                    // Get the observable instance from the field
                    var observableInstance = field.GetValue(target) as IObservable;

                    if (observableInstance != null)
                    {
                        // Manually handle change detection and notification
                        HandleObservablePropertyChange(field, observableInstance);
                    }
                }
            }

            // Apply any modified properties
            serializedObject.ApplyModifiedProperties();
        }

        private void HandleObservablePropertyChange(FieldInfo field, IObservable observableInstance)
        {
            // Get the SerializedProperty that represents the observable's value
            SerializedProperty valueProperty = serializedObject.FindProperty(field.Name).FindPropertyRelative("value");

            if (valueProperty != null)
            {
                // Begin listening for changes in the property
                EditorGUI.BeginChangeCheck();

                // Draw the property normally in the Inspector
                EditorGUILayout.PropertyField(valueProperty, new GUIContent(field.Name), true);

                // If a change was detected, apply modified properties and force notify
                if (EditorGUI.EndChangeCheck())
                {
                    // Apply modified properties to make sure the current value is up-to-date
                    serializedObject.ApplyModifiedProperties();

                    // Notify observers that the value has changed
                    observableInstance.Notify();
                }
            }
        }
    }

}

#endif