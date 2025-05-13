using LCPS.SlipForge;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace LCPS.SlipForge.Enemy
{
    [CustomEditor(typeof(EnemyMovementSet))]
    public class EnemyMovementSetEditor : Editor
    {
        private SerializedProperty behaviorsProperty;

        private void OnEnable()
        {
            behaviorsProperty = serializedObject.FindProperty("behaviors");
        }

        public override void OnInspectorGUI()
        {
            // Fetch the EnemyMovementSet reference
            EnemyMovementSet movementSet = (EnemyMovementSet)target;

            // Hide the default behavior list field
            serializedObject.Update();

            EditorGUILayout.Space();

            // Draw buttons for adding/removing behaviors
            DrawBehaviorManagementButtons(movementSet);

            // Draw the range overlaps for each behavior
            DrawRangeOverlaps(movementSet);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBehaviorManagementButtons(EnemyMovementSet movementSet)
        {
            EditorGUILayout.LabelField("Manage Pursuit Behaviors", EditorStyles.boldLabel);

            // Button to create a new PursuitBehavior
            if (GUILayout.Button("Add Pursuit Behavior"))
            {
                ShowPursuitBehaviorPicker(movementSet);
            }

            EditorGUILayout.Space();
        }

        private void ShowPursuitBehaviorPicker(EnemyMovementSet movementSet)
        {
            // Get all types that inherit from PursuitBehavior
            List<Type> behaviorTypes = GetAllPursuitBehaviorTypes();

            // Create a generic menu for selecting a behavior type
            GenericMenu menu = new GenericMenu();

            foreach (Type type in behaviorTypes)
            {
                // Add each type as a menu item
                menu.AddItem(new GUIContent(type.Name), false, () => CreateNewPursuitBehavior(movementSet, type));
            }

            // Show the menu
            menu.ShowAsContext();
        }

        private List<Type> GetAllPursuitBehaviorTypes()
        {
            // Use reflection to get all types that inherit from PursuitBehavior
            var behaviorTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 from type in assembly.GetTypes()
                                 where type.IsSubclassOf(typeof(PursuitBehavior)) && !type.IsAbstract
                                 select type).ToList();
            return behaviorTypes;
        }

        private void CreateNewPursuitBehavior(EnemyMovementSet movementSet, Type behaviorType)
        {
            // Create an instance of the selected behavior type
            PursuitBehavior newBehavior = (PursuitBehavior)CreateInstance(behaviorType);

            // Name the behavior by type
            newBehavior.name = behaviorType.Name;

            // TODO: Add an integer if there is more than one

            // Add the new behavior as a sub-asset of the movement set
            AssetDatabase.AddObjectToAsset(newBehavior, movementSet);
            AssetDatabase.SaveAssets();

            // Add the behavior to the list
            movementSet.Behaviors.Add(newBehavior);

            // Mark the movement set as dirty to ensure it gets saved
            EditorUtility.SetDirty(movementSet);
        }

        private void DrawRangeOverlaps(EnemyMovementSet movementSet)
        {
            EditorGUILayout.LabelField("Range Blending", EditorStyles.boldLabel);

            if (movementSet.Behaviors == null)
            {
                return;
            }

            for (int i = 0; i < movementSet.Behaviors.Count; i++)
            {
                PursuitBehavior behavior = movementSet.Behaviors[i];
                if (behavior == null) continue;

                EditorGUILayout.BeginHorizontal();

                // Add a bold heading label showing the behavior type (e.g., ZigZagBehavior, RushBehavior)
                EditorGUILayout.LabelField(behavior.GetType().Name, EditorStyles.boldLabel);

                // Add delete button
                if (GUILayout.Button("Delete", GUILayout.MaxWidth(60)))
                {
                    RemovePursuitBehavior(movementSet, i);

                    continue;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                string newName = EditorGUILayout.TextField(behavior.name, GUILayout.MaxWidth(120));


                // Check if the name has changed and update it
                if (newName != behavior.name)
                {
                    behavior.name = newName;

                    // Update the asset database to reflect the name change
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(behavior));
                }

                // Float field for the minimum range on the left
                float minRange = Mathf.Round(EditorGUILayout.FloatField(behavior.MinRange, GUILayout.MaxWidth(50)) * 100f) / 100f;

                // Draw the min-max slider in the middle
                EditorGUILayout.MinMaxSlider(ref minRange, ref behavior.MaxRange, 0f, 40f);

                // Float field for the maximum range on the right
                float maxRange = EditorGUILayout.FloatField(behavior.MaxRange, GUILayout.MaxWidth(50));

                // Ensure that the ranges are clamped properly
                minRange = Mathf.Clamp(minRange, 0f, behavior.MaxRange);
                maxRange = Mathf.Clamp(maxRange, minRange, 40f);

                // Round the values to a reasonable precision.
                minRange = Mathf.Round(minRange * 100f) / 100f;
                maxRange = Mathf.Round(maxRange * 100f) / 100f;

                // Update the behavior's ranges
                behavior.MinRange = minRange;
                behavior.MaxRange = maxRange;

                EditorGUILayout.EndHorizontal();

                // Add an AnimationCurve editor for controlling the weight based on the distance
                behavior.Curve = EditorGUILayout.CurveField(behavior.Curve);

                // Display additional properties that are part of the subclass
                DrawAdditionalProperties(behavior);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }

        private void RemovePursuitBehavior(EnemyMovementSet movementSet, int index)
        {
            if (index < movementSet.Behaviors.Count && index >= 0)
            {
                PursuitBehavior behavior = movementSet.Behaviors[index];

                // Remove the behavior from the list
                movementSet.Behaviors.RemoveAt(index);

                // Remove the sub-asset from the asset database
                DestroyImmediate(behavior, true);
                AssetDatabase.SaveAssets();

                // Mark the movement set as dirty
                EditorUtility.SetDirty(movementSet);
            }
        }

        private void DrawAdditionalProperties(PursuitBehavior behavior)
        {
            // Create a serialized object for the behavior
            SerializedObject serializedBehavior = new SerializedObject(behavior);
            serializedBehavior.Update();

            // Iterate through all properties of the object
            SerializedProperty property = serializedBehavior.GetIterator();
            property.NextVisible(true); // Skip the first element (script reference)

            while (property.NextVisible(false))
            {
                // Only display properties that are not part of the base class (PursuitBehavior)
                if (property.name != nameof(PursuitBehavior.MinRange)
                    && property.name != nameof(PursuitBehavior.MaxRange)
                    && property.name != nameof(PursuitBehavior.Curve))
                {
                    EditorGUILayout.PropertyField(property, true);
                }
            }

            serializedBehavior.ApplyModifiedProperties();
        }
    }
}

#endif