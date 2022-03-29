using UnityEngine;
using System;

public class MiyaAtk3Detection : MonoBehaviour
{
    public static event Action OnPlayerEnter;


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
      
            if (OnPlayerEnter != null)
            {
                OnPlayerEnter.Invoke();
            }
        }
    }
}
