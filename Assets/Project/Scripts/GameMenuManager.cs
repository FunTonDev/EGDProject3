using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameManager gameMan;

    public List<GameObject> panels;

    [Header("Variables")]
    public int pauseIndex;
    public int optionsIndex;
    public States.MenuSection currentSection;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        Cursor.visible = false;
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    public void TogglePauseMenu()
    {
        gameMan.paused = !gameMan.paused;
        Time.timeScale = gameMan.paused ? 0 : 1;
        panels[1].SetActive(gameMan.paused);
        Cursor.visible = gameMan.paused;
        pauseIndex = 2;
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
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        pauseIndex = 0;
        optionsIndex = 0;
        currentSection = 0;
    }
}
