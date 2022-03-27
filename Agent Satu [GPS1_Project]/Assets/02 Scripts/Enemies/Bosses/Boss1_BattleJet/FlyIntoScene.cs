using UnityEngine;

public class FlyIntoScene : MonoBehaviour
{
    [SerializeField] private Transform pointToGo;
    [SerializeField] private float moveSpeed = 1f;
    
    public delegate void OnReachingPoint();
    public event OnReachingPoint onReachingPointDelegate;

    private bool startedShooting = false;

    void FixedUpdate()
    {
        if (!ReachedPoint())
        {
            transform.position =
                Vector2.MoveTowards(transform.position, pointToGo.position, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (!startedShooting)
            {
                startedShooting = true;
                if (onReachingPointDelegate != null)
                {
                    onReachingPointDelegate.Invoke();
                }
            }
        }
    }

    private bool ReachedPoint()
    {
        if (Vector2.Distance(transform.position, pointToGo.position) > .01f)
        {
            return false;
        }

        return true;
    }
}
