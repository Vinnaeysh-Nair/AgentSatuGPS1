using Unity.VisualScripting;
using UnityEngine;


public class PlayerHpSystem : MonoBehaviour
{
    //Components
    public SceneLoader sceneLoader;
    
    //Fields
    public int hpCountPlayer = 5;
    private int currHp;
    private bool isTakingDamage = false;
    
    
    void Start()
    {
        currHp = hpCountPlayer;
    }
    
    public void TakeDamage(int dmg)
    {
        currHp -= dmg;
        
        //Might be used to scale healthbar
        float percentage =(float) currHp / hpCountPlayer;
        
        if (currHp <= 0)
        {
            //Debug.Log("ded");
            //Add lose scene
            sceneLoader.LoadLoseScene();
        }
    }

    public void ReplenishHealth(int amount)
    {
        if (currHp == hpCountPlayer) return;
        currHp += amount;
    }
}
