using UnityEngine;

namespace LCPS.SlipForge
{
    public class RoomSpawner : MonoBehaviour
    {
        public GameObject[] roomPrefabs;   // Drag your Room1, Room2, etc. prefabs here in the Inspector
        public Transform[] spawnPoints;    // Assign spawn points in the Inspector

        void Start()
        {
            SpawnAllRooms();
        }

        void SpawnAllRooms()
        {
            // Loop through all room prefabs and spawn them
            for (int i = 0; i < roomPrefabs.Length; i++)
            {
                GameObject roomPrefab = roomPrefabs[i];

                if (roomPrefab != null)
                {
                    // Use different spawn points for each room, or repeat if fewer spawn points
                    int spawnIndex = Mathf.Min(i, spawnPoints.Length - 1);
                    Instantiate(roomPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation, transform);
                }
                else
                {
                    Debug.LogError($"Room prefab at index {i} is missing.");
                }
            }
        }
    }
}