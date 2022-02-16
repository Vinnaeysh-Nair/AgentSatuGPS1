using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

//Attached to all limbs (auto - by FindLimbs script).
public class Dismemberment : MonoBehaviour
{
    //Components
    private ObjectPooler objectPooler;

    [SerializeField] private float flingX = 50f;
    [SerializeField] private float flingY = 50f;
    
    void Awake()
    {
        objectPooler = ObjectPooler.objPoolerInstance;
    }

    
    public void Dismember(GameObject limb)
    {
        //Disable original limb
        limb.SetActive(false);
        
        
        //Spawning new limb
        GameObject detachedLimb = objectPooler.SpawnFromPool(limb.name, limb.transform.position, Quaternion.identity);
        if (detachedLimb == null) return;
        
        //Set up detached limb
        Vector3 objActualScale = limb.transform.parent.localScale;       //Save scale of the original object (the parent to the limbs)
        detachedLimb.transform.localScale = objActualScale;
        
        //Disable unwanted components
        detachedLimb.GetComponent<SpriteSkin>().enabled = false;
        if (detachedLimb.TryGetComponent(out HingeJoint2D hingeJoint))
        {
            hingeJoint.enabled = false;
        }

        
        //Apply physics
        Rigidbody2D limbRb = detachedLimb.GetComponent<Rigidbody2D>();
        limbRb.isKinematic = false;
        //Apply more physics for impact
        limbRb.AddForce(new Vector2(flingX, flingY), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Bullet")) return;
        Rigidbody2D bulletRb = collision.rigidbody;
        
        
        //Debug.Log(bulletRb.velocity.x + ", " + bulletRb.velocity.y);
        if (bulletRb.velocity.x > 0)
        {
            flingX = 10f;
        }
        else
        {
            flingX = -10f;
        }

        if (bulletRb.velocity.y > 0)
        {
            flingY = 10f;
        }
        else
        {
            flingY = -10f;
        }
        
    }
}
