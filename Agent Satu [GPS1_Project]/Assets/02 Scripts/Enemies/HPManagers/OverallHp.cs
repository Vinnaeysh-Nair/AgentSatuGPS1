using UnityEngine;
using System.Collections.Generic;
using System;

//Attached to all gameObjects tagged with "Enemy" (auto - by SetupOverallHp script).
public class OverallHp : EnemyHp
{    
    //Components
    private Ragdoll ragdoll;
    private SpawnPickups spawnPickups;

    private List<LimbHp> limbHps;
    
    //Fields
    [Header("Edit: ")]
    [SerializeField] private int initialOverallHp;

    
    private int legDismemberedCount = 0;
    private bool isHeadDismembered = false;


    public delegate void OnDeath();
    public event OnDeath onDeathDelegate;

    private bool _isDead = false;
    

    public delegate void OnDamaged();
    public event OnDamaged onDamagedDelegate;

    //Sound
    private SoundManager _soundManager;
    
    [Header("Sound")]
    [SerializeField] private AudioClip deathSound;

    void Start()
    {
        _soundManager = SoundManager.Instance;
        
        ragdoll = transform.GetChild(0).GetComponent<Ragdoll>();
        spawnPickups = GetComponent<SpawnPickups>();
        
        base.currHp = initialOverallHp;
    }


    
    //Getter
    public int GetOverallHp()
    {
        return initialOverallHp;
    }
    
    public int GetLegDismemberedCount()
    {
        return this.legDismemberedCount;
    }

    public bool GetIsHeadDismembered()
    {
        return isHeadDismembered;
    }

    //Setter
    public void SetOverallHp(int overallHp)
    {
        this.initialOverallHp = overallHp;
    }
    
    public void SetLegDismemberedCount(int legDismemberedCount)
    {
        this.legDismemberedCount = legDismemberedCount;
    }
    
    public void SetIsHeadDismemberedToTrue()
    {
        this.isHeadDismembered = true;
    }
    
    public void TakeOverallDamage(int dmg, Vector2 flingDirection)
    {
        //Stop decrementing starting at 0
        if (currHp <= 0) return;
        currHp -= dmg;
        
        //Trigger detection when damaged
        if (onDamagedDelegate != null)
        {
            onDamagedDelegate.Invoke();
        }
        
        //Die
        if (currHp <= 0)
        {
            Die(flingDirection);
            enabled = false;
        }
    }

    public void Die(Vector2 flingDirection)
    {
        if (!enabled) return;   
        
        _soundManager.PlayEffect(deathSound, true);
        
        //Disable unwanted components
        if (transform.TryGetComponent(out EnemyAI_Melee enemyAIMelee))
        {
            enemyAIMelee.enabled = false;
        }

        if (transform.TryGetComponent(out EnemyAI_Ranged enemyAIRanged))
        {
            enemyAIRanged.enabled = false;
        }

        if (transform.TryGetComponent(out Enemy_Agro enemyAgro))
        {
            enemyAgro.enabled = false;
        }

        if (transform.TryGetComponent(out Collider2D collider))
        {
            collider.enabled = false;
        }
        
        ragdoll.ActivateRagdoll(flingDirection);
        spawnPickups.enabled = true;
        
        if (onDeathDelegate != null)
        {
            onDeathDelegate.Invoke();
        }
    }
}
