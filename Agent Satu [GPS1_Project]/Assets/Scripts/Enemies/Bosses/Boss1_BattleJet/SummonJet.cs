using UnityEngine;

public class SummonJet : MonoBehaviour
{
    [SerializeField] private GameObject[] jetsToSummon;
    [SerializeField] private BossMiniJetHp bossMiniJetHp;
    void Start()
    {
        BossMiniJetHp.onReachingThresholdDelegate += BossMiniJetHp_OnReachingThreshold;
    }

    private void BossMiniJetHp_OnReachingThreshold()
    {
        foreach (GameObject gO in jetsToSummon)
        {
            gO.SetActive(true);
        }
        BossMiniJetHp.onReachingThresholdDelegate -= BossMiniJetHp_OnReachingThreshold;
    }
}
