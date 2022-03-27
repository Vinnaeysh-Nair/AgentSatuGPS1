using UnityEngine;

public class WallDeflect : MonoBehaviour
{
   
   private void OnCollisionEnter2D(Collision2D bulletCol)
    {
       if (bulletCol.collider.CompareTag("Bullet"))
       {
           Rigidbody2D bulletRb = bulletCol.collider.GetComponent<Rigidbody2D>();
           BulletBehaviour bullet = bulletCol.collider.GetComponent<BulletBehaviour>();

           Vector2 bulletVelocity = bullet.GetPrevVelocity();
           ContactPoint2D contactPoint = bulletCol.contacts[0];
           
           Deflect(bulletRb, bulletVelocity, contactPoint);
       }
    }

   private void Deflect(Rigidbody2D bulletRb, Vector2 bulletVelocity, ContactPoint2D contactPoint)
   {
       Vector2 inDirection = bulletVelocity;
       float speed = inDirection.magnitude;
       Vector2 newDirection = Vector2.Reflect(inDirection.normalized, contactPoint.normal); 
       
       //Apply velocity
       bulletRb.velocity = newDirection * speed;
       
       //Change rotation
       bulletRb.transform.right = newDirection;
   }
 }
