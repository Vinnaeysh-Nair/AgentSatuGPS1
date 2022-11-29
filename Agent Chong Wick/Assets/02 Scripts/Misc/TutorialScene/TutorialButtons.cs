using UnityEngine;

public class TutorialButtons : MonoBehaviour
{
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private KeyCode controlButton;
    private bool isPressed = false;

    private void Start()
    {
        upButton.SetActive(true);
        downButton.SetActive(false);
    }

    void Update()
    {
        checkIfPressed();
    }

    void checkIfPressed()
    {
        if (Input.GetKeyDown(controlButton))
        {
            isPressed = true;
        }
        else if (Input.GetKeyUp(controlButton))
        {
            isPressed = false;
        }

        changeButtonImage();
    }

    public void changeButtonImage()
    {
        if (isPressed)
        {
            upButton.SetActive(false);
            downButton.SetActive(true);
        }
        else
        {
            upButton.SetActive(true);
            downButton.SetActive(false);
        }
    }
}
