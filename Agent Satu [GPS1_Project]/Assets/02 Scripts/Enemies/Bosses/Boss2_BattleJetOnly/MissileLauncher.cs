using System.Collections;
using UnityEngine;


public class MissileLauncher : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int numberOfMissiles = 3;


    [Header("Attack")] 
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform spawnPosContainer;
    [SerializeField] private Transform[] _spawnPosArray;
    
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
    }


    void Start()
    {
        int size = spawnPosContainer.childCount;
        _spawnPosArray = new Transform[size];
        for (int i = 0; i < size; i++)
        {
            _spawnPosArray[i] = spawnPosContainer.GetChild(i);
        }
        
        StartLaunchAnimation();
    }

    public void StartLaunchAnimation()
    {
        StartCoroutine(LaunchMissiles());
    }
    
    //attack
    private void SpawnMissiles()
    {
        for (int i = 0; i < numberOfMissiles; i++)
        {
            print("asdf");
            int spawnPosIndex = Random.Range(0, _spawnPosArray.Length);

            _pooler.SpawnFromPool(missile.name, _spawnPosArray[spawnPosIndex].position, Quaternion.identity);
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
    }

    private IEnumerator DisableMissile(GameObject missile)
    {
        yield return new WaitForSeconds(3f);
        missile.SetActive(false);
    }
}
