using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterStatus
{
    public GameObject twoHWeaponSlot;
    public GameObject twoHWeaponOnHand;
    private AttackDetector ad;
    private int IEnumCount;
    #region Singleton
    public static Player _instance;
    private void Awake()
    {
        if (_instance == null) { 
            // Assigning only if there are no other instances in memory. 
            _instance = this; 
        } 
        else if (_instance != null) {
            // Destroying itself if detects duplication. 
            Debug.LogError("more than on Player");
        }
    }
    #endregion

    // hide the weapon on back, if on hand
    public void TwoHWeaponSlotSetActive(bool visible)
    {
        twoHWeaponSlot.SetActive(visible);
        ad = twoHWeaponOnHand.GetComponent<AttackDetector>();
        if (ad == null) Debug.LogError("No AttackDetector attached to twoHWeaponOnHand");
    }

    public IEnumerator Attack(float waitTime)
    {
        if (ad != null) {
            IEnumCount++;
            ad.isDealingDamage = true;
            yield return new WaitForSeconds(waitTime);
            IEnumCount--;
            if (IEnumCount == 0) {
                ad.isDealingDamage = false;
            }
        }
    }

}
