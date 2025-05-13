using LCPS.SlipForge;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Input))]
public class PlayerController : MonoBehaviour
{
    // Singleton to expose the player controller to other scripts
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public ActionMap ActionMap => inputScheme;

    //Create ActionMap
    private ActionMap inputScheme;

    // Start is called before the first frame update
    SpriteRenderer PlayerSpriteRen;
    void Start()
    {
        PlayerSpriteRen = gameObject.GetComponentInChildren<SpriteRenderer>();

        //Enable movement
        inputScheme = new ActionMap();
        inputScheme.Enable();
    }
    public Sprite down;
    public Sprite left;
    public Sprite up;
    public Sprite right;
    float moveSpeed = 0.05f;
    // Update is called once per frame
    void Update()
    {
        //Read in movement vector values
        Vector2 moveInput = inputScheme.Player.Move.ReadValue<Vector2>();

        Vector3 moveVector = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += moveVector * moveSpeed;
        if (moveVector.magnitude <= 0)
        {
            PlayerSpriteRen.sprite = down;
        }
        else if (Mathf.Abs(moveVector.x) >= Mathf.Abs(moveVector.z))
        {
            PlayerSpriteRen.sprite = (moveVector.x > 0 ? right : left);
        }
        else if (Mathf.Abs(moveVector.x) <= Mathf.Abs(moveVector.z))
        {
			PlayerSpriteRen.sprite = (moveVector.z > 0 ? up : down);
		}
    }

    public IEnumerator OnDodge()
    {
        Vector2 moveInput = inputScheme.Player.Move.ReadValue<Vector2>();
        for (int i = 0; i < 30; i++)
        {
            print("Dodge " + i);
            yield return null;
        }
    }

    public void OnInteract()
    {
        print("Interact");
    }

    public void OnAbility()
    {
        print("Ability");
    }

    public void OnAttack1()
    {
        print("Attack1");
    }

    public void OnAttack2()
    {
        print("Attack2");
    }
}
