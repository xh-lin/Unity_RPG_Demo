using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpGUIFollowing : MonoBehaviour
{
    public GameObject PickupPanel;

    private InventoryManager inventoryManager;
    private Vector3 idlePosition;
    private Text objectNameDisplay;

    private void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        idlePosition = PickupPanel.transform.position;
        objectNameDisplay = PickupPanel.transform.GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        if (inventoryManager.focus != null) {
            PickupPanel.transform.position = 
                Camera.main.WorldToScreenPoint(inventoryManager.focus.transform.position) 
                + Vector3.up * 150;
            objectNameDisplay.text = inventoryManager.focus.name;
        }
        else {
            PickupPanel.transform.position = idlePosition;
        }
    }
}
