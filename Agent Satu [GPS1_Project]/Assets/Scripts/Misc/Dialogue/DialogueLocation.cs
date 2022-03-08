using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLocation : MonoBehaviour
{
    private Vector3 position;
    void Update()
    {
        position = transform.position;
    }
    
    public Vector3 DialoguePosition()
    {
        return position;
    }
}
