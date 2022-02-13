using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{
    public int hpCountPlayer;
    public GameObject playerToDestroy;
    public GameObject bulletDetectEnemies;

    void Start()
    {
        hpCountPlayer = 5;
    }

    void Update()
    {
        if(hpCountPlayer >= 0)
        {
            DestroyImmediate(playerToDestroy, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D bulletDetectEnemies)
    {
        Debug.Log("Player hit");
        hpCountPlayer -= 1;
        
        Debug.Log(hpCountPlayer);
    }
}
