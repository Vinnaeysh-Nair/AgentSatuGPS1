using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{
    public int hpCountPlayer = 5;
    private int currHp;


    void Start()
    {
        currHp = hpCountPlayer;
        TakeDamage(3);
    }
    
    public void TakeDamage(int dmg)
    {
        currHp -= dmg;
        
        //Might be used to scale healthbar
        float percentage =(float) currHp / hpCountPlayer;
        
        if (currHp <= 0)
        {
            //Debug.Log("ded");
            //Add lose condition
        }
    }

    public void ReplenishHealth(int amount)
    {
        if (currHp == hpCountPlayer) return;
        currHp += amount;
    }
}
