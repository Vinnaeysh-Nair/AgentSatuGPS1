using UnityEngine;
using UnityEngine.U2D.Animation;

//Attached to all limbs (auto - by FindLimbs script).
public class Dismemberment : MonoBehaviour
{
    //Components
    private ObjectPooler objectPooler;
    
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
        //limbRb.AddForce(new Vector2(1 * 0f, 1 * 0f), ForceMode2D.Impulse);
    }
}
