using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class REJ_EnemyAI_Melee : MonoBehaviour
{
    //Components
    private PlayerMovement playerMovement;
    
    //Fields
    public Transform[] patrolPoints;
   // private int currentPointIndex = 0;
    
    [SerializeField] private float speed = 1f;
    private bool facingRight = false;

    private bool detected = false;
    public bool isDead = false;

    //Patrol point system
    private int nextPosIndex;
    private Transform nextPos;

 
    void Start()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();
        
        nextPos = patrolPoints[0];
    }

    private void OnDisable()
    {
        isDead = true;
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        
        if (!detected)
        {
            Patrol();
        }
        else
        {
            FollowPlayer();
        }
   
    }

    public void Patrol()
    {
        if (transform.position.x == nextPos.position.x)
        {
            Flip();
            nextPosIndex++;

            if (nextPosIndex >= patrolPoints.Length)
            {
                nextPosIndex = 0;
                transform.eulerAngles = new Vector2(0f, 0f);
            }

            nextPos = patrolPoints[nextPosIndex];
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextPos.position.x, transform.position.y), speed * Time.fixedDeltaTime);
        }
    }
    

    public void FollowPlayer()
    {
        Vector2 enemyPos = transform.position;
        Vector2 playerPos = playerMovement.GetPlayerPos();
        float dirX = playerPos.x - enemyPos.x;

        if (dirX < 0f)
        {
            facingRight = false;
            transform.eulerAngles = new Vector2(0f, 0f);
        }
        else if (dirX > 0f)
        {
            facingRight = true;
            transform.eulerAngles = new Vector2(0f, 180f);
        }
        
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, transform.position.y), speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detected = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SetDetectedToFalse());
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    private IEnumerator SetDetectedToFalse()
    {
        yield return new WaitForSeconds(3f);
        detected = false;
  
        Flip();
        nextPosIndex = 0;
    }
}
