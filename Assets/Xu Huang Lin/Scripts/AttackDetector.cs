using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    public bool isDealingDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isDealingDamage) {
            isDealingDamage = false;
            float damage;
            damage = Player._instance.attack;
            other.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
            print("Dealing " + damage + " damage to " + other.name);
        }
            
    }


}
