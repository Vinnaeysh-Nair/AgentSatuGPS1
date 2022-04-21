using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    private CircleCollider2D targetCollider;
    private int targetHealth = 3;

    void Start()
    {
        targetCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if(targetHealth<=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D temp)
    {
        if (temp.gameObject.tag == "Bullet")
        {
            targetHealth--;
        }
    }
}
