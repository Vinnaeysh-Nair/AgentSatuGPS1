using UnityEngine;

public class BossMiniJetHp : EnemyHp
{
    [Header("Ref:")] 
    [SerializeField] private BarChangePivot healthBar;

    [Header("Edit: ")]
    [SerializeField] private int initialHp;
    
    
    [Header("Ignore if this is jet to be summoned")]
    [SerializeField] [Range(0f, 1f)] private float summonThreshold;

    private static int killedBossesCount = 0;

    public delegate void OnReachingThreshold();
    public static event OnReachingThreshold onReachingThresholdDelegate;

    public delegate void OnLevelComplete();
    public static event OnLevelComplete onLevelCompleteDelegate;


    void Start()
    {
        currHp = initialHp;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            int dmg = col.GetComponent<BulletBehaviour>().GetBulletDmg();
            TakeDamage(dmg);
            
            col.gameObject.SetActive(false);
        }
    }

    private void TakeDamage(int dmg)
    {
        currHp -= dmg;
        
        float percentage = (float) currHp / initialHp;
        healthBar.SetFillAmount(percentage);

        
        if (percentage <= summonThreshold)
        {
            if (onReachingThresholdDelegate != null)
            {
                onReachingThresholdDelegate.Invoke();
            }
        }

        if (currHp > 0) return;
        
        //Death logic
        killedBossesCount++;
       
        //print("ded" + killedBossesCount);
        if (killedBossesCount == 3)
        {
            //reset static value
            killedBossesCount = 0;
            if (onLevelCompleteDelegate != null)
            {
                onLevelCompleteDelegate.Invoke();
            }
        }
           
        gameObject.SetActive(false);
    }
}
