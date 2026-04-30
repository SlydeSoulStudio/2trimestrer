using Unity.VisualScripting;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int health=100;

   public void TakeDamage(int damage)
    {
        Debug.Log("take damage");
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
  


}