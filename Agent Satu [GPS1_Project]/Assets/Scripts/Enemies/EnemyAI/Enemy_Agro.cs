using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Agro : MonoBehaviour
{
    Enemy_Fliped enemflip;
    [SerializeField] Transform player;
    [SerializeField] private float AgroRange;
    [SerializeField] private float speed;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemflip = GetComponent<Enemy_Fliped>();
    }

    // Update is called once per frame
    void Update()
    {
        enemflip.LookAtPlayer();

        //Distance to player
        float DistToPlayer = Vector2.Distance(transform.position, player.position);

        if (DistToPlayer < AgroRange)
        {
            ChasePlayer();
        }
        else
        {
            BackToIdle();
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x <= player.position.x)
        {
            //enemy is to the left so move right
            rb.velocity = new Vector2(speed, 0);
            //AttackPlayer();
        }
        else if (transform.position.x > player.position.x)
        {
            //enemy is to the left so move right
            rb.velocity = new Vector2(-speed, 0);
            //AttackPlayer();
        }
    }

    private void BackToIdle()
    {
        rb.velocity = new Vector2(0, 0);
    }
}
