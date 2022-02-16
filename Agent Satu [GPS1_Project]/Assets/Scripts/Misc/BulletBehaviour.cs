using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //Components
    private TagManager tagManager;
    private Rigidbody2D rb;
    
    //Fields
    [Header("General")]
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private GameObject impactEffect;
    
    [Header("Force on enemy when dismembering and ragdolling")]
    [SerializeField] [Range(0f, 1f)] private float flingDampening = 1f;
    private bool hitRegistered = false;
    private int bulletDamage = 1;
    
    
    void Start()
    {
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        
        
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);

        if (!hitInfo.CompareTag(tagManager.tagScriptableObject.limbLegTag) && !hitInfo.CompareTag(tagManager.tagScriptableObject.limbOthersTag))
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
        }
    }
    
    
    private Vector2 CheckBulletDirection(Rigidbody2D bulletRb)
    {
        float flingX = bulletRb.velocity.x * flingDampening;
        float flingY = bulletRb.velocity.y * flingDampening;

        return new Vector2(flingX, flingY);
    }
}
