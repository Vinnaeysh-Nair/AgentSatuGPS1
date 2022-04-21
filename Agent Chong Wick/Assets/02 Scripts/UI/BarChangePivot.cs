using UnityEngine;

public class BarChangePivot : MonoBehaviour
{
    [SerializeField] private Transform fillPivot;
    private SpriteRenderer _fillSprite;

    public SpriteRenderer FillSprite
    {
        get => _fillSprite;
    }

    
    void Awake()
    {
        _fillSprite = fillPivot.GetChild(0).GetComponent<SpriteRenderer>();
    }
    
    public void SetFillAmount(float amt)
    {
        fillPivot.localScale = new Vector3(amt, 1f, 1f);
    }

    public void SetFillColor(Color c)
    {
        _fillSprite.color = c;
    }

    public void SetVisible(bool status)
    {
        transform.gameObject.SetActive(status);
    }
}
