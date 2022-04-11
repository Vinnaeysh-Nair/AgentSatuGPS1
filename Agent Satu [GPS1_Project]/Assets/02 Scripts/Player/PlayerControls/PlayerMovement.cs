using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private PlayerController controller;

    //General movement fields
    public static float horizontalMove;
    private bool jump = false;
    private bool crouch = false;
    private bool dodgeroll = false;

    
    public delegate void OnInteract();
    public static event OnInteract onInteractDelegate;

    
    private FallOffMap _fallOffMap;
    
    
    private Vector2 playerPos;


    
    //Getters
    public Vector2 GetPlayerPos()
    {
        return playerPos;
    }
    
    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void Start()
    {
        _fallOffMap = FallOffMap.Instance;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }


        if (Input.GetButtonDown("Dodgeroll"))
        {
            dodgeroll = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;

        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (onInteractDelegate != null)
            {
                onInteractDelegate.Invoke();
            }
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove, jump, crouch, dodgeroll);
        jump = false;
        dodgeroll = false;

        playerPos = transform.position;

        if (_fallOffMap == null) return;
        if (_fallOffMap.IsOutOfMap(playerPos.y))
        {
            _fallOffMap.FallToDeath();
        }
    }
}
