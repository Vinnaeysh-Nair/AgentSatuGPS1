using UnityEngine;

//Updates limb and overall hp, attached to all limbs (auto - by FindLimbs script).
public class EnemyHpUpdater : MonoBehaviour
{
    private SetupLimbHp setupLimb;
    private OverallHp overallHpManager;
    private Dismemberment dismemberment;
    private Ragdoll ragdoll;
    
    [SerializeField] private int limbHp;
    private TagManager tagManager;
    private SpawnPickups spawnPickups;
    
    void Awake()
    {
        setupLimb = SetupLimbHp.setupLimbInstance;
    }
    
    
    void Start()
    {
        overallHpManager = GetComponentInParent<OverallHp>();
        dismemberment = transform.Find("/EnemyHpManagers/ConfigureDismemberment").GetComponent<Dismemberment>();
        ragdoll = GetComponentInParent<Ragdoll>();
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        spawnPickups = GetComponentInParent<SpawnPickups>();
        
        CopyLimbHp();
    }

    //Get limb hp based on limb name in Limb list
    private void CopyLimbHp()
    {
        string limbName = transform.name;
        SetupLimbHp.Limb foundLimb = setupLimb.limbList.Find(limb => limb.GetLimbName() == limbName);
        limbHp = foundLimb.GetInitialHp();
    }
    
    public void TakeLimbDamage(int dmg,Vector2 flingDirection)
    {
        //Limb hp decerement
        if (limbHp > 0)
            limbHp -= dmg;

        //If hp 0, dismember this limb
        if (limbHp <= 0)
        {
            dismemberment.Dismember(transform.gameObject,  flingDirection);
            
            //Check if head dismembered
            if(transform.CompareTag(tagManager.tagSO.limbHeadTag))
            {
                overallHpManager.SetIsHeadDismemberedToTrue();
            }
        }
           
        
        //Check if legs dismembered
        if (transform.CompareTag(tagManager.tagSO.limbLegTag))
        {
            int legCount = overallHpManager.GetLegDismemberedCount();
            legCount += 1;
            overallHpManager.SetLegDismemberedCount(legCount);
        }
        
        //If Head dismemebered, activate ragdoll  
        if (overallHpManager.GetIsHeadDismembered())
            Die(flingDirection);
        

        //If both legs dismembered, activate ragdoll
        if (overallHpManager.GetLegDismemberedCount() == 2)
            Die(flingDirection);
            
    } 

    public void TakeOverallDamage(int dmg, Vector2 flingDirection)
    {
        //Overall Hp decerement
        int overallHp = overallHpManager.GetOverallHp();
        
        if (overallHp > 0)
        {
            overallHp -= dmg;
            overallHpManager.SetOverallHp(overallHp);
        }
        
        
        //If overall hp = 0, ragdoll this enemy
        if(overallHp <= 0)
            Die(flingDirection);
    }

    private void Die(Vector2 flingDirection)
    {
        ragdoll.ActivateRagdoll(flingDirection);
        spawnPickups.enabled = true;
    }
    

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (!collision.transform.CompareTag("Bullet")) return;
    //     
    //     //Disabling this script doesn't affect the colliders, therefore physics can still happen, thus need to check if this script is enabled.
    //     //if (!enabled) return; 
    //     TakeLimbDamage(1);
    //     TakeOverallDamage(1);
    // }
}
