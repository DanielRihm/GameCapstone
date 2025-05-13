using System.Collections;
using UnityEngine;

public class CameraAttachToPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform Player;

    public float FollowDistance = 5f;

    private void Start()
    {
        // Create 45 degree camera rotation
        var rotation = Quaternion.AngleAxis(45, Vector3.right);
        transform.rotation = rotation;


        if(Player == null)
        {
            StartCoroutine(LookForPlayer());
        }
    }

    IEnumerator LookForPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        while (player == null)
        {
            yield return new WaitForSeconds(0.5f);
            player = GameObject.FindGameObjectWithTag("Player");
        }

        Player = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player == null)
            return;

        transform.position = Player.transform.position + new Vector3(0, FollowDistance, -FollowDistance);
    }
}
