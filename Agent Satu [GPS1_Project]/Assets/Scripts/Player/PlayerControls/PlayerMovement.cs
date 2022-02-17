using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private PlayerController controller;
    public new SpriteRenderer renderer;

    //General movement fields
    private float horizontalMove;
    private bool jump = false;
    private bool crouch = false;
    private bool dodgeroll = false;

    private Vector2 playerPos;
    
    //Getters
    public float GetSpeed()
    {
        //used for running anim
        return horizontalMove;
    }
    
    public bool GetCrouch()
    {
        return crouch;
    }

    public Vector2 GetPlayerPos()
    {
        return playerPos;
    }
    
    public float GetPlayerHeight()
    {
        return renderer.sprite.bounds.size.y;
    }

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        renderer = GetComponent<SpriteRenderer>();
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
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove, jump, crouch, dodgeroll);
        jump = false;
        dodgeroll = false;
        
        playerPos = transform.position;
    }
}
