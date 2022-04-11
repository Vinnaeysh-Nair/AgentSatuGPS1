using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    
    [Header("Settings: ")] 
    [SerializeField] private float moveSpeed = 1f; 
    private Transform _missile;
    
    private PlayerMovement _playerMovement;
    private float startingAngle;

    void Start()
    {
        _missile= transform.parent;
        startingAngle = _missile.eulerAngles.z;
        
        _playerMovement = PlayerMain.Instance.PlayerMovement;
    }


    private void FixedUpdate()
    {
        PointAtPlayer();
        //_missile.position = Vector2.MoveTowards(_missile.position, _playerMovement.GetPlayerPos(), moveSpeed);
        
    }
    
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        print("exploded");
        if (col.collider.CompareTag("Player"))
        {
            _missile.gameObject.SetActive(false);
        }
    }

    private void PointAtPlayer()
    {
        Vector2 dir = _playerMovement.GetPlayerPos() - (Vector2) pivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        pivot.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
