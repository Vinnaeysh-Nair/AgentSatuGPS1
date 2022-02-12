using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float bulletSpeed = 40f;
    public float bulletDamage = 50f;
    public GameObject impactEffect;
    
    void Start()
    {
        rb.velocity = transform.right * bulletSpeed;
        DestroySelf();
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        // EnemyBehaviour enemy = hitInfo.GetComponent<EnemyBehaviour>();
        //
        // if (enemy != null)
        // {
        //     enemy.TakeDamage(bulletDamage);
        // }

        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    //If doesn't hit anything, destroy self after 5 secs
    private void DestroySelf()
    {
        Destroy(gameObject, 5f);
    }
}
