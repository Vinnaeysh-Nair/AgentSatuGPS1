using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    
    class tBlock
    {
        [SerializeField] private GameObject[] temp2;
        [SerializeField] private CircleCollider2D cCollider;
    }

    private tBlock[] t1;
    private void Start()
    {
        //cCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D temp)
    {
        Debug.Log("Hit by" + temp);
        Destroy(gameObject);
        if (gameObject.tag == "Bullet")
        {
            Debug.Log("Hit by" + temp);
        }
    }
}
