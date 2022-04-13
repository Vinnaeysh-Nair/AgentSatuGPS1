using UnityEngine;

public class ArmToPlayerTracking : MonoBehaviour
{
    //reference to other scripts
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private OverallHp overallHp;
    [SerializeField] private Enemy_Agro enemyAgro;
   
    private PlayerMovement playerMovement;
    
    
    private Vector2 playerPosition;


 
    
    void Start()
    {
        playerMovement  = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<PlayerMovement>();
        
        if(overallHp != null)
            overallHp.onDeathDelegate += OverallHp_OnDeath;
    }

    //track player's Vector x and y
    void Update()
    {
        if (!enemyAgro.GetInRange()) return;

        playerPosition = playerMovement.GetPlayerPos();
        PointToPlayer();
    }

    void OverallHp_OnDeath()
    {
        overallHp.onDeathDelegate -= OverallHp_OnDeath;
        enabled = false;
    }
    
    private void PointToPlayer()
    {
        Vector2 lookDir = (Vector2) pivotTransform.position - playerPosition;
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        
        if (angleTowards > 90f || angleTowards < -90f)
        {
            //Inverted rotation
            pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards);
        }
        else
        {
            //Normal rotation
            pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards);
        }
    }
}
