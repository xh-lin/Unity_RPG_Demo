using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityEngine.UI;
using RPGCharacterAnims;
using UnityEngine.SceneManagement;
using System.Linq;

/*
 * Control the In-Game UI
 *  
 */

public class InGameUIManager : MonoBehaviour
{
    public GameObject player;
    //public CharacterStatus playerStatus;
    public GameObject thirdPersonCamera;
    public Animator inGameMenuPanels;
    public Text statusText;
    public Animator deathPanel;
    public string mainMenuSceneName;

    public float EnemyHealthBarDisplayRadius;
    public LayerMask enemyMask;

    public Animator settingsPanel;
    public Animator exitPanel;

    private bool inGameMenuOpen = false;
    private FreeLookCam freeLookCam;
    private float camTurnSpeed;
    private CharacterStatus playerStatus;
    private RPGCharacterInputControllerFREE inputController;
    private bool playerAlive = true;
    //private Bounds screenBounds;
    private SphereCollider thisCollider;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // lock and hide cursor
        Cursor.visible = false;
        freeLookCam = thirdPersonCamera.GetComponent<FreeLookCam>();
        camTurnSpeed = freeLookCam.m_TurnSpeed; // backup camera turn speed
        playerStatus = player.GetComponent<CharacterStatus>();
        inputController = player.GetComponent<RPGCharacterInputControllerFREE>();
        //screenBounds = new Bounds(Vector3.zero, new Vector3(Screen.width * 2, Screen.height * 2, 20f));
        thisCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (playerAlive)
        {
            if (Input.GetButtonDown("OpenInGameMenu"))
            {
                
                inGameMenuPanels.SetBool("isHidden", !inGameMenuPanels.GetBool("isHidden"));
                inGameMenuOpen = !inGameMenuOpen;
                if (!inGameMenuOpen && !settingsPanel.GetBool("isHidden")) ToggleSettingsPanel();
                if (!inGameMenuOpen && !exitPanel.GetBool("isHidden")) ToggleExitPanel();
            }
            if (inGameMenuOpen)
            {
                StatusPanelUpdate();
                Cursor.lockState = CursorLockMode.None; // unlock and show cursor
                Cursor.visible = true;
                freeLookCam.m_TurnSpeed = 0f;   // lock camera
                inputController.DisableInputs = true;
                Time.timeScale = 0.1f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                freeLookCam.m_TurnSpeed = camTurnSpeed;
                inputController.DisableInputs = false;
                Time.timeScale = 1f;
            }
        }

        if (playerStatus.HP <= 0 && playerAlive)
        { // player die
            Debug.Log("Player Die");
            playerAlive = false;

            inGameMenuPanels.SetBool("isHidden", true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            freeLookCam.m_TurnSpeed = 0f;
            inputController.DisableInputs = true;

            player.GetComponent<RPGCharacterControllerFREE>().Death();
            deathPanel.SetBool("isHidden", false);
        }
        EnemyHealthBarDisplayUpdate();

    }

    private void StatusPanelUpdate()
    {
        statusText.text = string.Format(
            "Health: {0:#}/{1}\n" +
            "Stamina: {2:#}/{3}\n" +
            "Magic: {4:#}/{5}\n" +
            "Attack: {6}\n" +
            "Defense: {7}\n" +
            "Magic Attack: {8}\n" +
            "Magic Defense: {9}\n" +
            "Affinity: {10:P0}\n" +
            "Move Speed: {11:P0}\n" +
            "Attack Speed: {12:P0}",
            playerStatus.HP * playerStatus.maxHP, playerStatus.maxHP,
            playerStatus.SP * playerStatus.maxSP, playerStatus.maxSP,
            playerStatus.MP * playerStatus.maxMP, playerStatus.maxMP,
            playerStatus.attack, playerStatus.defense,
            playerStatus.magicAttack, playerStatus.magicDefense,
            playerStatus.affinity,
            playerStatus.moveSpeed,
            playerStatus.attackSpeed);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    void OnDrawGizmos() // SphereCast visualization of EnemyHealthBarDisplayUpdate
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.transform.position, EnemyHealthBarDisplayRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, EnemyHealthBarDisplayRadius + 1f);
    }

    private void EnemyHealthBarDisplayUpdate()
    {
        RaycastHit[] enterHits = Physics.SphereCastAll(player.transform.position, EnemyHealthBarDisplayRadius,
                                                  player.transform.forward, 0f, enemyMask);
        RaycastHit[] exitHits = Physics.SphereCastAll(player.transform.position, EnemyHealthBarDisplayRadius + 1f,
                                                  player.transform.forward, 0f, enemyMask);
        foreach (RaycastHit rch in enterHits)
        {
            Enemy e = rch.transform.gameObject.GetComponent<Enemy>();
            if (e == null) Debug.LogError(rch.transform.name + "has no Enemy Component");
            else if (!e.displayHealthBar)
            {
                ;
                e.displayHealthBar = true;
            }
        }
        foreach (RaycastHit rch in exitHits)
        {
            Enemy e = rch.transform.gameObject.GetComponent<Enemy>();
            if (e == null) Debug.LogError(rch.transform.name + "has no Enemy Component");
            else if (!enterHits.Contains<RaycastHit>(rch) && e.displayHealthBar)
            {
                e.displayHealthBar = false;
            }
        }
    }

    public void ToggleSettingsPanel()
    {
        if (!exitPanel.GetBool("isHidden")) ToggleExitPanel();
        settingsPanel.SetBool("isHidden", !settingsPanel.GetBool("isHidden"));
    }

    public void ToggleExitPanel()
    {
        if (!settingsPanel.GetBool("isHidden")) ToggleSettingsPanel();
        exitPanel.SetBool("isHidden", !exitPanel.GetBool("isHidden"));
    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        GameObject cgo = other.gameObject;
        if (cgo.CompareTag("Enemy")) {
            cgo.GetComponent<Enemy>().displayHealthBar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        GameObject cgo = other.gameObject;
        if (cgo.CompareTag("Enemy")) {
            cgo.GetComponent<Enemy>().displayHealthBar = false;
        }
    }*/
}
