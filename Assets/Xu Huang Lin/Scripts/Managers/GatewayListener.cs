using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GatewayListener : MonoBehaviour
{
    public bool isPlayerPassed = false;

    private GatewayRadio entranceRadio;
    private GatewayRadio exitRadio;
    private bool isChecking = false;    // if true, check which way does player go

    private void Start()
    {
        Transform entrance = transform.Find("entrance");
        Transform exit = transform.Find("exit");

        if (entrance != null) {
            entranceRadio = entrance.GetComponent<GatewayRadio>();
            if (entranceRadio == null) Debug.LogError(transform.name + " has no entrance Radio");
        }
        else Debug.LogError(transform.name + " has no entrance");
        if (exit != null) {
            exitRadio = exit.GetComponent<GatewayRadio>();
            if (exitRadio == null) Debug.LogError(transform.name + " has no exit Radio");
        }
        else Debug.LogError(transform.name + " has no exit");

    }

    private void Update()
    {
        if (!isChecking) {
            if (entranceRadio.isPlayerInside && exitRadio.isPlayerInside)
                isChecking = true;
        }
        else {  // if (isChecking == true)
            if (!entranceRadio.isPlayerInside && exitRadio.isPlayerInside) {
                isPlayerPassed = true;
                isChecking = false;
            }
            else if (entranceRadio.isPlayerInside && !exitRadio.isPlayerInside) {
                isPlayerPassed = false;
                isChecking = false;
            }
        }
    }
}
