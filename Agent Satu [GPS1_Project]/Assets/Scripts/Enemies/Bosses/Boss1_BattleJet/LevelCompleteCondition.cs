using UnityEngine;

public class LevelCompleteCondition : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;


    void Start()
    {
        BossMiniJetHp.onLevelCompleteDelegate += ActivateExitPoint;
    }

    void ActivateExitPoint()
    {
        BossMiniJetHp.onLevelCompleteDelegate -= ActivateExitPoint;
        exitPoint.gameObject.SetActive(true);
    }
}
