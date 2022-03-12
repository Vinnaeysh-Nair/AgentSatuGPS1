using System;
using UnityEngine;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class BulletBehaviour : MonoBehaviour
{
    //Components
    private TagManager tagManager;
    private Rigidbody2D rb;
    private PlayerHpSystem playerHp;
    private ObjectPooler pooler;

    //Fields
    [Header("General")]
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private GameObject impactEffect;
    
    [Header("Force on enemy when dismembering and ragdolling")]
    [SerializeField] [Range(0f, 1f)] private float forceDampening = 1f;
    [SerializeField] private int bulletDamage = 1;
    private bool hitRegistered = false;
    
    //Wall deflection
    [Header("Bullet Deflection")] 
    [SerializeField] private int maxDeflectionTimes;
    private Vector2 prevVelocity;
    private int deflectedTimes;

    public Vector2 GetPrevVelocity()
    {
        return prevVelocity;
    }
    
    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        playerHp = transform.Find("/Player/PlayerBody").GetComponent<PlayerHpSystem>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    


    //Each time spawned from pool
    void OnEnable()
    {
        rb.velocity = transform.right * bulletSpeed;
        
        //Reset boolean to allow bullet to hit again
        hitRegistered = false;
        
        //Reset deflected times
        deflectedTimes = 0;
        
        //Disable bullet after a duration
        StartCoroutine(SetBulletInactive(gameObject));
    }
  
    void FixedUpdate()
    {
        prevVelocity = rb.velocity;
    }

    //When hitting anything
    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        Collider2D hitObject = hitInfo.collider;

        //If hit deflection wall
        if (hitObject.CompareTag("Wall - Deflect"))
        {
            deflectedTimes += 1;

            if (deflectedTimes > maxDeflectionTimes)
            {
                SpawnBulletImpactEffect();
                gameObject.SetActive(false);
            }
            return;
        }
        
        
        //If hit anything
        SpawnBulletImpactEffect();
        gameObject.SetActive(false);
        if (!hitObject.CompareTag(tagManager.tagSO.limbLegTag) && !hitObject.CompareTag(tagManager.tagSO.limbOthersTag) && !hitObject.CompareTag(tagManager.tagSO.limbHeadTag))
        {
            return;
        }
        
        
        //Enemy take damage
        if (!hitRegistered)
        {
            hitRegistered = true;
            
            EnemyHpUpdater enemyHpUpdater = hitObject.GetComponent<EnemyHpUpdater>();
            EnemyTakeDamage(enemyHpUpdater);
        }
    }

    //To detect hits on player (because HitDetectors are triggers)
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        if (!hitRegistered)
        {
            hitRegistered = true;
            PlayerTakeDamage();
        }
    }

    private void PlayerTakeDamage()
    {
        playerHp.TakeDamage(bulletDamage);
                
        SpawnBulletImpactEffect();
        gameObject.SetActive(false);
    }

    private void EnemyTakeDamage(EnemyHpUpdater enemyHpUpdater)
    {
        if (enemyHpUpdater == null) return;
            
            
        Vector2 bulletDirection = CheckBulletDirection(rb);
            
        enemyHpUpdater.TakeLimbDamage(bulletDamage, bulletDirection);
        enemyHpUpdater.TakeOverallDamage(bulletDamage, bulletDirection);
    }

    private void SpawnBulletImpactEffect()
    {
        //Instantiate(impactEffect, transform.position, transform.rotation);
        pooler.SpawnFromPool(impactEffect.name, transform.position, transform.rotation);
    }
    

    //Determine force to push collided enemy's limbs
    private Vector2 CheckBulletDirection(Rigidbody2D bulletRb)
    {
        float flingX = bulletRb.velocity.x * forceDampening;
        float flingY = bulletRb.velocity.y * forceDampening;

        return new Vector2(flingX, flingY);
    }
    
    //Become inactive after a duration after being fired. 
    private IEnumerator SetBulletInactive(GameObject shotBullet)
    {
        yield return new WaitForSeconds(1f);
        shotBullet.SetActive(false);
    }
}
