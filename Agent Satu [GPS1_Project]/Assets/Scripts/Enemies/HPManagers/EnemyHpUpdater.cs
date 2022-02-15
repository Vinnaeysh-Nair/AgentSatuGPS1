using UnityEngine;

//Updates limb and overall hp, attached to all limbs (auto - by FindLimbs script).
public class EnemyHpUpdater : MonoBehaviour
{
    private SetupLimbHp setupLimb;
    private OverallHp overallHpManager;
    private Dismemberment dismemberment;
    private Ragdoll ragdoll;
    
    [SerializeField] private int limbHp;
    
    void Awake()
    {
        setupLimb = SetupLimbHp.setupLimbInstance;
    }
    
    
    void Start()
    {
        overallHpManager = GetComponentInParent<OverallHp>();
        dismemberment = transform.Find("/EnemyHpManagers/ConfigureDismemberment").GetComponent<Dismemberment>();
        ragdoll = GetComponentInParent<Ragdoll>();
        
        CopyLimbHp();
    }

    //Get limb hp based on limb name in Limb list
    private void CopyLimbHp()
    {
        string limbName = transform.name;
        SetupLimbHp.Limb foundLimb = setupLimb.limbList.Find(limb => limb.GetLimbName() == limbName);
        limbHp = foundLimb.GetInitialHp();
    }
    private void TakeLimbDamage(int dmg)
    {
        if (limbHp > 0)
        {
            limbHp -= dmg;
        }
        
        if(limbHp <= 0)
            dismemberment.Dismember(transform.gameObject);
    }

    private void TakeOverallDamage(int dmg)
    {
        int overallHp = overallHpManager.GetOverallHp();
        
        if (overallHp > 0)
        {
            overallHp -= dmg;
            overallHpManager.SetOverallHp(overallHp);
        }
        
        if(overallHp <= 0)
            ragdoll.ActivateRagdoll();
    }
    

    private void OnMouseDown()
    {
        //Disabling this script doesn't affect the colliders, therefore physics can still happen, thus need to check if this script is enabled.
        if (!enabled) return; 
        TakeLimbDamage(1);
        TakeOverallDamage(1);
    }


}
