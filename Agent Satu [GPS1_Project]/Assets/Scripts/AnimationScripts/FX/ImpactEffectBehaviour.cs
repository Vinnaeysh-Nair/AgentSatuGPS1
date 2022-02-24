using UnityEngine;
using System.Collections;


//Attached to bullet FX effect
public class ImpactEffectBehaviour : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        anim.SetTrigger("ImpactEffectTrigger");
        StartCoroutine(DisableSelf());
    }

    private IEnumerator DisableSelf()
    {
        yield return new WaitForSeconds(.5f);
        
        gameObject.SetActive(false);
    }
}
