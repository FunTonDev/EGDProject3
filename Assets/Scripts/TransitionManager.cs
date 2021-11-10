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

    public int toSwitch;
    public bool isCompleted = false;

    public void MouseAudioTrigger(AudioClip tClip)
    {
        navSource.PlayOneShot(tClip);
    }

    public void SceneSwitch(int index)
    {
        switch (index)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }

    }

    public void SceneSwitch(string sceneStr)
    {
        Debug.Log("Go to next scene");
        string sceneText = "Loading ";
        switch (sceneText)
        {
            case "HubWorld":
                sceneText += "Central Hub";
                break;
            case "RPGWorld":
                sceneText += "Fantasy Plains";
                break;
            case "ShooterWorld":
                sceneText += "Shooter Jungle";
                break;
            case "PlatformerWorld":
                sceneText += "Platformer Kingdom";
                break;
            case "MenuScene":
                sceneText = "Leaving the System";
                break;
        }

        loadingGroup.transform.GetChild(1).GetComponent<Text>().text = sceneText;
        loadingGroup.SetActive(true);
        StartCoroutine(TextUpdater(sceneText));
        SceneManager.LoadScene(sceneStr);
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
