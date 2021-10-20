using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameManager gameMan;
    [SerializeField] private InputManager inputMan;
    public List<GameObject> panels;
    public List<Button> mainButtons;
    public List<Button> optionButtons;

    [Header("Variables")]
    public int mainIndex;
    public int optionsIndex;

    public States.MenuSection currentSection;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        Cursor.visible = false;
    }

    private void Update()
    {
        MenuUpdate();
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void MenuUpdate()
    {
        if (gameMan.paused)
        {
            if (inputMan.inputY_D) { NavUpdate(); }
            if (inputMan.inputSubmit_D) { SubmitUpdate(); }
        }
        else { if (inputMan.inputCancel_D) { TogglePauseMenu(); } }
    }

    private void NavUpdate()
    {
        int navDiff = (inputMan.inputY > 0) ? -1 : 0 + ((inputMan.inputY < 0) ? 1 : 0);
        if (currentSection == States.MenuSection.Main)
        {
            mainIndex += ((navDiff < 0 && mainIndex > 0) || (navDiff > 0 && mainIndex < mainButtons.Count - 1)) ? navDiff : 0;
            mainButtons[mainIndex].Select();
        }
        else
        {
            optionsIndex += ((navDiff < 0 && optionsIndex > 0) || (navDiff > 0 && optionsIndex < mainButtons.Count - 1)) ? navDiff : 0;
            optionButtons[optionsIndex].Select();
        }
    }

    private void SubmitUpdate()
    {
        if (currentSection == States.MenuSection.Main)
        {
            mainButtons[mainIndex].onClick.Invoke();
            switch (mainIndex)
            {
                case 0:     //Go to main menu and save the game (not sure if we're gonna use savepoints)
                    break;
                case 1:
                    SwitchUISection(States.MenuSection.Options);
                    break;
                case 2:
                    TogglePauseMenu();
                    break;
                }
            }
        else if (currentSection == States.MenuSection.Options)
        {
            switch (optionsIndex)
            {
                case 0:
                    SwitchUISection(States.MenuSection.Main);
                    break;
            }
        }
    }

    public void TogglePauseMenu()
    {
        gameMan.paused = !gameMan.paused;
        Time.timeScale = gameMan.paused ? 0 : 1;
        Cursor.visible = gameMan.paused;
        if (gameMan.paused)
        {
            SwitchUISection(States.MenuSection.Main);
            mainButtons[mainIndex].Select();
        }
        else { SwitchUISection(States.MenuSection.Play); }
    }

    public void SwitchUISection(States.MenuSection i)
    {
        switch (currentSection)
        {
            case States.MenuSection.Play:
                panels[0].SetActive(false);
                break;
            case States.MenuSection.Main:
                panels[1].SetActive(false);
                break;
            case States.MenuSection.Options:
                panels[2].SetActive(false);
                break;
        }
        currentSection = i;
        switch (currentSection)
        {
            case States.MenuSection.Play:
                panels[0].SetActive(true);
                break;
            case States.MenuSection.Main:
                panels[1].SetActive(true);
                break;
            case States.MenuSection.Options:
                panels[2].SetActive(true);
                break;
        }
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
}
