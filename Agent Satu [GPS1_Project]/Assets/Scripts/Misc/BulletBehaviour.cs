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
    [SerializeField] private int bulletDamage = 1;
    private bool hitRegistered = false;             
    


    
    
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
        StartCoroutine(SetBulletInactive(gameObject));
        
        //Reset boolean to allow bullet to hit again
        hitRegistered = false;
        
    }

    //When hitting anything
    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        //If hit player
        if (hitInfo.CompareTag("Player"))
        {
            if (transform.CompareTag("Bullet - Player")) return;
            
            //To make sure bullet only hit one time
            if (!hitRegistered)
            {
                hitRegistered = true;
                
                playerHp.TakeDamage(bulletDamage);
                gameObject.SetActive(false);
                
                Instantiate(impactEffect, transform.position, transform.rotation);
                return;
            }
        }


        
        //If hit enemies
        //If not hitting any limbs, return
        if (!hitInfo.CompareTag(tagManager.tagSO.limbLegTag) && !hitInfo.CompareTag(tagManager.tagSO.limbOthersTag) && !hitInfo.CompareTag(tagManager.tagSO.limbHeadTag))
        {
            gameObject.SetActive(false);
            Instantiate(impactEffect, transform.position, transform.rotation);
            return;
        }
        
        //If is enemy's bullet, no friendly fire
        if (transform.CompareTag("Bullet - Enemy")) return;


        //Take Damage
        if (!hitRegistered)
        {
            hitRegistered = true;
            
            EnemyHpUpdater enemyHpUpdater = hitInfo.GetComponent<EnemyHpUpdater>();
            if (enemyHpUpdater == null) return;
            
            
            Vector2 bulletDirection = CheckBulletDirection(rb);
            
            enemyHpUpdater.TakeLimbDamage(bulletDamage, bulletDirection);
            enemyHpUpdater.TakeOverallDamage(bulletDamage, bulletDirection);
            
            gameObject.SetActive(false);
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
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
