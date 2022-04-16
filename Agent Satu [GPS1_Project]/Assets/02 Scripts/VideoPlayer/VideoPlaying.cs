using UnityEngine;
using UnityEngine.Video;

public class VideoPlaying : MonoBehaviour
{
    private VideoPlayer vp;
    private TransitionScript transition;

    private void OnDestroy()
    {
        vp.loopPointReached -= EndPointReached;
    }


    private void Start()
    {
        vp = GetComponent<VideoPlayer>();
        transition = FindObjectOfType<TransitionScript>();
        
        vp.loopPointReached += EndPointReached;
    }

    private void EndPointReached(VideoPlayer vp)
    {
        EndVideo();
    }

    protected void EndVideo()
    {
        transition.ReturnToMainMenu();
    }
}
