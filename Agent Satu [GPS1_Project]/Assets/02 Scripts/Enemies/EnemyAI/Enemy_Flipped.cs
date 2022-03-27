using UnityEngine;

public class Enemy_Flipped : MonoBehaviour
{
    private Transform playerBody;
    [SerializeField] private bool isFacingRight = false;

    public bool GetIsFacingRight()
    {
        return isFacingRight;
    }
    void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        
        if (isFacingRight)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
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

    //by jake
    //public bool detectfacingdirection()
    //{
    //    return isfacingright;
    //}
}
