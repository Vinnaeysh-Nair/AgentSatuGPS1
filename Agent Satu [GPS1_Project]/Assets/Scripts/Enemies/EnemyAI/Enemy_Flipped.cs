using UnityEngine;

public class Enemy_Flipped : MonoBehaviour
{
    private Transform playerBody;
    public bool isFacingRight = false;

    void Start()
    {
        playerBody = transform.Find("/Player/PlayerBody").GetComponent<Transform>();
    }
    
    public void LookAtPlayer()
    {
        float dirX = playerBody.position.x - transform.position.x;
        float ignoreOffset = .1f;                                       //to offset inaccuracy caused by gameObjects' center points

        if (dirX > 0f + ignoreOffset && !isFacingRight)
        {
            Flip();
        }
        else if (dirX <= 0f - ignoreOffset && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
