using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    public RawImage backgroundPicture;
    public float height;
    public float length;

    void Update()
    {
        backgroundPicture.uvRect = new Rect(backgroundPicture.uvRect.position + new Vector2(length, height) * Time.deltaTime, backgroundPicture.uvRect.size);
    }
}
