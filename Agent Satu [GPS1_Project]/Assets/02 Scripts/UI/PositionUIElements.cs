using UnityEngine;

public class PositionUIElements : MonoBehaviour
{
    public UIElement[] elements;
    private Camera cam;
    

    [System.Serializable]
    public class UIElement
    {
        public Transform elementTransform;
        public Transform targetPos;
        public Vector2 posOffset;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        GetComponent<Canvas>().enabled = true;
        
        foreach (UIElement element in elements)
        {
            element.elementTransform.position = (Vector2) element.targetPos.position + element.posOffset;
        }
    }
}
