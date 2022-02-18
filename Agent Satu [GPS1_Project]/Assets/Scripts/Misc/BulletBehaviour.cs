using System;
using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    //Components
    private TagManager tagManager;
    private Rigidbody2D rb;
    private PlayerHpSystem playerHp;

    //Fields
    [Header("General")]
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private GameObject impactEffect;
    
    [Header("Force on enemy when dismembering and ragdolling")]
    [SerializeField] [Range(0f, 1f)] private float forceDampening = 1f;
    private bool hitRegistered = false;             
    private int bulletDamage = 1;


    
    
    void Awake()
    {
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        playerHp = transform.Find("/Player/PlayerBody").GetComponent<PlayerHpSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    //Each time spawned from pool
    void OnEnable()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    //Hit enemy
    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        
        //If hit player
        if (hitInfo.CompareTag("Player"))
        {
            //To make sure bullet only hit one time
            if (!hitRegistered)
            {
                hitRegistered = true;
                
                playerHp.TakeDamage(bulletDamage);
                
                gameObject.SetActive(false);

                hitRegistered = false;
                return;
            }
        }
        
        //If hit enemies
        //If not hitting any limbs, return
        if (!hitInfo.CompareTag(tagManager.tagSO.limbLegTag) && !hitInfo.CompareTag(tagManager.tagSO.limbOthersTag) && !hitInfo.CompareTag(tagManager.tagSO.limbHeadTag))
        {
            gameObject.SetActive(false);
            return;
        }

        
        if (!hitRegistered)
        {
            hitRegistered = true;
            
            EnemyHpUpdater enemyHpUpdater = hitInfo.GetComponent<EnemyHpUpdater>();
            if (enemyHpUpdater == null) return;
            
            
            Vector2 bulletDirection = CheckBulletDirection(rb);
            enemyHpUpdater.TakeLimbDamage(bulletDamage, bulletDirection);
            enemyHpUpdater.TakeOverallDamage(bulletDamage, bulletDirection);
            
            gameObject.SetActive(false);
            hitRegistered = false;
        }
    }


    //Determine force to push collided enemy's limbs
    private Vector2 CheckBulletDirection(Rigidbody2D bulletRb)
    {
        float flingX = bulletRb.velocity.x * forceDampening;
        float flingY = bulletRb.velocity.y * forceDampening;

        return new Vector2(flingX, flingY);
    }
}
