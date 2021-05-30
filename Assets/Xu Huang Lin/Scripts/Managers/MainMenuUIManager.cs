using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public string FirstScene;
    public Animator ClosePanelButton;
    public Animator LoadGamePanel;
    public Animator SettingsPanel;
    public Animator CreditsPanel;
    public Animator ExitPanel;
    Animator CurrentOpenedPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(FirstScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ClosePanel()
    {
        CurrentOpenedPanel.SetBool("isHidden", true);
        ClosePanelButton.SetBool("isHidden", true);
    }

    public void OpenLoadGamePanel()
    {
        OpenPanel(LoadGamePanel);
    }

    public void OpenSettingsPanel()
    {
        OpenPanel(SettingsPanel);
    }

    public void OpenCreditsPanel()
    {
        OpenPanel(CreditsPanel);
    }

    public void OpenExitPanel()
    {
        OpenPanel(ExitPanel);
    }

    void OpenPanel(Animator Panel)
    {
        CurrentOpenedPanel = Panel;
        Panel.SetBool("isHidden", false);
        ClosePanelButton.SetBool("isHidden", false);
    }


}
