using System.Collections;
using UnityEngine;

public class TargetPointMovement : MonoBehaviour
{
    //Components
    private SynchGunMovements synch;
    
    
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



    
    [System.Serializable]
    struct Patterns
    {
        public Transform[] patternPoints;
    }

    [SerializeField] private Patterns[] patterns;


    
    void Start()
    {
        synch = transform.parent.GetComponent<SynchGunMovements>();
        
        //startPoint = patterns[0].patternPoints[0];
        //endPoint = patterns[0].patternPoints[1];
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
        DoPattern();
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(3f);    //put editable timer afterwards
        idling = false;
        
        inPosition = false;
        startPoint = patterns[0].patternPoints[0];
        endPoint = patterns[0].patternPoints[1];
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
        if (Vector2.Distance(transform.position, startPoint.position) > 0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPoint.position, getInPositionMoveSpeed * Time.fixedDeltaTime);
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
            transform.position = Vector2.MoveTowards(transform.position, endPoint.position, inPatternMoveSpeed * Time.fixedDeltaTime);
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
            transform.position = Vector2.MoveTowards(transform.position, idlePoint.position, inPatternMoveSpeed * Time.fixedDeltaTime);
        }
    }
}
