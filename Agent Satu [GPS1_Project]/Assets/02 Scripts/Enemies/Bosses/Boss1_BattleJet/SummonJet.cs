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
        foreach (GameObject jets in jetsToSummon)
        {
            if (jets == null)
            {
                print("Jet doesn't exist.");
            }
            else
            {
                jets.gameObject.SetActive(true);
            }
        }
    }
}
