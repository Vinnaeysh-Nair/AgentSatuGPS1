using UnityEngine;

public class FallOffMap : MonoBehaviour
{
    #region Singleton
    public static FallOffMap Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion

    public void FallToDeath()
    {
        PlayerMain.Instance.PlayerHpSystem.TakeDamage(1000);
    }

    public bool IsOutOfMap(float posY)
    {
        if (posY < transform.position.y)
        {
            return true;
        }

        return false;
    }
}
