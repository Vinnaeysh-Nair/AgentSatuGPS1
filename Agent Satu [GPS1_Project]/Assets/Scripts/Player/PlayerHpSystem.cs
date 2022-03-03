using UnityEngine;


public class PlayerHpSystem : MonoBehaviour
{
    //Components
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private BarChange healthBar;
    
    //Fields
    [SerializeField] private int hpCountPlayer = 5;
    private int currHp;
  
    
    void Start()
    {
        currHp = hpCountPlayer;
        healthBar.SetBarAmount(1f);
    }
    
    public void TakeDamage(int dmg)
    {
        currHp -= dmg;
        
        //Scale healthbar
        float percentage =(float) currHp / hpCountPlayer;
        healthBar.SetBarAmount(percentage);

        if (currHp > 0) return;
        sceneLoader.LoadLoseScene();
    }

    public void ReplenishHealth(int amount)
    {
        if (currHp == hpCountPlayer) return;
        currHp += amount;
    }
}
