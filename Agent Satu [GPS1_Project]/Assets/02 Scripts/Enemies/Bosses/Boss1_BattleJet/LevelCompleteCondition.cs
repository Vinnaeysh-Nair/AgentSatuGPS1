using System;
using UnityEngine;

public class LevelCompleteCondition : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;

    private void OnDestroy()
    {
        BossMiniJetHp.onLevelCompleteDelegate -= ActivateExitPoint;
    }


    void Start()
    {
        BossMiniJetHp.onLevelCompleteDelegate += ActivateExitPoint;
    }

    private void ActivateExitPoint()
    {
        exitPoint.gameObject.SetActive(true);
        BossMiniJetHp.onLevelCompleteDelegate -= ActivateExitPoint;
    }
}
