using UnityEngine;
using UnityEngine.U2D.Animation;


//Attached to each enemy types (manual).
public class Ragdoll : MonoBehaviour
{
    //Components
    private TagManager tagManager;
    
    //Fields
    [SerializeField] private float ragdolledLimbGravity = 5f;
    void Start()
    {
        tagManager = tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
    }
    public void ActivateRagdoll(Vector2 flingDirection)
    {
        //GetComponent<Animator>().enabled = false;
        
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform limb = transform.GetChild(i);

            if (!limb.gameObject.activeInHierarchy) continue;
            
            if (limb.CompareTag(tagManager.tagScriptableObject.limbOthersTag) ||
                limb.CompareTag(tagManager.tagScriptableObject.limbLegTag) ||
                limb.CompareTag(tagManager.tagScriptableObject.limbHeadTag))
            {
                RagdollEffect(limb, flingDirection);
            }
        }
    }

    private void RagdollEffect(Transform limb, Vector2 flingDirection)
    {
        limb.GetComponent<SpriteSkin>().enabled = false;
        
        //Setup
        Rigidbody2D ragdolledLimb = limb.GetComponent<Rigidbody2D>();
        ragdolledLimb.isKinematic = false;
        ragdolledLimb.gravityScale = ragdolledLimbGravity;
        
        //Disable hingeJoint if exists
        if (TryGetComponent(out HingeJoint2D hingeJoint))
        {
            hingeJoint.enabled = false;
        }
        
        //Fling according to bullet direction
        ragdolledLimb.AddForce(flingDirection, ForceMode2D.Impulse);
    }
}
