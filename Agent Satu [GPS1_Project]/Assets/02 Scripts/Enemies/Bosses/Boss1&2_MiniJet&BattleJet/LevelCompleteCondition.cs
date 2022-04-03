using UnityEngine;

public class LevelCompleteCondition : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;

    private void OnDestroy()
    {
        BossHp.onLevelCompleteDelegate -= ActivateExitPoint;
    }


    void Start()
    {
        BossHp.onLevelCompleteDelegate += ActivateExitPoint;
    }

    private void ActivateExitPoint()
    {
        exitPoint.gameObject.SetActive(true);
        BossHp.onLevelCompleteDelegate -= ActivateExitPoint;
    }
}
