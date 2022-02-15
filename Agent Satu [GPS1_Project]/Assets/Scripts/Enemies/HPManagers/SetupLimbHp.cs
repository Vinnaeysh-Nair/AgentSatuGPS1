using UnityEngine;
using System.Collections.Generic;

//Inside EnemyHpManagers, to setup hp for each limb types.
//Manually insert each enemy types into the enemyArr, if list doesn't generate yet, enter and exit PlayMode.
//When regenerating limbs, set list size to 0 to reset it.
[ExecuteInEditMode]
public class SetupLimbHp : MonoBehaviour
{
    
    #region Singleton, Initialise pooler instance, Populate setup list

    public static SetupLimbHp setupLimbInstance;
    void Awake()
    {
        setupLimbInstance = this;
    }

    void Start()
    {
        PopulateList();
    }
    
    #endregion
    
    
    //Fields
    [SerializeField] private Transform[] enemyArr;
    public List<Limb> limbList;

    
    //Limb class for setting limb hp
    [System.Serializable]
    public class Limb
    {
        [Header("Edit Hp only")] 
        [SerializeField] private string enemyName;
        [SerializeField] private Transform limb;
        [SerializeField] private int initialHp = 0;
        
        
        public Limb(Transform limb)
        {
            this.enemyName = limb.parent.name;
            this.limb = limb;
        }

        //Getter 
        public string GetLimbName()
        {
            return limb.name;
        }
        
        public int GetInitialHp()
        {
            return initialHp;
        }


        //Setters
        public void SetLimb(Transform limb)
        {
            this.limb = limb;
        }
    }
    
    
    //Use this list to adjust hp of limbs of the same type
    void PopulateList()
    {
        foreach (Transform enemy in enemyArr)
        {
            //Get every limbs of the enemy
            for (int i = 0; i < enemy.childCount; i++)
            {
                Transform child = enemy.GetChild(i);
                
                //If it is a limb
                if (child.CompareTag("Limb"))
                {
                    Limb newLimb = new Limb(child);
                    string tempName = newLimb.GetLimbName();

                    //If limb already exists in list, skip
                    if (limbList.Find(limb => limb.GetLimbName() == tempName) != null) continue;
                    
                    limbList.Add(newLimb);
                }
            }
        }
    }

    // void PopulateList()
    // {
    //     if (limbList.Count > 0) return;
    //     
    //     for (int i = 0; i < pooler.pools.Count; i++)
    //     {
    //         var objPool = pooler.pools[i];
    //         Transform obj = objPool.GetGameObj().transform;
    //         
    //         if (obj.CompareTag("Limb"))
    //         {
    //             Limb newLimb = new Limb(obj);
    //             limbList.Add(newLimb);
    //         }
    //     }
    //
    // }
}
