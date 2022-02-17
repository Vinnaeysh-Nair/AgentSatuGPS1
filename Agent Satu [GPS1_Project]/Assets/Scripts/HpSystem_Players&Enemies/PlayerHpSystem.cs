using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{
    public int hpCountPlayer;
    public GameObject playerToDestroy;
    public GameObject bulletDetectEnemies;
    public string displayText;
    void Start()
    {
        hpCountPlayer = 5;
    }

    void Update()
    {
        if(hpCountPlayer <= 0)
        {
            DestroyImmediate(playerToDestroy, true);
            hpCountPlayer = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D bulletDetectEnemies)
    {
        Debug.Log(displayText);
        hpCountPlayer = hpCountPlayer - 1;
        
        Debug.Log(hpCountPlayer);
    }
}
