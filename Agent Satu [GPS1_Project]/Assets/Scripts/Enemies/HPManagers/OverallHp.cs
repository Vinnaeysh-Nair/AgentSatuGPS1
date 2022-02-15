using UnityEngine;
using System.Collections.Generic;

//Attached to all gameObjects tagged with "Enemy" (auto - by SetupOverallHp script).
public class OverallHp : MonoBehaviour
{
    //Fields
    [SerializeField] private int overallHp = 4;
    private SetupOverallHp setupOverallHp;

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
