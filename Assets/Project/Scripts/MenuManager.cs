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

    [Header("Button Input")]
    [SerializeField] public float inputX;      //A-D
    [SerializeField] public float inputY;      //W-S
    [SerializeField] public float inputSubmit; //Enter
    [SerializeField] public float inputCancel; //Escape
    [SerializeField] public float inputAlt1;   //Space
    [SerializeField] public float inputAlt2;   //Tab
    [SerializeField] public float inputAlt3;   //Shift
    [SerializeField] public float inputAlt4;   //Ctrl
    [SerializeField] public bool inputButton;  //Check if button pressed

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    public void Start()
    {
        SetActiveMenuPanel(States.MenuSection.Main);
        mainIndex = 1;
    }

    private void Update()
    {
        ButtonUpdate();
        if (currentSection == States.MenuSection.Main)
        {
            panels[0].transform.GetChild(mainIndex-1).GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f);
            for (int i = 0; i < panels[0].transform.childCount-1; i++)
            {
                if (i != (mainIndex-1))
                {
                    panels[0].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                }
            }
            if (!inputButton)
            {
                //Down/S
                if (inputY < 0.0f && mainIndex < 5)
                {
                    mainIndex += 1;
                    inputButton = true;
                }
                //Up/W
                else if (inputY > 0.0f && mainIndex > 1)
                {
                    mainIndex -= 1;
                    inputButton = true;
                }
                //Enter/Submit
                if (inputSubmit != 0.0f)
                {
                    MenuChoice(mainIndex);
                    inputButton = true;
                }
                if (inputCancel != 0.0f)
                {
                    SetActiveMenuPanel(States.MenuSection.Main);
                    inputButton = true;
                }
            }
        }
        else if (currentSection == States.MenuSection.Play)
        {

        }
        else if (currentSection == States.MenuSection.Options)
        {

        }
        else if (currentSection == States.MenuSection.Help)
        {

        }
        else if (currentSection == States.MenuSection.Options)
        {

        }
        else if (currentSection == States.MenuSection.Credits)
        {

        }
        else if (currentSection == States.MenuSection.Quit)
        {

        }

        inputButton = false;
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

    private void ButtonUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        inputSubmit = Input.GetAxisRaw("Submit");
        inputCancel = Input.GetAxisRaw("Cancel");
        inputAlt1 = Input.GetAxisRaw("Alt1");
        inputAlt2 = Input.GetAxisRaw("Alt2");
        inputAlt3 = Input.GetAxisRaw("Alt3");
        inputAlt4 = Input.GetAxisRaw("Alt4");
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

