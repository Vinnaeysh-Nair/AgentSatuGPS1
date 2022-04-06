using UnityEngine;

public class BossMiniJetHp : BossHp
{
    
    [Header("Ignore if this is jet to be summoned")]
    [SerializeField] [Range(0f, 1f)] private float summonThreshold;

    private static int killedBossesCount = 0;

    public delegate void OnReachingThreshold();
    public static event OnReachingThreshold onReachingThresholdDelegate;
    

    void Start()
    {
        currHp = initialHp;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            BulletBehaviour bullet = col.GetComponent<BulletBehaviour>();
           
            //Visual
            bullet.SpawnBulletImpactEffect();
            
            //Damage
            int dmg = bullet.GetBulletDmg();
            TakeDamage(dmg);
            
            col.gameObject.SetActive(false);
        }
    }

    private new void TakeDamage(int dmg)
    {
        if (currHp <= 0) return;
        
        
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
            CompleteLevel();
        }
           
        gameObject.SetActive(false);
    }
}
