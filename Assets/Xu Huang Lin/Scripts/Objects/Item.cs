using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public ItemType itemType;
    public Sprite icon = null;
    public Color iconColor = Color.white;
    public GameObject itemPrefab;
    public Effect[] itemEffects;
}


