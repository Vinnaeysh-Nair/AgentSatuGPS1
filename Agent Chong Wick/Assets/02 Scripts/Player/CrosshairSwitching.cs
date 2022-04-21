using System;
using UnityEngine;


public class CrosshairSwitching : MonoBehaviour
{
    [SerializeField] private Crosshair[] crosshairs;
    [SerializeField] private WeaponSwitching wepSwitch;
    private SpriteRenderer spriteRenderer;
    private int currCrosshair = 0;

    [Serializable]
    public class Crosshair
    {
        [SerializeField] private string wepName;
        public int wepId;
        public Transform crosshairPrefab;
    }
    
    private void OnDestroy()
    {
        WeaponSwitching.OnWeaponChange -= WeaponSwitching_OnWeaponChange;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeCrosshair();
    
        WeaponSwitching.OnWeaponChange += WeaponSwitching_OnWeaponChange;
    }

    
    private void WeaponSwitching_OnWeaponChange()
    {
        ChangeCrosshair();
    }

    private void ChangeCrosshair()
    {
        currCrosshair = wepSwitch.selectedWeapon;

        Transform temp = crosshairs[currCrosshair].crosshairPrefab;
        spriteRenderer.sprite = temp.GetComponent<SpriteRenderer>().sprite;
        transform.localScale = temp.localScale;
    }
}
