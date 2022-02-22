using UnityEngine;


public class LimbHp : EnemyHp
{
    [SerializeField] private int initialLimbHp;
    
    void Start()
    {
        hp = initialLimbHp;
    }
}
