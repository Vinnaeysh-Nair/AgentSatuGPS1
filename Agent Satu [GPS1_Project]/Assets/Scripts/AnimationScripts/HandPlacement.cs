using UnityEngine;

public class HandPlacement : MonoBehaviour
{
    //Components
    [SerializeField] private bool isPlayer = false;
    
    [Header("RigLayer Targets")]
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform rightHandIKTarget;

    private WeaponSwitching wepSwitch;
    

    [Header("Positions for each guns")]
    [SerializeField] private HandPositions[] positionsArray;

    private Transform currLeftHandPos;
    private Transform currRightHandPos;

    

    [System.Serializable]
    private class HandPositions
    {
        [SerializeField] private string name;
        public Transform leftHandPos;
        public Transform rightHandPos;
    }

    void Start()
    {
        if (isPlayer)
        {
            wepSwitch = GetComponent<WeaponSwitching>();
            wepSwitch.OnWeaponChange += WeaponSwitching_OnWeaponChange;
        }
        
        InitialPos();
    }

    void OnDestroy()
    {
        if (isPlayer)
        {        
            wepSwitch.OnWeaponChange -= WeaponSwitching_OnWeaponChange;
            
        }
    }

    
    private void Update()
    {
        leftHandIKTarget.position = currLeftHandPos.position;
        rightHandIKTarget.position = currRightHandPos.position;
    }

    private void WeaponSwitching_OnWeaponChange(object sender, System.EventArgs e)
    {
        HandPositions currPositions = positionsArray[wepSwitch.selectedWeapon];

        currLeftHandPos = currPositions.leftHandPos;
        currRightHandPos = currPositions.rightHandPos;
    }

    void InitialPos()
    {
        HandPositions currPositions = positionsArray[0];
        
        currLeftHandPos = currPositions.leftHandPos;
        currRightHandPos = currPositions.rightHandPos;
    }
}
