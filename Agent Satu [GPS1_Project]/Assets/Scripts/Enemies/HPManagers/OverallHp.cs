using UnityEngine;
using System.Collections.Generic;

//Attached to all gameObjects tagged with "Enemy" (auto - by SetupOverallHp script).
public class OverallHp : MonoBehaviour
{    
    //Components
    private SetupOverallHp setupOverallHp;
    
    
    //Fields
    //Take away [SerializeField after debugging]
    [SerializeField] private int overallHp = 4;
    private int legDismemberedCount = 0;


    void Awake()
    {
        setupOverallHp = SetupOverallHp.setupOverallInstance;
    }
    
    void Start()
    {
        CopyInitialHp();
    }
    
    public int GetOverallHp()
    {
        return overallHp;
    }

    public void SetOverallHp(int overallHp)
    {
        this.overallHp = overallHp;
    }

    public int GetLegDismemberedCount()
    {
        return this.legDismemberedCount;
    }

    public void SetLegDismemberedCount(int legDismemberedCount)
    {
        this.legDismemberedCount = legDismemberedCount;
    }
    
    
    //Get overall hp based on enemy type name in the EnemyType list
    private void CopyInitialHp()
    {
        List<SetupOverallHp.EnemyType> typeList = setupOverallHp.enemyTypesList;
        for (int i = 0; i < typeList.Count; i++)
        {
            if (typeList[i].GetEnemyTypeName() == transform.name)
            {
                overallHp = typeList[i].GetInitialHp();
            }
        }
    }
}
