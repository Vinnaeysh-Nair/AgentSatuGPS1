using UnityEngine;
using UnityEngine.U2D.Animation;


//Attached to each enemy types (manual).
public class Ragdoll : MonoBehaviour
{ 
    public void ActivateRagdoll()
    {
        //GetComponent<Animator>().enabled = false;
        
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform limb = transform.GetChild(i);
            
            if (limb.gameObject.activeInHierarchy && limb.CompareTag("Limb"))
            {
                RagdollEffect(limb);
            }
        }
    }

    private void RagdollEffect(Transform limb)
    {
        limb.GetComponent<SpriteSkin>().enabled = false;
        
        Rigidbody2D ragdolledLimb = limb.GetComponent<Rigidbody2D>();
        ragdolledLimb.isKinematic = false;
        
        if (TryGetComponent(out HingeJoint2D hingeJoint))
        {
            hingeJoint.enabled = false;
        }
        

        //Add more rigidbody physics for impact
        //ragdolledLimb.AddForce(new Vector2(1f, 1f), ForceMode2D.Impulse);
    }
}
