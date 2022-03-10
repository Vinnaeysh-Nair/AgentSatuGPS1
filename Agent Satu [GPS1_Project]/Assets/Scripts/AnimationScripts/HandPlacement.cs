using UnityEngine;

public class HandPlacement : MonoBehaviour
{
    //Components
    [Header("RigLayer Targets")]
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform rightHandIKTarget;

    private WeaponSwitching wepSwitch;
    

    [Header("Positions for each guns")]
    [SerializeField] private HandPositions[] positionsArray;

    

    [System.Serializable]
    private class HandPositions
    {
        [SerializeField] private string name;
        public Transform leftHandPos;
        public Transform rightHandPos;
    }

    void Start()
    {
        wepSwitch = GetComponent<WeaponSwitching>();
    }

    
    private void Update()
    {
        HandPositions currPositions = positionsArray[wepSwitch.selectedWeapon];
        Vector2 currLeftHandPos = currPositions.leftHandPos.position;
        Vector2 currRightHandPos = currPositions.rightHandPos.position;
        
        leftHandIKTarget.position = currLeftHandPos;
        rightHandIKTarget.position = currRightHandPos;
    }
}
