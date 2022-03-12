using Unity.VisualScripting;
using UnityEngine;

//Attached to each enemy types (manual).
public class FindLimbs : MonoBehaviour
{ 
    private TagManager tagManager;
    private void Start ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform limb = transform.GetChild(i);
            limb.AddComponent<Dismemberment>();
        }
    }
}
