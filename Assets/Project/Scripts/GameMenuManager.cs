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
    public bool canNav;
    public bool canPause;

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
        if (inputMan.inputCancel != 0 && canPause && !gameMan.paused)
        {
            gameMan.paused = !gameMan.paused;
            TogglePauseMenu();
        }
        if (gameMan.paused)
        {
            NavUpdate();
            SubmitUpdate();
            //sectionDel();
        }
    }
    private void NavUpdate()
    {
        int navDiff = (inputMan.inputY > 0) ? 1 : 0 + ((inputMan.inputY < 0) ? -1 : 0);
        if (navDiff != 0 && canNav)
        {
            StartCoroutine(ResetNavDelay());
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
        
        /*foreach (Button b in mainButtons)
        {
            
            //b.Select();
        }*/
    }

    private void SubmitUpdate()
    {
        if (inputMan.inputSubmit == 1 && canNav)
        {
            if (currentSection == States.MenuSection.Main)
            {
                switch (mainIndex)
                {
                    case 0:
                        //Go to main menu and save the game (not sure if we're gonna use savepoints)
                        break;
                    //Options
                    case 1:
                        SwitchMenuSection(States.MenuSection.Options);
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
                        break;
                }
            }
        }
    }
    public void TogglePauseMenu()
    {
        gameMan.paused = !gameMan.paused;
        Time.timeScale = gameMan.paused ? 0 : 1;
        panels[1].SetActive(gameMan.paused);
        Cursor.visible = gameMan.paused;
        SwitchMenuSection(States.MenuSection.Main);
    }

    public void SwitchMenuSection(States.MenuSection i)
    {
        panels[(int)currentSection].SetActive(false);
        currentSection = i;
        panels[(int)currentSection].SetActive(true);
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    private IEnumerator ResetNavDelay()
    {
        canNav = false;
        yield return new WaitForSeconds(1.0f);
        canNav = true;
    }

    private IEnumerator ResetPauseDelay()
    {
        canPause = false;
        yield return new WaitForSeconds(1.0f);
        canPause = true;
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
        canNav = false;
        canPause = true;
    }
}
