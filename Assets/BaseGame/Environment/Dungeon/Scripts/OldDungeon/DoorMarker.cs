using UnityEngine;
using LCPS.SlipForge;

public class DoorMarker : MonoBehaviour
{
    //Enum for door direction
    public enum DoorDirection { North, East, South, West }

    // Assign a direction to this door (optional, can be expanded upon later)
    public DoorDirection doorDirection;

    // Reference to whether this door is registered
    public bool isRegistered = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        //register this door in the parent room if applicable
        Room parentRoom = GetComponentInParent<Room>();
        if (parentRoom != null && !isRegistered)
        {
            parentRoom.RegisterDoor(this);
            isRegistered = true; // Mark the door as registered
        }
    }

}
