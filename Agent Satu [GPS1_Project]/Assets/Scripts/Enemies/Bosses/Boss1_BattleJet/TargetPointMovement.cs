using System.Collections;
using UnityEngine;
using System;

public class TargetPointMovement : MonoBehaviour
{
    //Components
    private SynchGunMovements synch;
    private PlayerMovement playerMovement;
    
    [SerializeField] private BattleJetGun gun;
    [SerializeField] private Transform idlePoint;
   
    private Transform startPoint;
    private Transform endPoint;
    private Transform playerPos;
    
    //Fields
    [SerializeField] private bool inPosition;       //remove later
    
    [Header("General")]
    [SerializeField] private int attackCounter = 1;
    [SerializeField] private float timeToStartBattle = 3f;
    [SerializeField] private float idleBufferTime = 5f;
    
    private int maxAttackNumbers = 3;

    private float stopTime = 0f;

    
    [Header("Move Speeds")]
    [SerializeField] private float goToPositionMoveSpeed = 1f;
    [SerializeField] private float inPatternMoveSpeed = 1f;
    private int nextPattern = 0;
    
    private bool idling = false;
    private bool isChangingAttack = false;
    
    
    [Header("[Attack 1]")]
    [SerializeField] private int atk1ShotsPerBurst;
    [SerializeField] private float atk1FireRate;
    [SerializeField] private float atk1TimeUntilNextBurst;
    private float nextFireTime = 0f;
    private bool atk1CanShoot = false;

    [SerializeField] private Vector3 playerFollowOffset;
    [SerializeField] private float playerFollowSpeed;
    
    [SerializeField] private float timeTillAttack2;


    [Space]
    [Header("[Attack 2]")]
    //[SerializeField] private int atk2ShotsPerBurst;
    [SerializeField] private float atk2FireRate;

    [SerializeField] private float timeToNextPattern = 3f;
    [SerializeField] private Patterns[] patterns;
    
    
    [Space]
    [Header("[Attack 3]")]
    [SerializeField] private float timeTillAttack1;

    
    public event EventHandler OnReachingIdle;
    
    
    [Serializable]
    struct Patterns
    {
        public Transform[] patternPoints;
        public int atk2ShotsPerBurst;
    }


    public bool GetInPosition()
    {
        return inPosition;
    }


    private void BattleJetGun_OnFiredAllShots(object sender, System.EventArgs e)
    {
        atk1CanShoot = false;
    }

    private void OnDestroy()
    {
        gun.OnFiredAllShots -= BattleJetGun_OnFiredAllShots;
    }

    void Start()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();
        synch = transform.parent.GetComponent<SynchGunMovements>();
        
        playerPos = playerMovement.transform;
        gun.OnFiredAllShots += BattleJetGun_OnFiredAllShots;
        
        
        //Wait time before starting battle
        startPoint = idlePoint;
        idling = true;
        StartCoroutine(StartBattle());
    }
    
    void Update()
    {
        if (idling)
        {
            Idle();
            return;
        }
        

        switch (attackCounter)
        {
            case 1: Attack1();
                break;
            case 2: Attack2();
                break;
            case 3: Attack3();
                break;
        }
    }

 

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(timeToStartBattle);    //put editable timer afterwards
        
        idling = false;
        inPosition = false;

      
        //Attack 2 initial pos
        startPoint = patterns[0].patternPoints[0];
        endPoint = patterns[0].patternPoints[1];
    }
    
    
    //Track playerPos and shoot with delay
    private void Attack1()
    {
        if (stopTime == 0f)
        {
            stopTime = Time.time + timeTillAttack2;
        }
        
        
        //Track player constantly
        if (Vector2.Distance(transform.position, playerPos.position + playerFollowOffset) > 0.1f)   //small value to offset inaccuracy
        {
            Move(playerPos, playerFollowSpeed, playerFollowOffset);
        }
        else
        {
            atk1CanShoot = true;
        }

        if (atk1CanShoot)
        {
            gun.BurstShoot(atk1ShotsPerBurst, atk1FireRate, atk1TimeUntilNextBurst);
        }


        if (Time.time > stopTime && !idling)
        {
            idling = true;
            
            if (!isChangingAttack)
            {
                StartCoroutine(StartNextAttack(idleBufferTime));
            }
        }
    } 

    //Preset targets to shoot at, cycles through an array of them
    private void Attack2()
    {
        if (!inPosition)
        {
            GoToPosition();
            return;
        }
        
        if (!synch.BothPointsInPosition()) return;
        DoPattern();
    }

    
    //Missiles that follow player, can be shot down
    private void Attack3()
    {
        if (stopTime == 0f)
        {
            stopTime = Time.time + timeTillAttack1;
        }
        
        
        if (Time.time > stopTime && !idling)
        {
            idling = true;
                
            if (!isChangingAttack)
            {
                StartCoroutine(StartNextAttack(idleBufferTime));
            }
        }
    }


    //For Attack 2
    private IEnumerator StartNewPattern()
    {
        yield return new WaitForSeconds(timeToNextPattern);    //put editable timer afterwards
        
        idling = false;
       
        inPosition = false;
        nextPattern++;
        if (nextPattern > patterns.Length - 1)
        {
            //Reset
            nextPattern = 0;
            startPoint = patterns[0].patternPoints[0];
            endPoint = patterns[0].patternPoints[1];
            ChangeAttack();
        }
        
        //update movement points
        startPoint = patterns[nextPattern].patternPoints[0];
        endPoint = patterns[nextPattern].patternPoints[1];
    }


    
    private void GoToPosition()
    {
        if (Vector2.Distance(transform.position, startPoint.position) > 0f)
        {
            Move(startPoint, goToPositionMoveSpeed);
        }
        else
        {
            inPosition = true;
        }
    }
    
    private void DoPattern()
    {
        if (Vector2.Distance(transform.position, endPoint.position) > 0f)
        {
            Move(endPoint, inPatternMoveSpeed);
            gun.Shoot(patterns[nextPattern].atk2ShotsPerBurst, atk2FireRate);
        }
        else
        {
            if (!idling)
            {
                idling = true;
                StartCoroutine(StartNewPattern());
            }
        }
    }

    private void Idle()
    {
        if (Vector2.Distance(transform.position, idlePoint.position) > 0f)
        {
            Move(idlePoint, inPatternMoveSpeed);
            OnReachingIdle?.Invoke(this, EventArgs.Empty);
        }
    }
    
    
    private void ChangeAttack()
    {
        attackCounter++;
        if (attackCounter > maxAttackNumbers)
        {
            attackCounter = 1;      //reset
        }

        stopTime = 0f;
    }

    private IEnumerator StartNextAttack(float timeTillNextAttack)
    {
        isChangingAttack = true;
        yield return new WaitForSeconds(timeTillNextAttack);

       
        idling = false;
        inPosition = false;
        if (atk1CanShoot)
        {
            atk1CanShoot = false;
        }
        
        ChangeAttack();
        isChangingAttack = false;
    }
    
    
    void Move(Transform target, float moveSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void Move(Transform target, float moveSpeed, Vector3 followOffSet)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position + followOffSet, moveSpeed * Time.deltaTime);
    }
}
