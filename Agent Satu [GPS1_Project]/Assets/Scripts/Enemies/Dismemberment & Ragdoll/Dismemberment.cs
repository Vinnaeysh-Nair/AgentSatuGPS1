using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

//Attached to all limbs (auto - by FindLimbs script).
public class Dismemberment : MonoBehaviour
{
    //Components
    private ObjectPooler objectPooler;

    //Fields
    [SerializeField] private float dismemberedLimbGravity = 3f;
    private Vector3 objActualScale;
    void Awake()
    {
        objectPooler = ObjectPooler.objPoolerInstance;
    }

    void Start()
    {
        //Save scale of the original object (the parent to the limbs)
        objActualScale = transform.parent.parent.localScale;
    }

    
    public void Dismember(GameObject limb, Vector2 flingDirection)
    {
       
        
        //Spawning new limb
        GameObject detachedLimb = objectPooler.SpawnFromPool(limb.name, limb.transform.position, Quaternion.identity);
        if (detachedLimb == null) return;
        
        //Set up detached limb
             
        detachedLimb.transform.localScale = objActualScale;
        
        //Disable unwanted components
        if (detachedLimb.TryGetComponent(out SpriteSkin spriteSkin))
        {
            spriteSkin.enabled = false;
        }
        if (detachedLimb.TryGetComponent(out HingeJoint2D joint))
        {
            joint.enabled = false;
        }
        
        
        //Setup
        Rigidbody2D limbRb = detachedLimb.GetComponent<Rigidbody2D>();
        limbRb.isKinematic = false;
        limbRb.drag = .5f;
        limbRb.gravityScale = dismemberedLimbGravity;
       
        //Apply physics
        //Fling according to bullet direction
        limbRb.AddForce(flingDirection, ForceMode2D.Impulse);
        
        //Disable original limb
        limb.SetActive(false);
    }
}
