using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    BoxCollider2D target;

    void Start()
    {
        target = gameObject.GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D temp)
    {
        Debug.Log("sus");
        if (temp.tag == "Bullet")
            Destroy(target);
    }
}
