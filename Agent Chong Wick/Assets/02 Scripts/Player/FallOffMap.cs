using UnityEngine;

/*
 * The prefab containing this script is only put inside the scene whenever the player is expected to be able to fall out of map. (ex: Level 3-2)/
 * Uses its own transform's y-position to determine the map limit for it to count as player falling out of map.
 */


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
