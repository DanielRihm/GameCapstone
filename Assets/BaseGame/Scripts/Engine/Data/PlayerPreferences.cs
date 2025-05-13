using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.BaseGame.Engine.Data
{
    public static class PlayerPreferences
    {
        /// <summary>
        /// Attempt to load settings from PlayerPrefs for the specific struct type. If no settings are found, return the original struct.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The struct to update with loaded values</param>
        /// <returns>The updated struct with loaded values, or the original struct if no settings were found</returns>
        public static T LoadSettings<T>(T data) where T : struct
        {
            string typeName = typeof(T).FullName;

            foreach (var field in typeof(T).GetFields())
            {
                string key = $"{typeName}.{field.Name}";
                if (PlayerPrefs.HasKey(key))
                {
                    try
                    {
                        string base64String = PlayerPrefs.GetString(key);
                        byte[] bytes = Convert.FromBase64String(base64String);

                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            var fieldValue = formatter.Deserialize(ms);
                            field.SetValueDirect(__makeref(data), fieldValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to load setting for key {key}: {ex.Message}");
                    }
                }
            }
            return data;
        }

        public static void SaveSettings<T>(T data) where T : struct
        {
            string typeName = typeof(T).FullName;

            foreach (var field in typeof(T).GetFields())
            {
                string key = $"{typeName}.{field.Name}";
                var fieldValue = field.GetValue(data);

                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(ms, fieldValue);
                        byte[] bytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(bytes);
                        PlayerPrefs.SetString(key, base64String);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to save setting for key {key}: {ex.Message}");
                }
            }
        }
    }
}
