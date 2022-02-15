using Unity.VisualScripting;
using UnityEngine;

//Attached to each enemy types (manual).
public class FindLimbs : MonoBehaviour
{ 
    private void Start ()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            Transform obj = transform.GetChild(i);
            
            //When object is limb
            if (obj.CompareTag("Limb"))
            {
                obj.AddComponent<Dismemberment>();
                obj.AddComponent<EnemyHpUpdater>();
            }
        }
    }
}
