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
    private bool inPosition;       

    [Header("General")]
    [SerializeField] private bool isMiniJet = false;
    [TextArea] [SerializeField] private string note;
    
    [Space][Space]
    [SerializeField] private int attackCounter = 1;
    [SerializeField] private float timeToStartBattle = 3f;
    [SerializeField] private float atkChangeBufferTime = 5f;
    
    private int nextPattern = 0;
    
    private bool idling = false;
    private bool isChangingAttack = false;
    
    private int maxAttackNumbers = 3;

    private float stopTime = 0f;
    private bool firedAllShots = false;

    
    [Header("Move Speeds")]
    [SerializeField] private float goToPositionMoveSpeed = 1f;
    [SerializeField] private float goToIdleMoveSpeed = 1f;
    

    
    
    [Header("[Attack 1]")]
    [SerializeField] private int atk1ShotsPerBurst;
    [SerializeField] private float atk1FireRate;
    [SerializeField] private float atk1TimeUntilNextBurst;
    private float nextFireTime = 0f;
    private bool atk1CanShoot = false;

    
    [SerializeField] private Vector3 playerFollowOffset;
    [SerializeField] private float playerFollowSpeed;
    
    [SerializeField] private float timeTillAttack2;


    [Space] [Header("[Attack 2]")]
    [SerializeField] private float atk2FireRate;

    [SerializeField] private float nextPatternBufferTime = 3f;
    [SerializeField] private Patterns[] patterns;
    private int nextPatternPoint = 0;  


    [Space]
    [Header("[Attack 3]")]
    [SerializeField] private float timeTillAttack1;

    
    public event EventHandler OnReachingIdle;
    
    
    [Serializable]
    struct Patterns
    {
        public Transform[] patternPoints;
        public int atk2ShotsPerBurst;
        public float atk2InPatternMoveSpeed;
        
        [Header("[If unchecked, goes to idle immediately after reaching endPoint]")]
        public bool keepShootingAfterReachingEndPoint;
        [Header("[Number of Fired Shots takes priority, if time ran out but still have shots, keeps firing]")]
        public float timeStayingAtEndPoint;
    }


    public bool GetInPosition()
    {
        return inPosition;  
    }


    private void BattleJetGun_OnFiredAllShots(object sender, EventArgs e)
    {
        firedAllShots = true;
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

        
        //Additional Settings for Mini Jet, only use Attack 2
        if (isMiniJet)
        {
            attackCounter = 2;
        }
        
        
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
        AssignPatternPoints(patterns[0]);
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
            firedAllShots = false;
        }

        if (atk1CanShoot && !firedAllShots)
        {
            gun.BurstShoot(atk1ShotsPerBurst, atk1FireRate, atk1TimeUntilNextBurst);
        }


        if (Time.time > stopTime && !idling)
        {
            idling = true;
            
            if (!isChangingAttack)
            {
                StartCoroutine(StartNextAttack(atkChangeBufferTime));
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
                StartCoroutine(StartNextAttack(atkChangeBufferTime));
            }
        }
    }


    //For Attack 2
    private IEnumerator StartNewPattern()
    {
        yield return new WaitForSeconds(nextPatternBufferTime);    //put editable timer afterwards

        stopTime = 0f;
        
        idling = false;
        inPosition = false;
        firedAllShots = false;

        nextPatternPoint = 0;
        nextPattern++;
        if (nextPattern > patterns.Length - 1)
        {
            //Reset
            nextPattern = 0;
            
            //update movement points
            AssignPatternPoints(patterns[0]);
            
            
            if (!isMiniJet)
            {
                ChangeAttack();
            }
        }
        
        //update movement points
        AssignPatternPoints(patterns[nextPattern]);
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
        Patterns currPattern = patterns[nextPattern];
        
        
        //Movement and attack
        if (Vector2.Distance(transform.position, endPoint.position) > 0f)
        {
            Move(endPoint, currPattern.atk2InPatternMoveSpeed);
            gun.Shoot(currPattern.atk2ShotsPerBurst, atk2FireRate);
        }
        //If reached end point
        else
        {
            //If havent finished all pattern points yet, increment, skip idle
            if (nextPatternPoint + 1 < currPattern.patternPoints.Length)
            {
                nextPatternPoint++;

                startPoint = endPoint;  
                endPoint = currPattern.patternPoints[nextPatternPoint];
                return;
            }
            
            //If want to keep shooting, dont go idle first
            if (currPattern.keepShootingAfterReachingEndPoint)
            {
                gun.Shoot(currPattern.atk2ShotsPerBurst, atk2FireRate);
            }
            else
            {
                if (!idling)
                {
                    idling = true;
                    StartCoroutine(StartNewPattern());
                }
            }
            
            //Duration it stays shooting after reaching endPoint
            if (stopTime == 0f)
            {
                stopTime = Time.time + currPattern.timeStayingAtEndPoint;
            }
            
            //If havent reached duration yet, keep shooting
            if (Time.time < stopTime) return;
            
            
            //If fired all shots, idle
            if (firedAllShots)
            {
                if (!idling)
                {
                    idling = true;
                    StartCoroutine(StartNewPattern());
                }
            }
        }
    }

    private bool IsSinglePatternPoint(Patterns pattern)
    {
        if (pattern.patternPoints.Length == 1)
        {
            return true;
        }
        return false;
    }

    private void AssignPatternPoints(Patterns pattern)
    {
        if (IsSinglePatternPoint(pattern))
        {
            startPoint = idlePoint;
            endPoint = pattern.patternPoints[0];
        }
        else
        {
            startPoint = pattern.patternPoints[0];
            endPoint = pattern.patternPoints[1];
        }
    }

    private void Idle()
    {
        if (Vector2.Distance(transform.position, idlePoint.position) > 0f)
        {
            Move(idlePoint, goToIdleMoveSpeed);
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
