using UnityEngine;

public class BossMiniJetHp : EnemyHp
{
    [Header("Ref:")] 
    [SerializeField] private BarChangePivot healthBar;

    [Header("Edit: ")]
    [SerializeField] private int initialHp;
    
    
    [Header("Ignore if this is jet to be summoned")]
    [SerializeField] [Range(0f, 1f)] private float summonThreshold;

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
            int dmg = col.GetComponent<BulletBehaviour>().GetBulletDmg();
            TakeDamage(dmg);
            
            col.gameObject.SetActive(false);
        }
    }

    private void TakeDamage(int dmg)
    {
        if (currHp <= 0) return;
        currHp -= dmg;
        
        float percentage = (float) currHp / initialHp;
        healthBar.SetFillAmount(percentage);
        print(percentage);
            
        if (currHp == 0)
        {
            print("ded");
        }
        
        if (percentage <= summonThreshold)
        {
            if (onReachingThresholdDelegate != null)
            {
                onReachingThresholdDelegate.Invoke();
            }
        }
    }
}
