using System;
using UnityEngine;

public class LevelCompleteCondition : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;

    private void OnDestroy()
    {
        EnemyHp.onLevelCompleteDelegate -= ActivateExitPoint;
    }


    void Start()
    {
        EnemyHp.onLevelCompleteDelegate += ActivateExitPoint;
    }

    private void ActivateExitPoint()
    {
        exitPoint.gameObject.SetActive(true);
        EnemyHp.onLevelCompleteDelegate -= ActivateExitPoint;
    }
}
