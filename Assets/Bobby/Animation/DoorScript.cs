using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Animator _animator;

    public GameObject OpenPanel = null;

    private bool _isInsideTrigger = false;

	// Use this for initialization
	void Start () {
        OpenPanel.SetActive(false);
        Transform child = transform.Find("Door");
        if (child != null)
        {
            _animator = transform.Find("Door").GetComponent<Animator>();
        }
        else if(child == null)
        {
            
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _isInsideTrigger = true;
            OpenPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _isInsideTrigger = false;
            _animator.SetBool("open", false);
            OpenPanel.SetActive(false);
        }
    }

    private bool IsOpenPanelActive
    {
        get
        {
            return OpenPanel.activeInHierarchy;
        }
    }

    // Update is called once per frame
    void Update () {

        if(IsOpenPanelActive && _isInsideTrigger)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                OpenPanel.SetActive(false);
                _animator.SetBool("open", true);  
            }
        }
	}
}
