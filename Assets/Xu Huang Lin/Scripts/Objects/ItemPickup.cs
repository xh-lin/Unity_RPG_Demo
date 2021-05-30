using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Control the item to be picked up
 */

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public Item Pickup()
    {
        Destroy(gameObject);
        return item;
    }
}
