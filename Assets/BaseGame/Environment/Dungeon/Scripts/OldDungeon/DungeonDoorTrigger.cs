
using LCPS.SlipForge;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


public class DungeonDoorTrigger : TriggerActionInvoker
{
    private GameObject _player;
    [SerializeField]
    private LayerMask DoorMask;

    public Transform TeleportingTo;

    public override void Interact()
    {
        // Sphere cast for the closest door
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f, DoorMask);

        // Look for s DoorMarker
        DungeonDoorTrigger closestMarker = null;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent(out DungeonDoorTrigger doorTrigger))
            {
                // Make sure the door is not this door and is the closest door
                // The door should also have a different parrent
                if (doorTrigger != this && transform.parent != doorTrigger.transform.parent )
                {
                    if (closestMarker == null)
                    {
                        closestMarker = doorTrigger;
                    }
                    else if (Vector3.Distance(doorTrigger.transform.position, transform.position) < Vector3.Distance(closestMarker.transform.position, transform.position))
                    {
                        closestMarker = doorTrigger;
                    }
                }
            }
        }

        // If we found a door, move the player to the door
        if (closestMarker != null)
        {
            TeleportingTo = closestMarker.transform;

            // Parent the player to the next room
            _player.transform.parent = closestMarker.transform.parent;

            // Warp the Nav Mesh Agent
            var agent = _player.GetComponent<NavMeshAgent>();
            if(agent != null)
            {
                agent.Warp(closestMarker.transform.position + closestMarker.transform.forward * 2);
            }
        }
    }

}
