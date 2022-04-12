using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Ref:")]
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private AudioClip explosionSound;
    
    [SerializeField] private Transform pivot;

    [Header("Settings: ")] 
    [SerializeField] private int damage = 5;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int missileHp = 3;
    private Transform _missile;
    
    private PlayerMovement _playerMovement;
    private PlayerHpSystem _playerHpSystem;

    void Start()
    {
        _missile= transform.parent;
        
        PlayerMain playerMain = PlayerMain.Instance;
        ;
        _playerMovement = playerMain.PlayerMovement;
        _playerHpSystem = playerMain.PlayerHpSystem;
    }


    private void FixedUpdate()
    {
        PointAtPlayer();
        _missile.position = Vector2.MoveTowards(_missile.position, _playerMovement.GetPlayerPos(), moveSpeed);
        
    }
    
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Bullet"))
        {
            BulletBehaviour bullet = col.collider.GetComponent<BulletBehaviour>();
            TakeDamage(bullet.GetBulletDmg());
        }
        else
        {
            ExplodeFx();
            _missile.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ExplodeFx();
            _playerHpSystem.TakeDamage(damage);
            _missile.gameObject.SetActive(false);
        }
    }

    private void PointAtPlayer()
    {
        Vector2 dir = _playerMovement.GetPlayerPos() - (Vector2) pivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        float lerpedAngle = Mathf.LerpAngle(pivot.eulerAngles.z, angle, .05f);

        pivot.eulerAngles = new Vector3(0f, 0f, lerpedAngle);
    }

    private void ExplodeFx()
    {
        GameObject fx = Instantiate(explosionFx, transform.position, Quaternion.identity);
        fx.transform.localScale = pivot.localScale;

        SoundManager.Instance.PlayEffect(explosionSound);
    }

    private void TakeDamage(int dmg)
    {
        if (missileHp <= 0) return;
        missileHp -= dmg;

        if (missileHp > 0) return;
        
        ExplodeFx();
        _missile.gameObject.SetActive(false);
    }
}
