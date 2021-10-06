using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MenuSection { Main, Help, Quit }

public class MenuManager : MonoBehaviour
{
    public List<GameObject> panels;
    public AudioSource navSource;
    public AudioClip choiceClip;
    public AudioClip confirmClip;
    public Graphic bg;

    public void Start()
    {
        SetActiveMenuPanel(MenuSection.Main);
        //StartCoroutine(ParallaxPanLoop(bg, 12.0f));
    }

    IEnumerator ParallaxPanLoop(Graphic tGraphic, float displace = 1.0f)
    {
        float xNew, yNew;
        float xOrigin = tGraphic.rectTransform.localPosition.x;
        float yOrigin = tGraphic.rectTransform.localPosition.y;
        while (true)
        {
            float xTarget = Random.Range(xOrigin - displace, xOrigin + displace);
            float yTarget = Random.Range(yOrigin - displace, yOrigin + displace);
            float xChange = Mathf.Sign(xTarget - tGraphic.rectTransform.localPosition.x) * 0.25f;
            float yChange = Mathf.Sign(yTarget - tGraphic.rectTransform.localPosition.y) * 0.25f;

            while (Mathf.Abs(xTarget - tGraphic.rectTransform.localPosition.x) > 0.5f && Mathf.Abs(yTarget - tGraphic.rectTransform.localPosition.y) > 0.5f)
            {
                xNew = Mathf.Abs(xTarget - tGraphic.rectTransform.localPosition.x) > 0.5f ? tGraphic.rectTransform.localPosition.x + xChange : tGraphic.rectTransform.localPosition.x;
                yNew = Mathf.Abs(yTarget - tGraphic.rectTransform.localPosition.y) > 0.5f ? tGraphic.rectTransform.localPosition.y + yChange : tGraphic.rectTransform.localPosition.y;
                tGraphic.rectTransform.localPosition = new Vector3(xNew, yNew, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void MouseAudioTrigger(AudioClip tClip)
    {
        navSource.PlayOneShot(tClip);
    }

    public void MenuChoice(int index)
    {
        switch (index)
        {
            case -1:
                Application.Quit();
                break;
            case 0:
                SetActiveMenuPanel(MenuSection.Main);
                break;
            case 1:
                SetActiveMenuPanel(MenuSection.Help);
                break;
            case 2:
                SetActiveMenuPanel(MenuSection.Quit);
                break;
            case 3:
                //Transition to game
                NextGuidePage();
                break;
            case 4:
                PrevGuidePage();
                break;
        }
    }

    public void SetActiveMenuPanel(MenuSection tSection)
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }
        panels[(int)tSection].SetActive(true);
    }

    public void NextGuidePage()
    {

    }

    public void PrevGuidePage()
    {

    }
}

