using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        vcam.m_Lens.FieldOfView = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
