using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

/*
 * Control the inventory interaction
 *  
 */

public class InventoryManager : MonoBehaviour
{
    public GameObject player;
    public LayerMask InteractMask;
    public float interactRadius = 2f;
    public GameObject InventoryPanel;
    public Transform weaponSlot;
    [NonSerialized]
    public GameObject focus = null;

    private bool isInFocus = false;
    private Item[] inventory;
    private int stock = 0;
    private Transform SlotsHolder;
    private int slotCount;
    private CharacterStatus playerStatus;


    private void Start()
    {
        foreach (Transform child in InventoryPanel.transform)   // get SlotsHolder
            if (child.name == "SlotsHolder") SlotsHolder = child;
        slotCount = SlotsHolder.childCount;
        //inventory = new Item[slotCount];
        playerStatus = player.GetComponent<CharacterStatus>();
        inventory = playerStatus.inventory = new Item[slotCount];
        
    }

    private void Update()
    {
        InteractUpdate();

    }

    void OnDrawGizmos() // SphereCast visualization of interaction range
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, interactRadius);
    }

    /*===========================================================================================*/

    /*
     * check if there's any interactble nearby 
     */
    void InteractUpdate()   // 
    {
        RaycastHit[] hits = Physics.SphereCastAll(player.transform.position, interactRadius, 
                                                  player.transform.forward, 0f, InteractMask);
        ItemPickup itempickup;

        if (hits.Length > 0) {  // if see an interactable object
            focus = hits[0].transform.gameObject;
            itempickup = focus.GetComponent<ItemPickup>();
            if (itempickup != null) {   // if it is a item pickup
                if (!isInFocus)
                    print("UI: Press F to pickup");
                isInFocus = true;
                if (Input.GetButtonDown("Interact"))
                    InventoryPut(itempickup); 
            }
        }
        else {
            focus = null;
            isInFocus = false;
        }
    }

    /*
     * pick up the item
     */
    bool InventoryPut(ItemPickup itempickup)
    {
        if (stock < inventory.Length) { // if inventory not full
            for (int i = 0; i < inventory.Length; i++) {
                if (inventory[i] == null) {
                    inventory[i] = itempickup.Pickup(); // put Item into inventory, and destroy itemPickup
                    SlotIconUpdate(i);
                    break;
                }
            }
            stock++;
            return true;
        }
        print("UI: Inventory Full");
        return false;
    }

    /*
     * update slot in inventory
     */
    void SlotIconUpdate(int slotIndex)
    {
        Transform slot = SlotsHolder.GetChild(slotIndex);

        if (inventory[slotIndex] == null) { // if this slot has no item
            foreach (Transform child in slot)
                Destroy(child.gameObject);  // destroy icon if any
        }
        else {
            string iconName = "icon_" + inventory[slotIndex].name;
            switch (slot.childCount) {
                case 0: // if this slot has no icon, creat a new icon, a new button
                    GameObject newImage = new GameObject();
                    newImage.name = iconName;
                    Image icon = newImage.AddComponent<Image>();
                    icon.sprite = inventory[slotIndex].icon;
                    icon.color = inventory[slotIndex].iconColor;
                    newImage.GetComponent<RectTransform>().SetParent(slot);
                    newImage.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                    newImage.AddComponent<Button>();
                    newImage.GetComponent<Button>().onClick.AddListener(UseThisItem);
                    slot.name = Convert.ToString(slotIndex);    // cheat to remember its index
                    break;
                case 1: // if current icon doesn't match current item, update icon
                    Transform child = slot.GetChild(0);
                    if (child.name != iconName) {
                        child.name = iconName;
                        child.GetComponent<Image>().sprite = inventory[slotIndex].icon;
                    }
                    break;
                default:
                    Debug.LogError("slot " + slotIndex + " has " + slot.childCount + "children");
                    break;
            }
        }


        
    }

    void UseThisItem()
    {
        int slotIndex = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        Item thisItem = inventory[slotIndex];
        Debug.Log("selected item: " + thisItem.name);
        if (thisItem.itemType == ItemType.Potion || thisItem.itemType == ItemType.Food) {
            ApplyEffects(thisItem.itemEffects);
            inventory[slotIndex] = null;
            SlotIconUpdate(slotIndex);
        }
        else if (thisItem.itemType == ItemType.Weapon || thisItem.itemType == ItemType.Armor) {
            Equip(thisItem);
            inventory[slotIndex] = null;
            SlotIconUpdate(slotIndex);
        }
    }

    void ApplyEffects(Effect[] effects)
    {
        foreach (Effect e in effects) {
           ApplyEffect(e);
        }
    }
    
    void ApplyEffect(Effect effect)
    {
        foreach (Effect.StatusEffect se in effect.statusEffects) {
            StartCoroutine(ApplyStatusEffect(se, 
                                             effect.isEquip, effect.isContinuous, effect.willUndo, 
                                             effect.duration, effect.interval));
        }
    }

    private IEnumerator ApplyStatusEffect(Effect.StatusEffect se, 
                                          bool isEquip, bool isContinuous, bool willUndo, 
                                          float duration, float interval)
    {
        float timePassed = 0f;  // check if duration is over
        bool continueIter = true;   // check if only iter once or more
        while (continueIter && timePassed <= duration) {
            yield return new WaitForSeconds(interval);  // wait for (interval) seconds
            switch (se.effectType) {
                case StatusEffectType.maxHP:
                    playerStatus.maxHP += se.value;
                    break;
                case StatusEffectType.maxSP:
                    playerStatus.maxSP += se.value;
                    break;
                case StatusEffectType.maxMP:
                    playerStatus.maxMP += se.value;
                    break;

                case StatusEffectType.HP:
                    float nextVal = playerStatus.HP + se.value;
                    if (nextVal >= 1f) nextVal = 1f;
                    else if (nextVal <= 0f) nextVal = 0f;
                    playerStatus.HP = nextVal;
                    break;
                case StatusEffectType.SP:
                    playerStatus.SP += se.value;
                    break;
                case StatusEffectType.MP:
                    playerStatus.MP += se.value;
                    break;

                case StatusEffectType.attack:
                    playerStatus.attack += se.value;
                    break;
                case StatusEffectType.defense:
                    playerStatus.defense += se.value;
                    break;
                case StatusEffectType.magicAttack:
                    playerStatus.magicAttack += se.value;
                    break;
                case StatusEffectType.magicDefense:
                    playerStatus.magicDefense += se.value;
                    break;

                case StatusEffectType.affinity:
                    playerStatus.affinity += se.value;
                    break;
                case StatusEffectType.moveSpeed:
                    playerStatus.moveSpeed += se.value;
                    break;
                case StatusEffectType.attackSpeed:
                    playerStatus.attackSpeed += se.value;
                    break;
            }

            timePassed += interval;
            if (!isContinuous) continueIter = false;
            if (willUndo && timePassed > duration) {
                se.value = -se.value;
                timePassed = 0f ;
                willUndo = false;
            }
        }
    }

    void Equip(Item equipment)
    {
        playerStatus.weaponSlot = equipment;
        GameObject go = Instantiate(equipment.itemPrefab);
        go.transform.SetParent(weaponSlot);
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.GetComponent<MeshCollider>().isTrigger = true;
        go.layer = 0;
        
    }
}
