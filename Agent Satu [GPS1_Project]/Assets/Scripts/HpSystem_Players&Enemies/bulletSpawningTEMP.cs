using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletSpawningTEMP : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    //public PlayerAnimationController animCon;


    public float shootStanceDelay = 1f;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        Instantiate(bullet, firePoint.position , firePoint.rotation);
        //animCon.OnShooting();

        //IEnumerator allows delay after a task
        yield return new WaitForSeconds(shootStanceDelay);
        //animCon.OnStopShooting();
    }
}
