using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    //Attached to FX effects
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}
