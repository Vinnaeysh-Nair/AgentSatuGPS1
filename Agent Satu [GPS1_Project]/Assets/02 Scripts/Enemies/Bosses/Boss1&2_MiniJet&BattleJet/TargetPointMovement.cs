using System.Collections;
using UnityEngine;
using System;


public class TargetPointMovement : MonoBehaviour
{
    //Components
    private SynchGunMovements synch;
    private PlayerMovement playerMovement;
    
    [SerializeField] private BattleJetGun gun;
    [SerializeField] private FlyIntoScene flyIntoScene;
    [SerializeField] private Transform idlePoint;
   
    private Transform startPoint;
    private Transform endPoint;
    private Vector2 playerPos;
    
    //Fields
    private bool inPosition;       

    [Header("General")]
    [Header("If isMiniJet checked, only one attack forever")]
    [SerializeField] private bool isMiniJet = false;
    [TextArea] [SerializeField] private string note;

    [Space][Space]
    [SerializeField] private int attackCounter = 1;
    [SerializeField] private float timeToStartBattle = 3f;
    [SerializeField] private float atkChangeBufferTime = 5f;

    private bool startFight = false;
    
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
    [SerializeField] private MissileLauncher missileLauncher;
    [SerializeField] private float timeTillAttack1;

    public event EventHandler OnReachingIdle;

   
    
    [Serializable]
    struct Patterns
    {
        public Transform[] patternPoints;
        public int atk2ShotsPerBurst;
        public float atk2InPatternMoveSpeed;
        public bool isBurst;
        
        [Header("Ignore if isBurst unchecked")]
        public float timePerBurst;
        
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


    void OnDestroy()
    {
        flyIntoScene.onReachingPointDelegate -= FlyIntoScene_OnReachingTarget;
        gun.OnFiredAllShots -= BattleJetGun_OnFiredAllShots;
    }
    
    
    void OnDisable()
    {
         gun.OnFiredAllShots -= BattleJetGun_OnFiredAllShots;
    }

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<PlayerMovement>();
        synch = transform.parent.GetComponent<SynchGunMovements>();
        
        gun.OnFiredAllShots += BattleJetGun_OnFiredAllShots;

        flyIntoScene.onReachingPointDelegate += FlyIntoScene_OnReachingTarget;

        
        
        //Wait time before starting battle
        startPoint = idlePoint;
        idling = true;
        
        //Attack 2 initial pos
        AssignPatternPoints(patterns[0]);
    }

    void FlyIntoScene_OnReachingTarget()
    {
        flyIntoScene.onReachingPointDelegate -= FlyIntoScene_OnReachingTarget;
        
        startFight = true;
        idling = false;
        inPosition = false;
        
        timeToStartBattle = Time.time + timeToStartBattle;
    }
    void Update()
    {
        if (!startFight) return;

        if (Time.time <= timeToStartBattle) return;
        
        if (idling)
        {
            Idle();
            return;
        }
        
        playerPos = playerMovement.GetPlayerPos();

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

    
    
    //Track playerPos and shoot with delay
    private void Attack1()
    {
        if (stopTime == 0f)
        {
            stopTime = Time.time + timeTillAttack2;
        }
        
        
        //Track player constantly
        if (Vector2.Distance(transform.position, playerPos + (Vector2) playerFollowOffset) > 0.1f)   //small value to offset inaccuracy
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

        //Mini jet only 1 attack
        if (isMiniJet) return;

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
            missileLauncher.gameObject.SetActive(true);
        }
        
  
        
        if (isMiniJet) return;
        
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
            
            //Mini jet only 1 attack
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
        if (Vector2.Distance(transform.position, startPoint.position) > 0.01f)
        {
            Move(startPoint.position, goToPositionMoveSpeed);
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
        if (Vector2.Distance(transform.position, endPoint.position) > 0.01f)
        {
            Move(endPoint.position, currPattern.atk2InPatternMoveSpeed);
            
            if (currPattern.isBurst)
            {
                gun.BurstShoot(currPattern.atk2ShotsPerBurst, atk2FireRate, currPattern.timePerBurst);
            }
            else
            {
                gun.Shoot(currPattern.atk2ShotsPerBurst, atk2FireRate);
            }
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
                if (currPattern.isBurst)
                {
                    gun.BurstShoot(currPattern.atk2ShotsPerBurst, atk2FireRate, currPattern.timePerBurst);
                }
                else
                {
                    gun.Shoot(currPattern.atk2ShotsPerBurst, atk2FireRate);
                }
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
        if (Vector2.Distance(transform.position, idlePoint.position) > 0.01f)
        {
            Move(idlePoint.position, goToIdleMoveSpeed);
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
    
    
    void Move(Vector2 targetPos, float moveSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void Move(Vector2 targetPos, float moveSpeed, Vector2 followOffSet)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos + followOffSet, moveSpeed * Time.deltaTime);
    }
}
