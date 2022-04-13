using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class MissileLauncher : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int numberOfMissiles = 3;


    [Header("Attack")] 
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform spawnPosContainer;
    private Transform[] _spawnPosArray;
    
    [Header("Animation")]
    [SerializeField] private GameObject missileAnim;
    [SerializeField] private float launchRate = 1f;
    [SerializeField] private float missileFlySpeed = 1f;

    [SerializeField] private float minOffsetX = -.5f;
    [SerializeField] private float maxOffsetX = .5f;
    
    
    private ObjectPooler _pooler;

    void Awake()
    {
        _pooler = ObjectPooler.objPoolerInstance;
        
        int size = spawnPosContainer.childCount;
        _spawnPosArray = new Transform[size];
        for (int i = 0; i < size; i++)
        {
            _spawnPosArray[i] = spawnPosContainer.GetChild(i);
        }
    }

    private void Start()
    {
        StartLaunchAnimation();
    }

    void OnEnable()
    {
        StartLaunchAnimation();
    }
    private void StartLaunchAnimation()
    {
        StartCoroutine(LaunchMissiles());
    }
    
    //attack
    private void SpawnMissiles()
    {
        for (int i = 0; i < numberOfMissiles; i++)
        {
            int spawnPosIndex = Random.Range(0, _spawnPosArray.Length);

            Vector2 pos = _spawnPosArray[spawnPosIndex].position;
            Vector2 offsetX = new Vector2(Random.Range(-3f, 3f), 0f);
            _pooler.SpawnFromPool(missile.name, pos + offsetX, Quaternion.identity);
        }
    }
    


    //animation
    private IEnumerator LaunchMissiles()
    {
        Vector2 launchPoint = transform.position;
        
        for (int i = 0; i < numberOfMissiles; i++)
        {
            Vector2 randOffsetX = new Vector2(Random.Range(minOffsetX, maxOffsetX), 0f);
            launchPoint += randOffsetX;
            
            GameObject launchedMissile = _pooler.SpawnFromPool(missileAnim.name, launchPoint, missileAnim.transform.rotation);
            launchedMissile.GetComponent<Rigidbody2D>().velocity = Vector2.up * missileFlySpeed;
            StartCoroutine(DisableMissile(launchedMissile));
            
            yield return new WaitForSeconds(1 / launchRate);
        }
        
        SpawnMissiles();
        gameObject.SetActive(false);
    }

    private IEnumerator DisableMissile(GameObject missileAnim)
    {
        yield return new WaitForSeconds(2f);
        missileAnim.SetActive(false);
    }
}
