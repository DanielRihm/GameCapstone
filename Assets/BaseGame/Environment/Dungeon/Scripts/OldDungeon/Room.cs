using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // List to keep track of all doors in this room
    public List<DoorMarker> doorMarkers = new List<DoorMarker>();

    // Method to register a door in this room
    public void RegisterDoor(DoorMarker door)
    {
        if (!doorMarkers.Contains(door))
        {
            doorMarkers.Add(door);
            Debug.Log($"Registered door: {door.gameObject.name} with direction: {door.doorDirection}");
        }
    }

    private void Start()
    {
        Debug.Log($"Room {gameObject.name} has started.");

        // Fetch doors from deeper nested children as well
        DoorMarker[] doors = GetComponentsInChildren<DoorMarker>(true); // 'true' includes inactive children
        foreach (DoorMarker door in doors)
        {
            RegisterDoor(door);
        }

        if (doorMarkers.Count == 0)
        {
            Debug.LogWarning($"No doors found in room: {gameObject.name}");
        }

        // Disable the Collider, it destroys the bullets.
        var collider = GetComponent<Collider>();
        if(collider != null)
        {
            collider.enabled = false;
        }
    }
}
