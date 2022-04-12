using UnityEngine;

public class BossBattleJetHp : BossHp
{
    [Header("Ref:")] 
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private AudioClip explosionSound;
    
    
    [Header("Ignore pivot version")]
    [SerializeField] private BarChangeSlider hpBar;
    void Start()
    {
        currHp = initialHp;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Bullet"))
        {
            int dmg = col.collider.GetComponent<BulletBehaviour>().GetBulletDmg();
            
            TakeDamage(dmg);
            col.gameObject.SetActive(false);
        }
    }
    
    private new void TakeDamage(int dmg)
    {
        if (currHp <= 0) return;
        
        currHp -= dmg;
        
        float percentage = (float) currHp / initialHp;
        hpBar.SetBarAmount(percentage);
        
        if (currHp > 0) return;
        
        Die();
        CompleteLevel();
        gameObject.SetActive(false);
    }

    private void Die()
    {
        GameObject fx = Instantiate(explosionFx, transform.position, Quaternion.identity);
        fx.transform.localScale = transform.parent.localScale * 3f;

        SoundManager.Instance.PlayEffect(explosionSound);
    }
}
