using UnityEngine;

public class EnemyHp : MonoBehaviour
{
   [Header("For view only: ")]
   [SerializeField] protected int currHp;
   
   
   public int GetHp()
   {
      return currHp;
   }
}
