using UnityEngine;
using System.Collections;


public class BossMiniJetHp : BossHp
{
    [Header("Ignore if this is jet to be summoned")]
    [SerializeField] [Range(0f, 1f)] private float summonThreshold;

    [Header("Fx")] 
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject explosionVFX;
    
    private static int killedBossesCount = 0;

    public delegate void OnReachingThreshold();
    public static event OnReachingThreshold onReachingThresholdDelegate;


    private FlyIntoScene _flyIntoScene;
    private bool _canTakeDamage = false;


    void OnDestroy()
    {
        _flyIntoScene.onReachingPointDelegate -= FlyIntoScene_OnReachingPoint;
    }
    
    void Start()
    {
        _flyIntoScene = GetComponent<FlyIntoScene>();
        _flyIntoScene.onReachingPointDelegate += FlyIntoScene_OnReachingPoint;
        

        //reset static value
        killedBossesCount = 0;
        
        currHp = initialHp;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Bullet"))
        {
            if (!_canTakeDamage) return;
            
            BulletBehaviour bullet = col.collider.GetComponent<BulletBehaviour>();
           
            //Visual
            bullet.SpawnBulletImpactEffect();
            
            //Damage
            int dmg = bullet.GetBulletDmg();
            TakeDamage(dmg);
            
            col.gameObject.SetActive(false);
        }
    }

    private new void TakeDamage(int dmg)
    {
        if (currHp <= 0) return;
        
        
        currHp -= dmg;
        
        float percentage = (float) currHp / initialHp;
        healthBar.SetFillAmount(percentage);

        
        if (percentage <= summonThreshold)
        {
            if (onReachingThresholdDelegate != null)
            {
                onReachingThresholdDelegate.Invoke();
                
                //temporarily cant take damage
                _canTakeDamage = false;
                StartCoroutine(SetCanTakeDamageToTrue());
            }
        }

        if (currHp > 0) return;
        
        //Death logic
        SoundManager.Instance.PlayEffect(explosionSound);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        killedBossesCount++;
       
        //print("ded" + killedBossesCount);
        if (killedBossesCount == 3)
        {
            CompleteLevel();
        }
           
        gameObject.SetActive(false);
    }

    private void FlyIntoScene_OnReachingPoint()
    {
        _canTakeDamage = true;
    }
    
    private IEnumerator SetCanTakeDamageToTrue()
    {
        yield return new WaitForSeconds(5f);
        _canTakeDamage = true;
    }
}
