using UnityEngine;

public class EnemiesHpSystem : MonoBehaviour
{
    private int hpCount;
    public GameObject bulletDetect;
    public GameObject enemiesToDestroy;
    void Start()
    {
        hpCount = 5;
        Debug.Log("Start debug");
    }

    void Update()
    {
        if (hpCount <= 0)
        {
            DestroyImmediate(enemiesToDestroy, true);
            hpCount = 5;
        }
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter2D(Collider2D bulletDetect)
    {
        Debug.Log("hit");
        hpCount -= 1;

        Debug.Log(hpCount);
    }
}
