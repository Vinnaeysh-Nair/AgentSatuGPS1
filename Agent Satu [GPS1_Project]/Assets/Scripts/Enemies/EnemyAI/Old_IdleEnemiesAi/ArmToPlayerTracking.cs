using UnityEngine;

public class ArmToPlayerTracking : MonoBehaviour
{
    //reference to other scripts
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private Enemy_Flipped enemyFlipped;
    [SerializeField] private Enemy_Agro enemyAgro;
    private PlayerMovement playerMovement;
    private OverallHp overallHp;

    
    [SerializeField] private float followAngleOffset;
    //[SerializeField] private bool isFacingRight = false;
    private Vector2 playerPosition;


 
    
    void Start()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();

        overallHp = transform.parent.parent.GetComponent<OverallHp>();
        overallHp.OnDeath += OverallHp_OnDeath;
    }

    //track player's Vector x and y
    void Update()
    {
        if (!enemyAgro.GetInRange()) return;

        playerPosition = playerMovement.GetPlayerPos();
        PointToPlayer();
    }

    void OverallHp_OnDeath(object sender, System.EventArgs e)
    {
        overallHp.OnDeath -= OverallHp_OnDeath;
        enabled = false;
    }
    
    private void PointToPlayer()
    {
        Vector2 lookDir = (Vector2)pivotTransform.position - playerPosition;
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        
        if (angleTowards > 90f || angleTowards < -90f)
        {
            //Inverted rotation
            pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards - followAngleOffset);
        }
        else
        {
            //Normal rotation
            pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards - followAngleOffset);
        }
    }
    
    //jake ver
    //If pointing to left side, invert the pivot's x rotation and angleTowards
    //to accomodate sprite rotations
    // if (isFacingRight)
    // {
    //     if (angleTowards > 90f || angleTowards < -90f)
    //     {
    //         //Inverted rotation
    //         pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards - followAngleOffset);
    //     }
    // }
    // else
    // {
    //     if (!(angleTowards > 90f || angleTowards < -90f))
    //     {
    //         //Normal rotation
    //         pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards - followAngleOffset);
    //     }
    // }
}
