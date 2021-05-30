

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxScript : MonoBehaviour
{
    public GameObject OpenPanel = null;
    public bool _isInsideTrigger = false;
    public bool isOpen;
    public GameObject box;
    public GameObject[] items;
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        OpenPanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isOpen == false)
        {
            GameObject.FindWithTag("Box").SetActive(true);
            GameObject.FindWithTag("Potion").SetActive(false);
        }
        */

        if (_isInsideTrigger){
            if (Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                box.SetActive(false);
                foreach (var i in items) {
                    i.SetActive(true);
                }
                isOpen = true;
                OpenPanel.SetActive(false);
            }

        }
        if (isOpen == false)
        {
            box.SetActive(true);
            foreach (var i in items)
            {
                i.SetActive(false);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isOpen)
        {
            Debug.Log("In");

            _isInsideTrigger = true;
            OpenPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _isInsideTrigger = false;
            OpenPanel.SetActive(false);
        }
    }

}
