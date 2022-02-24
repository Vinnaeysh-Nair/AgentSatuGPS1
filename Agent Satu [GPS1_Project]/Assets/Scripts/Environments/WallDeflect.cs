using UnityEngine;

public class WallDeflect : MonoBehaviour
{
   
   private void OnCollisionEnter2D(Collision2D bulletCol)
    {
       if (bulletCol.collider.CompareTag("Bullet - Player") || bulletCol.collider.CompareTag("Bullet - Enemy"))
       {
          Rigidbody2D bulletRb = bulletCol.collider.GetComponent<Rigidbody2D>();
          BulletBehaviour bullet = bulletCol.collider.GetComponent<BulletBehaviour>();
          
          Vector2 inDirection = bullet.GetPrevVelocity();
          float speed = inDirection.magnitude;
          Vector2 newDirection = Vector2.Reflect(inDirection.normalized, bulletCol.contacts[0].normal); 
     
          
          bulletRb.velocity = newDirection * speed;
       }
    }
 }
