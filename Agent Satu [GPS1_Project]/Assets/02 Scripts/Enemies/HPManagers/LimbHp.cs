using UnityEngine;


public class LimbHp : EnemyHp
{
    //Components
    private Dismemberment dismemberment;
    private OverallHp overallHp;
    private TagManager tagManager;
    
    //Fields
    [Header("Edit: ")]
    [SerializeField] private int initialLimbHp;

 
    void Start()
    {
        dismemberment = GetComponent<Dismemberment>();
        overallHp = transform.parent.parent.GetComponent<OverallHp>();
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        
        base.currHp = initialLimbHp;
    }

    public void TakeLimbDamage(int dmg, Vector2 flingDirection)
    {
        //Stop decrementing starting at 0
        if (currHp <= 0) return;
        currHp -= dmg;

        if (currHp > 0) return;
        dismemberment.Dismember(gameObject, flingDirection);
        

        //Other conditions for dying
        if (transform.CompareTag(tagManager.tagSO.limbHeadTag))
        {
            overallHp.SetIsHeadDismemberedToTrue();
        }

        if (transform.CompareTag(tagManager.tagSO.limbLegTag))
        {
            int legCount = overallHp.GetLegDismemberedCount();
            legCount += 1;
            overallHp.SetLegDismemberedCount(legCount);
        }

        //Die
        if (overallHp.GetIsHeadDismembered() || overallHp.GetLegDismemberedCount() == 2)
        {
            overallHp.Die(flingDirection);
        }
    }
}
