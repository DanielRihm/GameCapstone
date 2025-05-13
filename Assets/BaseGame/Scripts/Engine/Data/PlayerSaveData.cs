using System;
using System.IO;
using System.Reflection;
using Assets.BaseGame.Scripts.Engine.Data;
using UnityEngine;

namespace LCPS.SlipForge.Engine
{
    public static class PlayerSaveData
    {
        private static string GetSaveFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        }

        public static void SaveData<T>(T source) where T : class
        {
            string json = JsonUtility.ToJson(source);
            var path = GetSaveFilePath(typeof(T).Name);
            File.WriteAllText(path, json);
        }

        public static void LoadData<T>(T target) where T : class
        {
            string filePath = GetSaveFilePath(typeof(T).Name);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                // Deserialize JSON to a struct
                T saveData = JsonUtility.FromJson<T>(json);

                // Trigger any observers
                CopyFields(saveData, target);
            }
            else
            {
                Debug.LogWarning($"Save file not found {typeof(T).Name}. Creating new save file.");
                SaveData(target);
            }
        }

        private static void CopyFields(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var fields = sourceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var sourceValue = field.GetValue(source);
                var targetField = targetType.GetField(field.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (targetField == null) continue;

                var targetValue = targetField.GetValue(target);

                // Cause existing Observable subscribers to get notified
                if (sourceValue is IObservable observableSource && targetValue is IObservable observableTarget)
                {
                    // Copy the Value property of the Observable
                    var valueProperty = observableSource.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (valueProperty != null)
                    {
                        var value = valueProperty.GetValue(observableSource);
                        valueProperty.SetValue(observableTarget, value);
                    }
                }
                else if (sourceValue != null && targetValue != null && sourceValue.GetType().IsClass && !sourceValue.GetType().IsPrimitive && sourceValue.GetType() != typeof(string))
                {
                    // Recursively copy fields for nested objects
                    CopyFields(sourceValue, targetValue);
                }
                else
                {
                    // Copy the field as usual
                    targetField.SetValue(target, sourceValue);
                }
            }
        }
    }
}