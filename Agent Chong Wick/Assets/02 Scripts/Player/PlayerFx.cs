using UnityEngine;

public class PlayerFx : MonoBehaviour
{
    [Header("Running FX")]
    [SerializeField] private GameObject runningFx;
    [SerializeField] private Transform runningFxSpawn;
    [SerializeField] private float timeBetweenDust = .2f;
    private float nextKickDustTime = 0f;

    [Header("Dodgeroll FX")]
    [SerializeField] private GameObject dodgerollFx;
    [SerializeField] private Transform dodgerollFxSpawn;
    
    void OnDestroy()
    {
        PlayerController.OnRunning -= PlayerController_OnRunning;
        PlayerController.OnDodgerolling -= PlayerController_OnDodgerolling;
    }

    void Start()
    {
        PlayerController.OnRunning += PlayerController_OnRunning;
        PlayerController.OnDodgerolling += PlayerController_OnDodgerolling;
    }

    private void PlayerController_OnRunning()
    {
        if (Time.time > nextKickDustTime)
        {
            Instantiate(runningFx, runningFxSpawn.position, Quaternion.identity);
            nextKickDustTime = Time.time + timeBetweenDust;
        }
    }

    private void PlayerController_OnDodgerolling()
    {
        Instantiate(dodgerollFx, dodgerollFxSpawn.position, Quaternion.identity);
    }
}
