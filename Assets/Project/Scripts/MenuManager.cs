using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //Remove later, reserve scene changes for TRANSITIONMANAGER

public class MenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource menuSource;
    [SerializeField] private Graphic bg;

    public List<GameObject> panels;
    public AudioClip choiceClip;
    public AudioClip confirmClip;
    

    [Header("Variables")]
    public int mainIndex;
    public int optionsIndex;
    public States.MenuSection currentSection;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    public void Start()
    {
        SetActiveMenuPanel(States.MenuSection.Main);
    }

    /*============================================================================
     * MENU METHODS
     ============================================================================*/
    public void MenuChoice(int index)
    {
        switch (index)
        {
            case -2:
                SceneManager.LoadScene("[TestScene]");
                break;
            case -1:
                Application.Quit();
                break;
            case 0:
                SetActiveMenuPanel(States.MenuSection.Main);
                break;
            case 1:
                SetActiveMenuPanel(States.MenuSection.Play);
                break;
            case 2:
                SetActiveMenuPanel(States.MenuSection.Options);
                break;
            case 3:
                SetActiveMenuPanel(States.MenuSection.Help);
                break;
            case 4:
                SetActiveMenuPanel(States.MenuSection.Credits);
                break;
            case 5:
                SetActiveMenuPanel(States.MenuSection.Quit);
                break;
        }
    }
    
    private void SetActiveMenuPanel(States.MenuSection section)
    {
        panels[(int)currentSection].SetActive(false);
        currentSection = (States.MenuSection)section;
        panels[(int)currentSection].SetActive(true);
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        mainIndex = 0;
        optionsIndex = 0;
        currentSection = States.MenuSection.Main;
    }

    public void AudioTrigger(AudioClip tClip)
    {
        menuSource.PlayOneShot(tClip);
    }
}

