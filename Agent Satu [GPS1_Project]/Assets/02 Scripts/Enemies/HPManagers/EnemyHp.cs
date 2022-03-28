using UnityEngine;

public class EnemyHp : MonoBehaviour
{
   [Header("For view only: ")]
   [SerializeField] protected int currHp;
   
   
   public delegate void OnLevelComplete();
   public static event OnLevelComplete onLevelCompleteDelegate;

   
   public int GetCurrHp()
   {
      return currHp;
   }

   protected void CompleteLevel()
   {
      if (onLevelCompleteDelegate != null)
      {
         onLevelCompleteDelegate.Invoke();
      }
   }
}
