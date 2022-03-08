using System.Collections;
using UnityEngine;

public class Enemy_Agro : MonoBehaviour
{
    //Components
    private Enemy_Flipped enemflip;
    private Transform playerBody;
    private OverallHp overallHp;


    //Fields
    [Header("Aggro Settings")] 
    [SerializeField] private float AgroRange;

    [Header("Detection speeds")] 
    [SerializeField] private float detectionSpeed;
    [SerializeField] private float loseDetectionSpeed;

    private bool detected = false;
    private bool detectionStatusChanging = false;


    private Vector2 playerPos;
    private Vector2 enemyPos;

    public bool GetDetected()
    {
        return detected;
    }

    public Vector2 GetPlayerPos()
    {
        return playerPos;
    }



    void Start()
    {
        playerBody = transform.Find("/Player/PlayerBody").GetComponent<Transform>();
        enemflip = GetComponent<Enemy_Flipped>();
        overallHp = GetComponent<OverallHp>();
    }

    void FixedUpdate()
    {
        if (overallHp.GetOverallHp() == 0) return;
        
        playerPos = playerBody.position;
        enemyPos = transform.position;
        UpdateDetection();

        
        if (detected)
        {
            enemflip.LookAtPlayer();
        }
    }


    private void UpdateDetection()
    {
        float distToPlayer = Vector2.Distance(enemyPos, playerPos);
        

        if (detectionStatusChanging) return;
        if (distToPlayer < AgroRange)
        {
            StartCoroutine(SetDetectedStatus(true, detectionSpeed));

        }
        else
        {
            StartCoroutine(SetDetectedStatus(false, loseDetectionSpeed));

        }
    }


    private IEnumerator SetDetectedStatus(bool status, float speed)
    {
        if (detected == status) yield break;
        
        
        detectionStatusChanging = true;

        yield return new WaitForSeconds(speed);
        detected = status;
        detectionStatusChanging = false;
    }

    public void DetectedFromDamage()
    {
        StartCoroutine(SetDetectedStatus(true, detectionSpeed));
    }
}
