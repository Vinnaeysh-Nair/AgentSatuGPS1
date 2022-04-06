using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHpSystem : MonoBehaviour
{
    //Components
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private BarChangeSlider healthBar;
    
    //Fields
    [SerializeField] private int hpCountPlayer = 5;
    private int currHp;

    public static int deathLevelIndex;
    
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

        deathLevelIndex = SceneManager.GetActiveScene().buildIndex;
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
