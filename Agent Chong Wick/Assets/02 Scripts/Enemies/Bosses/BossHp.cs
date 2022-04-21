using UnityEngine;

public class BossHp : EnemyHp
{
    [Header("Ref:")] 
    [SerializeField] protected BarChangePivot healthBar;

    [Header("Edit: ")]
    [SerializeField] protected int initialHp;

    public delegate void OnLevelComplete();
    public static event OnLevelComplete onLevelCompleteDelegate;
    
    protected void TakeDamage(int dmg)
    {
        if (currHp <= 0) return;
        
        currHp -= dmg;
        
        float percentage = (float) currHp / initialHp;
        healthBar.SetFillAmount(percentage);
        

        if (currHp > 0) return;
        //Death logic
        CompleteLevel();
        
           
        gameObject.SetActive(false);
    }
    
    protected void CompleteLevel()
    {
        if (onLevelCompleteDelegate != null)
        {
            onLevelCompleteDelegate.Invoke();
        }
    }
}
