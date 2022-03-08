using UnityEngine;

public class EnemyAI_Melee : MonoBehaviour
{
    //Components
    private Enemy_Agro enemyAgro;
    private Transform playerBody;
    private Rigidbody2D rb;


    
    //Fields
    [SerializeField] private float YRangeToStopChasing;
    [SerializeField] private float movementSpeed;

    private float ignoreOffset = .5f;
    private Vector2 playerPos;
    private Vector2 enemyPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAgro = GetComponent<Enemy_Agro>();
    }
    void FixedUpdate()
    {
        if (enemyAgro.GetDetected()) 
        {
            playerPos = enemyAgro.GetPlayerPos();
            enemyPos = transform.position;
            Move();
        }
        else
        {
            StopChasing();
        }
    }
    
    private void Move()
    {
        ChasePlayer();
        
        //If player higher than enemy for a specific range, stop chasing
        float distY = Mathf.Abs(playerPos.y - enemyPos.y);
        if (distY > YRangeToStopChasing)
        {
            StopChasing();
        }
            
        //If reached player's X-pos, stop chasing
        float distX = Mathf.Abs(playerPos.x - enemyPos.x) ;
        if (distX <= ignoreOffset)                                  //to offset inaccuracy caused by gameObjects' center points
        {
            StopChasing();
        }
    }
    
    private void ChasePlayer()
    {
        //enemy is to the left so move right
        if (transform.position.x < playerPos.x)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            //AttackPlayer();
        }
        //enemy is to the left so move right
        else if (transform.position.x > playerPos.x)
        {
            rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
            //AttackPlayer();
        }
    }

    private void StopChasing()
    {
        rb.velocity = new Vector2(0f, 0f);
    }
}
