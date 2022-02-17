using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{
    public int hpCountPlayer = 5;
    private int currHp;


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
            //Add lose condition
        }
    }
}
