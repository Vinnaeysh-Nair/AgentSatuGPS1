using Unity.VisualScripting;
using UnityEngine;

//Attached to each enemy types (manual).
public class FindLimbs : MonoBehaviour
{ 
    private TagManager tagManager;
    private void Start ()
    {
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        
        
        
        for (int i = 0; i < transform.childCount; i++) 
        {
            Transform obj = transform.GetChild(i);
            
            //When object is limb
            if (obj.CompareTag(tagManager.tagScriptableObject.limbOthersTag) || obj.CompareTag(tagManager.tagScriptableObject.limbLegTag))
            {
                obj.AddComponent<Dismemberment>();
                obj.AddComponent<EnemyHpUpdater>();
            }
            
        }
    }
}
