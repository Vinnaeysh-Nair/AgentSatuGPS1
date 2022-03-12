using UnityEngine;
using UnityEngine.UI;
public class BarChange : MonoBehaviour
{
    private Slider bar;
    //private Slider bulletTimeBar;

    void Start()
    {
       bar = GetComponent<Slider>();
    }

    public void SetBarAmount(float amt)
    {
        bar.value = amt;
    }
}
