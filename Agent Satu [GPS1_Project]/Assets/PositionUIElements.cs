using UnityEngine;

public class PositionUIElements : MonoBehaviour
{
    public Camera cam;
    public UIElement[] elements;

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
        foreach (UIElement element in elements)
        {
            element.elementTransform.position = (Vector2) element.targetPos.position + element.posOffset;
        }
    }
}
