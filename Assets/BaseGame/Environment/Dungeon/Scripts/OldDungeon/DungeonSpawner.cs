using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    // Reference to the BossRoomSouth prefab
    public GameObject bossRoomSouthPrefab;

    // List of room prefabs to choose from
    [SerializeField]
    private List<GameObject> roomPrefabs;

    // Spawn point for the BossRoomSouth
    public Vector3 bossRoomSpawnPoint;

    private void Start()
    {
        // Spawn the BossRoomSouth
        GameObject bossRoom = Instantiate(bossRoomSouthPrefab, bossRoomSpawnPoint, Quaternion.identity, transform);

        // Find a room from the list that has a north door
        GameObject roomWithNorthDoor = FindRoomWithNorthDoor();
        if (roomWithNorthDoor != null)
        {
            // Align and spawn the room with the north door below the boss room
            GameObject nextRoom = SpawnAndAlignRoom(bossRoom, roomWithNorthDoor);

            if (nextRoom != null)
            {
                Debug.Log($"Spawned room: {nextRoom.name}, aligned below the BossRoomSouth.");
            }
        }
    }

    // Method to find a room prefab that contains a north-facing door
    private GameObject FindRoomWithNorthDoor()
    {
        foreach (GameObject roomPrefab in roomPrefabs)
        {
            DoorMarker[] doorMarkers = roomPrefab.GetComponentsInChildren<DoorMarker>(true); // 'true' to include inactive children
            foreach (DoorMarker doorMarker in doorMarkers)
            {
                if (doorMarker.doorDirection == DoorMarker.DoorDirection.North)
                {
                    Debug.Log($"Found a room with a north-facing door: {roomPrefab.name}");
                    return roomPrefab;
                }
            }
        }

        Debug.LogWarning("No room with a north-facing door found in the prefabs list.");
        return null;
    }

    // Method to spawn and align the room with the north-facing door below the boss room
    private GameObject SpawnAndAlignRoom(GameObject bossRoom, GameObject roomPrefab)
    {
        // Spawn the next room below the boss room
        GameObject nextRoom = Instantiate(roomPrefab);
        nextRoom.transform.parent = transform;

        // Get the box colliders of both rooms
        BoxCollider bossRoomCollider = bossRoom.GetComponent<BoxCollider>();
        BoxCollider nextRoomCollider = nextRoom.GetComponent<BoxCollider>();

        // Calculate the size of both rooms along the Z-axis for north-south connection
        float bossRoomZSize = bossRoomCollider.bounds.size.z;
        float nextRoomZSize = nextRoomCollider.bounds.size.z;

        // Align the room with the north door below the boss room
        Vector3 spawnPosition = bossRoom.transform.position - new Vector3(0, 0, (bossRoomZSize / 2) + (nextRoomZSize / 2));
        nextRoom.transform.position = spawnPosition;

        return nextRoom;
    }
}
