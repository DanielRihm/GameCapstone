#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace LCPS.SlipForge.Engine
{
    [CustomPropertyDrawer(typeof(Observable<>))]
    [CustomPropertyDrawer(typeof(ObservableList<>))]
    public class ObservablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("value");

            if (valueProperty == null)
            {
                return;
            }

            // Add padding
            float padding = 4f;
            Rect paddedPosition = new Rect(
                position.x,
                position.y + padding,
                position.width,
                position.height - padding * 2
            );

            // Background to make the observables distinct
            EditorGUI.DrawRect(paddedPosition, new Color(0.9f, 0.9f, 1.0f, 0.2f));

            // Adjust position for the input field to keep it centered within the background
            Rect inputPosition = new Rect(
                paddedPosition.x + padding,
                paddedPosition.y + padding,
                paddedPosition.width - padding * 2,
                paddedPosition.height - padding * 2
            );

            // Handle custom attributes
            var rangeAttribute = fieldInfo.GetCustomAttribute<ObservableRangeAttribute>();
            var textAreaAttribute = fieldInfo.GetCustomAttribute<ObservableTextAreaAttribute>();

            EditorGUI.BeginChangeCheck();

            if (rangeAttribute != null)
            {
                if (valueProperty.propertyType == SerializedPropertyType.Float)
                {
                    EditorGUI.Slider(inputPosition, valueProperty, rangeAttribute.Min, rangeAttribute.Max, label);
                }
                else if (valueProperty.propertyType == SerializedPropertyType.Integer)
                {
                    EditorGUI.IntSlider(inputPosition, valueProperty, (int)rangeAttribute.Min, (int)rangeAttribute.Max, label);
                }
                else
                {
                    EditorGUI.PropertyField(inputPosition, valueProperty, label);
                }
            }
            else if (textAreaAttribute != null)
            {
                // Draw the label
                EditorGUI.LabelField(new Rect(position.x + 4f, position.y + 6f, position.width, EditorGUIUtility.singleLineHeight), label);

                // Adjust input position for the text area
                inputPosition.y += EditorGUIUtility.singleLineHeight + padding;
                inputPosition.height = EditorGUIUtility.singleLineHeight * textAreaAttribute.MinLines;

                // Draw the text area
                valueProperty.stringValue = EditorGUI.TextArea(inputPosition, valueProperty.stringValue);
            }
            else
            {
                EditorGUI.PropertyField(inputPosition, valueProperty, label);
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                NotifyObservable(property);
            }
        }

        private void NotifyObservable(SerializedProperty property)
        {
            object targetObject = property.serializedObject.targetObject;
            var fieldNames = property.propertyPath.Split('.');

            foreach (var fieldName in fieldNames)
            {
                var field = targetObject.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null)
                {
                    return;
                }
                targetObject = field.GetValue(targetObject);
            }

            if (targetObject is IObservable observableInstance)
            {
                observableInstance.Notify();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("value");
            var textAreaAttribute = fieldInfo.GetCustomAttribute<ObservableTextAreaAttribute>();

            if (textAreaAttribute != null)
            {
                return EditorGUIUtility.singleLineHeight * (textAreaAttribute.MinLines + 2) + 8f; // Add padding
            }

            return valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) + 20 : base.GetPropertyHeight(property, label);
        }
    }
}
#endif