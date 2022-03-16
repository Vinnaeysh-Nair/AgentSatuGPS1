using UnityEngine;

public class SummonJet : MonoBehaviour
{
    [SerializeField] private GameObject[] jetsToSummon;
  
    void Start()
    {
        BossMiniJetHp.onReachingThresholdDelegate += BossMiniJetHp_OnReachingThreshold;
    }

    private void BossMiniJetHp_OnReachingThreshold()
    {
        BossMiniJetHp.onReachingThresholdDelegate -= BossMiniJetHp_OnReachingThreshold;
        foreach (GameObject gO in jetsToSummon)
        {
            gO.SetActive(true);
        }
    }
}
