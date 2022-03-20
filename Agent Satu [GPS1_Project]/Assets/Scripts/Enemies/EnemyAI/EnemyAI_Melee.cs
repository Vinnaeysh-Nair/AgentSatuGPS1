using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI_Melee : MonoBehaviour
{
    //Components
    private Enemy_Agro enemyAgro;
    private Transform playerBody;
    private Rigidbody2D rb;


    [SerializeField] private Animator anim;
    private PlayerHpSystem _playerHpSystem;

    
    //Fields
    [Header("Movement")]
    [SerializeField] private float YRangeToStopChasing;
    [SerializeField] private float movementSpeed;


    [Header("Attack")] 
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float attackAreaOffset;
    [SerializeField] private float attackAreaSize;
    [SerializeField] [Range(0f, 3f)] private float startAtkDistX = 0f;
    

    [SerializeField] private LayerMask playerHitLayer;

    [SerializeField] private AudioClip MeleeSound;
     private AudioSource AD;

    private float ignoreOffset = .5f;
    private Vector2 playerPos;
    private Vector2 enemyPos;

    void Start()
    {
        AD = GetComponent<AudioSource>();
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

        Vector2 normalizedVelocity = rb.velocity.normalized;
        anim.SetFloat("Speed", Mathf.Abs(normalizedVelocity.x));
    }
    
    private void Move()
    {
        ChasePlayer();
        anim.SetBool("IsAttacking", false);
        
        //If player higher than enemy for a specific range, stop chasing
        float distY = Mathf.Abs(playerPos.y - enemyPos.y);
        if (distY > YRangeToStopChasing)
        {
            StopChasing();
        }
            
        //If reached player's X-pos, stop chasing
        float distX = Mathf.Abs(playerPos.x - enemyPos.x) ;
        if (distX <= ignoreOffset + startAtkDistX)                                  //to offset inaccuracy caused by gameObjects' center points
        {
            StopChasing();
            anim.SetBool("IsAttacking", true);
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

    //Used in Animation timeline
    public void DamagePlayer()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(attackAreaOffset, 0f, 0f ), attackAreaSize, playerHitLayer);

        if (hitPlayer != null)
        {
            AD.PlayOneShot(MeleeSound);
            Transform playerRoot = hitPlayer.transform.root;
            
            if (_playerHpSystem == null)
            {
                //Locate PlayerBody to get hp system script
                _playerHpSystem = playerRoot.GetChild(0).GetComponent<PlayerHpSystem>();
            }
            _playerHpSystem.TakeDamage(damageToPlayer);
        }
    }
}
