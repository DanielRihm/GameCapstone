using UnityEngine;
using LCPS.SlipForge;
using UnityEngine.AI;

public class DungeonDoor : TriggerActionInvoker
{
    public Vector3 ToPosition;
    public GameObject ToRoom;
    private GameObject _player;

    void Start()
    {
        _player = PlayerInstance.Instance.gameObject;
        OnEnter += OnObjectEnter;
    }

    void OnObjectEnter(TriggerActionInvoker invoker, Collider collider, string name)
    {
        if (this != invoker) return;

        _player = collider.gameObject;
    }
    void OnDestroy()
    {
        OnEnter -= OnObjectEnter;
    }

    public override void Interact()
    {
        var _script = ToRoom.GetComponent<DungeonFloor>();
        DungeonManager.Instance.SwitchRoom(_script.Location);
        _player.transform.parent = ToRoom.transform;
        var agent = _player.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(new Vector3(ToPosition.x + ToRoom.transform.position.x, _player.transform.position.y, ToPosition.z + ToRoom.transform.position.z));
        }
    }
}
