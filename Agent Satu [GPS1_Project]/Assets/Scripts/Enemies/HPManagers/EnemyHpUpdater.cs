using UnityEngine;


//Updates limb and overall hp, attached to all limbs (auto - by FindLimbs script).
public class EnemyHpUpdater : MonoBehaviour
{
 
    //Components
    private TagManager tagManager;
    private SpawnPickups spawnPickups;
    private Enemy_Agro enemyAgro;
    
    private Dismemberment dismemberment;
    private Ragdoll ragdoll;
    
    
    private OverallHp overallHp;
    private LimbHp limbHp;
    

    //Fields
    [SerializeField] private int currLimbHp;

    void Start()
    {
        dismemberment = transform.GetComponent<Dismemberment>();
        tagManager = transform.Find("/ScriptableObjects/TagManager").GetComponent<TagManager>();
        ragdoll = transform.GetComponentInParent<Ragdoll>();

        Transform grandparent = transform.parent.parent;
        spawnPickups = grandparent.GetComponent<SpawnPickups>();
        enemyAgro = grandparent.GetComponent<Enemy_Agro>();
        
        //Get Hp components
        overallHp = grandparent.GetComponent<OverallHp>();
        limbHp = GetComponent<LimbHp>();
        
        currLimbHp = limbHp.GetInitialHp();
    }

    
    public void TakeLimbDamage(int dmg,Vector2 flingDirection)
    {
        //Limb hp decrement
        if (currLimbHp > 0)
            currLimbHp -= dmg;

        //If hp 0, dismember this limb
        if (currLimbHp <= 0)
        {
            dismemberment.Dismember(transform.gameObject,  flingDirection);
            
            //Check if head dismembered
            if(transform.CompareTag(tagManager.tagSO.limbHeadTag))
            {
                overallHp.SetIsHeadDismemberedToTrue();
            }
            
            //Check if legs dismembered
            if (transform.CompareTag(tagManager.tagSO.limbLegTag))
            {
                int legCount = overallHp.GetLegDismemberedCount();
                legCount += 1;
                overallHp.SetLegDismemberedCount(legCount);
            }
        }
    } 

    public void TakeOverallDamage(int dmg, Vector2 flingDirection)
    {
        enemyAgro.DetectedFromDamage();
        
        //Overall Hp decerement
        int currOverallHp = overallHp.GetOverallHp();
        
        if (currOverallHp > 0)
        {
            currOverallHp -= dmg;
            overallHp.SetOverallHp(currOverallHp);
        }
        
        
        //If overall hp = 0, head or both legs are dismembered, ragdoll this enemy
        if(currOverallHp <= 0 || overallHp.GetIsHeadDismembered() || overallHp.GetLegDismemberedCount() == 2)
            Die(flingDirection);
    }

    private void Die(Vector2 flingDirection)
    {
        //Disable unwanted components
        Transform mainContainer = transform.parent.parent;
        if (mainContainer.TryGetComponent(out EnemyAI_Melee enemyAI))
        {
            enemyAI.enabled = false;
        }

        if (mainContainer.TryGetComponent(out Enemy_Agro enemyAgro))
        {
            enemyAgro.enabled = false;
        }

        if (mainContainer.TryGetComponent(out Collider2D collider))
        {
            collider.enabled = false;
        }
        
        overallHp.SetOverallHp(0);
        ragdoll.ActivateRagdoll(flingDirection);
        spawnPickups.enabled = true;
    }
}
