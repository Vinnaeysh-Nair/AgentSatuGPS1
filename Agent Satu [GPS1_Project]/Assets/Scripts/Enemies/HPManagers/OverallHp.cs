using UnityEngine;
using System.Collections.Generic;

//Attached to all gameObjects tagged with "Enemy" (auto - by SetupOverallHp script).
public class OverallHp : MonoBehaviour
{    
    //Components
    private SetupOverallHp setupOverallHp;
    
    
    //Fields
    [SerializeField] private int overallHp = 4;
    [SerializeField] private int legDismemberedCount = 0;
    private bool isHeadDismembered = false;

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
    
    public int GetLegDismemberedCount()
    {
        return this.legDismemberedCount;
    }

    public bool GetIsHeadDismembered()
    {
        return isHeadDismembered;
    }

    public void SetOverallHp(int overallHp)
    {
        this.overallHp = overallHp;
    }
    
    public void SetLegDismemberedCount(int legDismemberedCount)
    {
        this.legDismemberedCount = legDismemberedCount;
    }
    
    public void SetIsHeadDismemberedToTrue()
    {
        this.isHeadDismembered = true;
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
