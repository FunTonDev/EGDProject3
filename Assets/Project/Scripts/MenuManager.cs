using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void AudioTrigger(AudioClip tClip)
    {
        menuSource.PlayOneShot(tClip);
    }

    public void MenuChoice(int index)
    {
        switch (index)
        {
            case -1:
                Application.Quit();
                break;
            case 0:
                SetActiveMenuPanel(States.MenuSection.Main);
                break;
            case 1:
                SetActiveMenuPanel(States.MenuSection.Help);
                break;
            case 2:
                SetActiveMenuPanel(States.MenuSection.Quit);
                break;
        }
    }

    public void SetActiveMenuPanel(States.MenuSection tSection)
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }
        panels[(int)tSection].SetActive(true);
    }
}

