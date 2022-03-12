using UnityEngine;

public class EnemyAI_Ranged : MonoBehaviour
{
    //Components
    [SerializeField] private EnemyWeapon_Guns enemyGun;
    private Enemy_Agro enemyAgro;
    
    //Fields
    [SerializeField] private bool isBurst;
    private bool shooting = false;

    void Start()
    {
        enemyAgro = GetComponent<Enemy_Agro>();
    }
    private void FixedUpdate()
    {
        if (enemyAgro.GetDetected())
        {
            if (isBurst)
            {
                enemyGun.StartBurstShooting();
            }
            else
            {
                enemyGun.StartShooting();
            }
        }
    }
}
