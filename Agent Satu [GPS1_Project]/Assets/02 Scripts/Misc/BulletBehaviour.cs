using UnityEngine;
using System.Collections;

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
    [SerializeField] private GameObject bloodEffect;
    
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

    public int GetBulletDmg()
    {
        return bulletDamage;
    }
    
    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        tagManager = ObjectPooler.tagManager;
        
        GameObject playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
        playerHp = playerBody.GetComponent<PlayerHpSystem>();
        
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
        
        
        gameObject.SetActive(false);
        
        
        //If hit environment
        if (!hitObject.CompareTag(tagManager.tagSO.limbLegTag) && !hitObject.CompareTag(tagManager.tagSO.limbOthersTag) && !hitObject.CompareTag(tagManager.tagSO.limbHeadTag))
        {
            SpawnBulletImpactEffect();
            return;
        }
        
        
        //If hit limbs
        //Enemy take damage
        if (!hitRegistered)
        {
            hitRegistered = true;
            
            SpawnBloodSplatterEffect();
            
            
            Vector2 bulletDirection = CheckBulletDirection(rb);
            
            //Limb Hp damage
            LimbHp limbHp = hitObject.GetComponent<LimbHp>();
            limbHp.TakeLimbDamage(bulletDamage, bulletDirection);

            
            //Overall Hp damage
            Transform limbContainer = hitObject.transform.parent;
            if (limbContainer == null) return;                      //if not inside a limb container, meaning it's a pooled object
            
            Transform mainContainer = limbContainer.parent;
            OverallHp overallHp = mainContainer.GetComponent<OverallHp>();
            overallHp.TakeOverallDamage(bulletDamage, bulletDirection);
        }
    }

    //To detect hits on player (because HitDetectors are triggers)
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        if (!hitRegistered)
        {
            hitRegistered = true;

            SpawnBloodSplatterEffect();
            
            PlayerTakeDamage();
        }
    }

    private void PlayerTakeDamage()
    {
        playerHp.TakeDamage(bulletDamage);
        gameObject.SetActive(false);
    }

    public void SpawnBulletImpactEffect()
    {
        Transform t = transform;
        pooler.SpawnFromPool(impactEffect.name, t.position, t.rotation);
    }

    private void SpawnBloodSplatterEffect()
    {
        Vector3 currRot = transform.eulerAngles;
        Vector3 newRot = new Vector3(0f, currRot.y - 90f, 0f);

        GameObject fx = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        fx.transform.eulerAngles = newRot;
    }

    //Determine force to push collided enemy's limbs
    private Vector2 CheckBulletDirection(Rigidbody2D bulletRb)
    {
        Vector2 bulletVelocity = bulletRb.velocity;
        
        float flingX = bulletVelocity.x * forceDampening;
        float flingY = bulletVelocity.y * forceDampening;
        
        return new Vector2(flingX, flingY);
    }
    
    //Become inactive after a duration after being fired. 
    private IEnumerator SetBulletInactive(GameObject shotBullet)
    {
        yield return new WaitForSeconds(10f);
        shotBullet.SetActive(false);
    }
}
