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
    
    //Applied to original (not dismembered) limbs
    public void ActivateRagdoll(Vector2 flingDirection)
    {
        //GetComponent<Animator>().enabled = false;
        
        //If root container object is flipped, when Ragdoll activates, flip the limbs container as well (fixes the rotation bug where dismembered parts rotations crazily)
        Vector3 parentRotation = transform.parent.eulerAngles;
        transform.eulerAngles = parentRotation;
        
        
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform limb = transform.GetChild(i);

            if (!limb.gameObject.activeInHierarchy) continue;
            
            if (limb.CompareTag(tagManager.tagSO.limbOthersTag) ||
                limb.CompareTag(tagManager.tagSO.limbLegTag) ||
                limb.CompareTag(tagManager.tagSO.limbHeadTag))
            {
                RagdollEffect(limb, flingDirection);
            }
        }
    }

    private void RagdollEffect(Transform limbToRagdoll, Vector2 flingDirection)
    {
        //Disable unwanted components
        if (limbToRagdoll.TryGetComponent(out SpriteSkin spriteSkin))
        {
            spriteSkin.enabled = false;
        }
        

        
        //Setup rigidbody
        Rigidbody2D ragdolledLimbRb = limbToRagdoll.GetComponent<Rigidbody2D>();
        ragdolledLimbRb.isKinematic = false;
        ragdolledLimbRb.gravityScale = ragdolledLimbGravity;
        
        
        //Fling according to bullet direction
        ragdolledLimbRb.AddForce(flingDirection, ForceMode2D.Impulse);
    }
}
