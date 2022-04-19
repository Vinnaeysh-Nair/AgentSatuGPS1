using System;
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
    
    private bool detected;
    private bool detectionStatusChanging = false;
    private bool inRange = false;

    private Vector2 playerPos;
    private Vector2 enemyPos;


    public bool GetDetected()
    {
        return detected;
    }

    public bool GetInRange()
    {
        return inRange;
    }

    public Vector2 GetPlayerPos()
    {
        return playerPos;
    }

    
    void Start()
    {
        playerBody = PlayerMain.Instance.transform;
        enemflip = GetComponent<Enemy_Flipped>();
        
        overallHp = GetComponent<OverallHp>();

        if (overallHp != null)
        {
            overallHp.onDamagedDelegate += OverallHp_OnDamaged;
            overallHp.onDeathDelegate += OverallHp_OnDeath;
        }
    }

    
    void FixedUpdate()
    {
        playerPos = playerBody.position;
        enemyPos = transform.position;
        UpdateDetection();
  
        
        if (detected)
        {
            enemflip.LookAtPlayer();
        }
    }
    
    private void OverallHp_OnDamaged()
    {
        DetectedFromDamage();
    }

    private void OverallHp_OnDeath()
    {
        overallHp.onDamagedDelegate -= OverallHp_OnDamaged;
        overallHp.onDeathDelegate -= OverallHp_OnDeath;
    }
    
    private void UpdateDetection()
    {
        float distToPlayer = Vector2.Distance(enemyPos, playerPos);
        
        if (distToPlayer < AgroRange)
        {
            inRange = true;
            
            if (detectionStatusChanging) return;
            StartCoroutine(SetDetectedStatus(true, detectionSpeed));
            
        }
        else
        {
            inRange = false;
            
            if (detectionStatusChanging) return;
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
