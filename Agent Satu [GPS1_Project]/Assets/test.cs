using UnityEngine;

public class test : MonoBehaviour
{
    public PlayerMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerMovement.GetPlayerPos().x, 0f), 1 * Time.deltaTime);
    }
}
