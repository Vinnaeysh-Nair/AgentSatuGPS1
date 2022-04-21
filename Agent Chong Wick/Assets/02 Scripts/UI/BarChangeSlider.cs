using UnityEngine;
using UnityEngine.UI;
public class BarChangeSlider : MonoBehaviour
{
    private Slider bar;

    void Awake()
    {
       bar = GetComponent<Slider>();
    }

    public void SetBarAmount(float amt)
    {
        bar.value = amt;
    }
}
