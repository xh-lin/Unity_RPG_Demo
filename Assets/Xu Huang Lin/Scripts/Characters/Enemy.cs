using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterStatus
{
    // public GameObject TheEnemy;
    public GameObject healthBarPrefab;
    public Transform healthBarContainer;
    public bool displayHealthBar;

    private GameObject hBar;
    private CapsuleCollider thisCollider;
    private Bounds screenBounds;
    private bool isDead;
    void Start()
    {
        // create health bar
        hBar = Instantiate(healthBarPrefab);
        hBar.GetComponent<Healthbar>().characterStatus = this;
        hBar.transform.SetParent(healthBarContainer);
        hBar.name = this.gameObject.name + " HealthBar";
        thisCollider = this.gameObject.GetComponent<CapsuleCollider>();
        hBar.SetActive(false);
        screenBounds = new Bounds(Vector3.zero, new Vector3(Screen.width * 2, Screen.height * 2, 20f));
    }


    void Update()
    {
        // health bar following
        Vector3 p = Camera.main.WorldToScreenPoint(this.gameObject.transform.position 
                                                   + Vector3.up * thisCollider.height / 1.5f);
        Vector2 p2 = p;
        if (displayHealthBar
            && screenBounds.Contains(p2)
            && p.z > 0f
            ) {
            hBar.transform.position = p2;
            hBar.SetActive(true);
        }
        else {  // disable health bar if outside of camera or too far
            hBar.SetActive(false);
        }

        // if HP <= 0, die
        // if (HP <= 0 && !isDead) Die();
    }


    public void Die()
    {
        isDead = true;
        Debug.Log("Enemy " + this.gameObject.name + " Die");
        // Drop loot
        if (weaponSlot != null)
             Instantiate(weaponSlot.itemPrefab, transform.position, transform.rotation);
        // destroy this
        Destroy(this.gameObject);
        // destroy health bar
        Destroy(hBar);

    }
}
