using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    [SerializeField] private FlyIntoScene _flyIntoScene;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        
        _flyIntoScene.onReachingPointDelegate += StartAnimate;
    }

    private void StartAnimate()
    {
        _animator.enabled = true;
        _flyIntoScene.onReachingPointDelegate -= StartAnimate;
    }
}
