using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public AudioSource navSource;
    public AudioClip choiceClip;
    public AudioClip confirmClip;

    public GameObject loadingGroup;
    public List<Image> stageIcons;

    SaveFile so;

    public int toSwitch;
    public bool isCompleted = false;

    public void Start()
    {
        so = SaveManager.Load();
    }

    public void MouseAudioTrigger(AudioClip tClip)
    {
        navSource.PlayOneShot(tClip);
    }

    public void SceneSwitch(string sceneStr, bool res = false)
    {
        so = SaveManager.Load();
        if (res)
        {
            so.lastPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        Debug.Log("Go to next scene -- " + sceneStr);
        string sceneText = "Loading ";
        switch (sceneStr)
        {
            case "HubWorld":
                sceneText += "Central Hub";
                so.currentGenre = 0;
                if (!so.gameStart)
                {
                    sceneStr = "CutsceneScene";
                }
                break;
            case "RPGWorld":
                sceneText += "Fantasy Plains";
                so.currentGenre = 3;
                if (!so.rpgStart)
                {   
                    sceneStr = "CutsceneScene";
                }
                break;
            case "RPGBattle":
                sceneText += "Into Battle";
                so.currentGenre = 3;
                break;
            case "ShooterWorld":
                sceneText += "Shooter Jungle";
                so.currentGenre = 2;
                if (!so.shotStart)
                {
                    sceneStr = "CutsceneScene";
                }
                break;
            case "PlatformerWorld":
                sceneText += "Platformer Kingdom";
                so.currentGenre = 1;
                if (!so.platStart)
                {
                    sceneStr = "CutsceneScene";
                } 
                break;
            case "MenuScene":
                sceneText = "Leaving the System";
                break;
        }
        Debug.Log("Next scene = " + sceneStr);
        loadingGroup.transform.GetChild(1).GetComponent<Text>().text = sceneText;
        loadingGroup.SetActive(true);
        StartCoroutine(TextUpdater(sceneText));
        SaveManager.Save(so);
        SceneManager.LoadSceneAsync(sceneStr);
    }
    public void SceneSwitch(string sceneStr)
    {
        so = SaveManager.Load();

        Debug.Log("Go to next scene -- " + sceneStr);
        string sceneText = "Loading ";
        int ori = so.currentGenre;
        switch (sceneStr)
        {
            case "HubWorld":
                sceneText += "Central Hub";
                so.currentGenre = 0;
                if (!so.gameStart)
                {
                    sceneStr = "CutsceneScene";
                }
                break;
            case "RPGWorld":
                sceneText += "Fantasy Plains";
                so.currentGenre = 3;
                if (!so.rpgStart)
                {
                    sceneStr = "CutsceneScene";
                }
                break;
            case "RPGBattle":
                sceneText += "Into Battle";
                so.currentGenre = 3;
                break;
            case "ShooterWorld":
                sceneText += "Shooter Jungle";
                so.currentGenre = 2;
                if (!so.shotStart)
                {
                    sceneStr = "CutsceneScene";
                }
                break;
            case "PlatformerWorld":
                sceneText += "Platformer Kingdom";
                so.currentGenre = 1;
                if (!so.platStart)
                {
                    sceneStr = "CutsceneScene";
                }
                break;
            case "MenuScene":
                sceneText = "Leaving the System";
                break;
        }
        if (ori != so.currentGenre)
        {
            so.lastPosition = new Vector3(0, 0, 0);
        }
        Debug.Log("Next scene = " + sceneStr);
        loadingGroup.transform.GetChild(1).GetComponent<Text>().text = sceneText;
        loadingGroup.SetActive(true);
        StartCoroutine(TextUpdater(sceneText));
        SaveManager.Save(so);
        SceneManager.LoadSceneAsync(sceneStr);
    }

    public IEnumerator TextUpdater(string ori)
    {
        int initLen = ori.Length;
        while (!isCompleted)
        {
            if (loadingGroup.transform.GetChild(1).GetComponent<Text>().text.Length >= ori.Length + 3)
            {
                loadingGroup.transform.GetChild(1).GetComponent<Text>().text = loadingGroup.transform.GetChild(1).GetComponent<Text>().text.Remove(ori.Length);
            }
            else
            {
                loadingGroup.transform.GetChild(1).GetComponent<Text>().text += ".";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
