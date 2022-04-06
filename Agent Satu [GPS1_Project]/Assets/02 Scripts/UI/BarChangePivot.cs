using UnityEngine;

public class BarChangePivot : MonoBehaviour
{
    [SerializeField] private Transform fillPivot;

    
    public void SetFillAmount(float amt)
    {
        fillPivot.localScale = new Vector3(amt, 1f, 1f);
    }
}
