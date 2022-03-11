using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;

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
    public bool inPosition;
    
    [Header("General")]
    [SerializeField] private int attackCounter = 1;
    private int maxAttackNumbers = 3;
    
    
    [Header("Move Speeds")]
    [SerializeField] private float getInPositionMoveSpeed = 1f;
    [SerializeField] private float inPatternMoveSpeed = 1f;
    private int nextPattern = 0;
    
    private bool idling = false;
    private bool isChangingAttack = false;
    
    [Header("[Attack 1]")]
    [SerializeField] private int atk1ShotsPerBurst;
    [SerializeField] private float atk1FireRate;
    [SerializeField] private float atk1TimeUntilNextBurst;
    [SerializeField] private float shootDelay = 1f;
    private float nextFireTime = 0f;
    private bool atk1CanShoot = false;
    
    [SerializeField] private Vector3 playerFollowOffset;
    [SerializeField] private float playerFollowSpeed;
    
    [SerializeField] private float timeTillAttack2;


    [Header("[Attack 2]")]
    [SerializeField] private int atk2ShotsPerBurst;
    [SerializeField] private float atk2FireRate;
    
    [SerializeField] private Patterns[] patterns;
    
    [Header("[Attack 3]")]
    [SerializeField] private float timeTillAttack1;

    public event EventHandler OnReachingTarget;
  
    
    
    [Serializable]
    struct Patterns
    {
        public Transform[] patternPoints;
    }

    

    
    
    void Start()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();
        synch = transform.parent.GetComponent<SynchGunMovements>();
        
        playerPos = playerMovement.transform;
        

        //Wait time before starting battle
        startPoint = idlePoint;
        idling = true;
        StartCoroutine(StartBattle());
    }
    
    void FixedUpdate()
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
        yield return new WaitForSeconds(3f);    //put editable timer afterwards
        
        idling = false;
        inPosition = false;
        
        //Attack 2 initial pos
        startPoint = patterns[0].patternPoints[0];
        endPoint = patterns[0].patternPoints[1];
    }
    
    
    //Track playerPos and shoot with delay
    private void Attack1()
    {
        //Track player constantly
        if (Vector2.Distance(transform.position, playerPos.position + playerFollowOffset) > 0f)
        {
            Move(playerPos, playerFollowSpeed, playerFollowOffset);
            atk1CanShoot = true;
        }
        else
        {
           // nextFireTime = Time.time + shootDelay;
           //gun.Shoot(atk1ShotsPerBurst, atk1FireRate, atk1TimeUntilNextBurst);
        }


        
       

        if (!isChangingAttack)
        {
            //StartCoroutine(StartNextAttack(timeTillAttack2));
        }
    }

    
    private void Attack2()
    {
        if (!inPosition)
        {
            GetInPosition();
            return;
        }
        
        if (!synch.BothPointsInPosition()) return;
        print("attack2");
        DoPattern();
    }

    private void Attack3()
    {
        print("attack3");

        if (!isChangingAttack)
        {
            StartCoroutine(StartNextAttack(timeTillAttack1));
        }
    }
    
    private void ChangeAttack()
    {
        attackCounter++;
        if (attackCounter > maxAttackNumbers)
        {
            attackCounter = 1;
        }
    }

    private IEnumerator StartNextAttack(float timeTillNextAttack)
    {
        isChangingAttack = true;
        
        yield return new WaitForSeconds(timeTillNextAttack);

        if (atk1CanShoot)
        {
            atk1CanShoot = false;
        }
        
        ChangeAttack();
        isChangingAttack = false;
    }

    
    private IEnumerator StartNewPattern()
    {
        yield return new WaitForSeconds(3f);    //put editable timer afterwards
        
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


    //For Attack 2
    private void GetInPosition()
    {
        if (Vector2.Distance(transform.position, startPoint.position) > 0f)
        {
            Move(startPoint, getInPositionMoveSpeed);
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
            gun.Shoot(atk2ShotsPerBurst, atk2FireRate);
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
        print("idling");
        if (Vector2.Distance(transform.position, idlePoint.position) > 0f)
        {
             Move(idlePoint, inPatternMoveSpeed);
             OnReachingTarget?.Invoke(this, EventArgs.Empty);
        }
    }

    void Move(Transform target, float moveSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
    }

    void Move(Transform target, float moveSpeed, Vector3 followOffSet)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position + followOffSet, moveSpeed * Time.fixedDeltaTime);
    }
}
