using System.Collections;
using UnityEngine;

public class TargetPointMovement : MonoBehaviour
{
    //Components
    private SynchGunMovements synch;
    private PlayerMovement playerMovement;
    
    
    //Fields
    [SerializeField] private BattleJetGun gun;
    [SerializeField] private float inPatternMoveSpeed = 1f;
    [SerializeField] private Transform idlePoint;
    public float getInPositionMoveSpeed = 1f;
    public bool inPosition;
    
    private int nextPattern = 0;
    private Transform startPoint;
    private Transform endPoint;

    private bool idling = false;

    [SerializeField] private int attackCounter = 1;
    
    [Header("Attack 1")]
    private Transform playerPos;
    [SerializeField] private Vector3 playerFollowOffset;
    [SerializeField] private float playerFollowSpeed;



    
    [System.Serializable]
    struct Patterns
    {
        public Transform[] patternPoints;
    }

    [SerializeField] private Patterns[] patterns;
    
    
    
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
        if (!inPosition)
        {
            GetInPosition();
            return;
        }
        
        if (idling)
        {
            Idle();
            return;
        }

        if (!synch.BothPointsInPosition()) return;
        switch (attackCounter)
        {
            case 1: Attack1();
                break;
            case 2: Attack2();
                break;
        }
    }

 

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(3f);    //put editable timer afterwards
        idling = false;
        
        inPosition = false;
        startPoint = patterns[0].patternPoints[0];
        endPoint = patterns[0].patternPoints[1];
    }
    
    
    //Track playerPos and shoot with delay
    private void Attack1()
    {
        print("attack1");
        //Move(playerPos, inPatternMoveSpeed, playerFollowOffset);
    }
    
    private void Attack2()
    {
        print("attack2");

        DoPattern();
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
        }
        
        //update movement points
        startPoint = patterns[nextPattern].patternPoints[0];
        endPoint = patterns[nextPattern].patternPoints[1];
    }


    private void GetInPosition()
    {
        //Attack1
        if (attackCounter == 1)
        {
            if (Vector2.Distance(transform.position, playerPos.position + playerFollowOffset) > 0f)
            {
                Move(playerPos, playerFollowSpeed, playerFollowOffset);
            }
            else
            {
                print("in position");
                inPosition = true;
            }
        }
        //Attack2
        else if (attackCounter == 2)
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
    }
    
    private void DoPattern()
    {
        if (Vector2.Distance(transform.position, endPoint.position) > 0f)
        {
            Move(endPoint, inPatternMoveSpeed);
            gun.Shoot();
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
