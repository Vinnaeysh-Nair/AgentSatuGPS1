using System.Collections.Generic;
using UnityEngine;

//Create pools from the GameObject Arr.
//Use the SpawnFromPool() function to spawn pooled gameObjects.
/*Can create pools for gameObjects parented to a container(ex. limbs parented to an enemy container) or a standalone gameObject (ex. bullet)
    - for this reason, make sure limbs are organized inside a container and tagged with "Limb".
    - for enemies, only drop one type of each enemies, as limbs of the same name can be reused by the pooler.
    - no additional rule for pooling standalone gameObjects, just drag and drop inside.
 */
public class ObjectPooler : MonoBehaviour
{
    #region Singleton

    public static ObjectPooler objPoolerInstance;

    void Awake()
    {
        objPoolerInstance = this;
    }

    #endregion

    //Components
    [Header("Insert gameObjects to pool here:")]
    [SerializeField] GameObject[] objectArr;
    
    //Fields
    private const int poolSize = 5;
    
    
    [System.Serializable] 
    public class Pool
    {
        [SerializeField] private GameObject gameObj;
        private  string name;
        private  int size;

        public Pool(string name, GameObject gameObj)
        {
            this.name = name;
            this.gameObj = gameObj;
            this.size = poolSize;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return name;
        }
        public GameObject GetGameObj()
        {
            return gameObj;
        }
        public int GetSize()
        {
            return size;
        }
    }
    
    public Dictionary<string, Queue<GameObject>> poolDictionary;   
    public List<Pool> pools;


    void Start()
    {
        //Stops ObjectPooler from creating more pools than necessary when entering and exiting playmode.
        if (pools.Count > 0) pools.Clear();
        
        
        
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        //Mainly for pooling limbs, also for bullet and FX
        //Add characters in inspector
        foreach (GameObject obj in objectArr)
        {
            //For objects with Limb gameObjects as children
            if (obj.transform.childCount > 0)
            {
                //Finds all chidlren in obj, excludes bones, add to pools list
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    GameObject child = obj.transform.GetChild(i).gameObject;

                    if (child.CompareTag("Limb"))
                    {
                        Pool newPool = new Pool(child.name, child);
                        pools.Add(newPool);
                    }
                }
            }
            //For standalone objects
            else
            {
                Pool newPool = new Pool(obj.name, obj);
                pools.Add(newPool);
            }
        }


        //Add Queues of gameObjects
        foreach (Pool myPool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            
            //Instantiating, setting inactive & enqueueing into pool
            for (int i = 0; i < myPool.GetSize(); i++)
            {
                GameObject objToPool = Instantiate(myPool.GetGameObj());
                
                //Set up objToPool
                objToPool.name = myPool.GetName();
                objToPool.SetActive(false);

                objPool.Enqueue(objToPool);
            }

            //Add to dictionary
            poolDictionary.Add(myPool.GetName(), objPool);
        }
    }

    public GameObject SpawnFromPool(string objName, Vector3 position, Quaternion rotation)
    {
        //If pool doesn't exist
        if (!poolDictionary.ContainsKey(objName)) return null;
        
        
        //Spawn from pool
        GameObject objectToSpawn = poolDictionary[objName].Dequeue();

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        
        //Queue up again for future use
        poolDictionary[objName].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
