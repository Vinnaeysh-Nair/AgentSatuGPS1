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
    }
    
    public void TakeDamage(int dmg)
    {
        currHp -= dmg;
        
        //Scale healthbar
        healthBar.SetBarAmount(ConvertToPercentage());

        if (currHp > 0) return;
        sceneLoader.LoadLoseScene();
    }

    public void ReplenishHealth(int amount)
    {
        if (currHp == hpCountPlayer) return;
        currHp += amount;
        
        //Scale healthbar
        healthBar.SetBarAmount(ConvertToPercentage());
    }

    private float ConvertToPercentage()
    {
        float percentage = (float) currHp / hpCountPlayer;
        return percentage;
    }
}
