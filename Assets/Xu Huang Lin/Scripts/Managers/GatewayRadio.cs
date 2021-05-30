using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayRadio : MonoBehaviour
{
    public bool isPlayerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = false;
    }
}
