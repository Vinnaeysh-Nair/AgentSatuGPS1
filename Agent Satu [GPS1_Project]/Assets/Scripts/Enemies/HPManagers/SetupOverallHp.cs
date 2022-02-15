using UnityEngine;
using System.Collections.Generic;

//Inside EnemyHpManagers, to setup overall hp of each enemy types.
//Manually insert enemy types, hp values will be copied based on enemies of the same name as the list element.
public class SetupOverallHp : MonoBehaviour
{
    #region Singleton

    public static SetupOverallHp setupOverallInstance;
    void Awake()
    {
        setupOverallInstance = this;
    }
    
    
    #endregion
    
    void Start()
    {
        AttachHp();
    }
    
    [System.Serializable]
    public class EnemyType
    {
        [Header("Only insert one enemy of each type")]
        [SerializeField] private GameObject enemyType;
        [SerializeField] private int initialHp;

        public EnemyType(GameObject enemyType, int initialHp)
        {
            this.enemyType = enemyType;
            this.initialHp = initialHp;
        }

        public string GetEnemyTypeName()
        {
            return enemyType.name;
        }

        public int GetInitialHp()
        {
            return initialHp;
        }
    }

    public List<EnemyType> enemyTypesList;
    
    
    //Find enemies to attach the OverallHp script thru the "Enemy" tag
    private void AttachHp()
    {
        GameObject[] foundEnemy = GameObject.FindGameObjectsWithTag("Enemy");
    
        foreach (GameObject enemy in foundEnemy)
        {
            enemy.AddComponent<OverallHp>();
        }
    }
    
    //public EnemyType[] enemyTypeArr;
    
    //Automated ver, but only compares tags
    // void Start()
    // {
    //     // if (enemyTypesList.Count > 0) return;
    //     //
    //     // GameObject[] enemyTypes = GameObject.FindGameObjectsWithTag("Enemy");
    //     //
    //     // foreach (GameObject enemy in enemyTypes)
    //     // {
    //     //     EnemyType foundEnemyType = new EnemyType(enemy.transform.name, 0);
    //     //     enemyTypesList.Add(foundEnemyType);
    //     // }
    //     
    //     // GameObject[] enemyTypes = GameObject.FindGameObjectsWithTag("Enemy");
    //     //
    //     // // int i = 0;
    //     // // foreach (GameObject enemyType in enemyTypes)
    //     // // {
    //     // //     EnemyType foundEnemyType = new EnemyType(enemyType.name, 0);
    //     // //     enemyTypeArr[i] = foundEnemyType;
    //     // //     i++;
    //     // // }
    //
    //     // for (int i = 0; i < enemyTypes.Length; i++)
    //     // {
    //     //     EnemyType foundEnemyType = new EnemyType(enemyTypes[i].name, 0);
    //     //     enemyTypeArr[i] = foundEnemyType;
    //     // }
    // }
    
}
