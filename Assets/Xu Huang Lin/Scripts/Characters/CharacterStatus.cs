using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public float maxHP = 100; // Health Point
    public float maxSP = 100; // Stamina Point
    public float maxMP = 100; // Magic Point
    [Range(0f, 1f)] // 0% ~ 100%
    public float HP = 1f;
    [Range(0f, 1f)] // 0% ~ 100%
    public float SP = 1f;
    [Range(0f, 1f)] // 0% ~ 100%
    public float MP = 1f;

    public float attack = 1f;
    public float defense = 1f;
    public float magicAttack = 1f;
    public float magicDefense = 1f;
    [Range(-1f, 1f)] // -100% ~ 100%
    public float affinity = 0.01f;  // chance of a critical hit

    [Range(0f, 3f)] // 0% ~ 300%
    public float moveSpeed = 1f;
    [Range(0f, 3f)] // 0% ~ 300%
    public float attackSpeed = 1f;

    public Item[] inventory;
    public Item weaponSlot;

    public void ReceiveDamage(float damage)
    {
        float recvDamage = damage - defense;
        if (recvDamage <= 0) recvDamage = 1;
        float newHP = HP - (recvDamage / maxHP);
        if (newHP < 0f) newHP = 0f;
        HP = newHP;
    }

}
