using UnityEngine;
using System;
public class MiyaHp : BossHp
{
    [Header("Ignore pivot version")]
    [SerializeField] private BarChangeSlider hpBar;

    [SerializeField] private float[] atk4HpThresholds;
    private int atk4ThresholdCounter = 0; 

    public static event Action OnReachingThreshold;
    
    void Start()
    {
        currHp = initialHp;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Bullet"))
        {
            int dmg = col.collider.GetComponent<BulletBehaviour>().GetBulletDmg();
            
            TakeDamage(dmg);
            col.gameObject.SetActive(false);
        }
    }

    private new void TakeDamage(int dmg)
    {
        if (currHp <= 0) return;
        
        currHp -= dmg;
        
        float percentage = (float) currHp / initialHp;
        hpBar.SetBarAmount(percentage);

        if (percentage <= atk4HpThresholds[atk4ThresholdCounter])
        {
            if (OnReachingThreshold != null)
            {
                OnReachingThreshold.Invoke();
            }

            if(atk4ThresholdCounter < atk4HpThresholds.Length - 1)
                atk4ThresholdCounter++;
        }
        
        if (currHp > 0) return;
        CompleteLevel();
        
        gameObject.SetActive(false);
    }
    
    public float HpPercent
    {
        get => (float) currHp / initialHp;
    }
}