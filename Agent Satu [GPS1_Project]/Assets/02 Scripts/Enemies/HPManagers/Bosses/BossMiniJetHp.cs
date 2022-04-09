using UnityEngine;

public class BossMiniJetHp : BossHp
{
    [Header("Ignore if this is jet to be summoned")]
    [SerializeField] [Range(0f, 1f)] private float summonThreshold;

    [Header("Fx")] 
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject explosionVFX;
    
    private static int killedBossesCount = 0;

    public delegate void OnReachingThreshold();
    public static event OnReachingThreshold onReachingThresholdDelegate;
    

    void Start()
    {
        //reset static value
        killedBossesCount = 0;
        
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
        SoundManager.Instance.PlayEffect(explosionSound);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        killedBossesCount++;
       
        //print("ded" + killedBossesCount);
        if (killedBossesCount == 3)
        {
            CompleteLevel();
        }
           
        gameObject.SetActive(false);
    }
}
