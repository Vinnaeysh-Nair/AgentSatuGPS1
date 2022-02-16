using UnityEngine;
using UnityEngine.U2D.Animation;


//Attached to each enemy types (manual).
public class Ragdoll : MonoBehaviour
{
    private TagManager tagManager;
    void Start()
    {
        tagManager = tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
    }
    public void ActivateRagdoll(Vector2 bulletDirection)
    {
        //GetComponent<Animator>().enabled = false;
        
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform limb = transform.GetChild(i);
            
            if (limb.gameObject.activeInHierarchy)
            {
                if (limb.CompareTag(tagManager.tagScriptableObject.limbOthersTag) ||
                    limb.CompareTag(tagManager.tagScriptableObject.limbLegTag))
                {
                    RagdollEffect(limb);
                    limb.GetComponent<Rigidbody2D>().AddForce(bulletDirection, ForceMode2D.Impulse);
                }
                    
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
