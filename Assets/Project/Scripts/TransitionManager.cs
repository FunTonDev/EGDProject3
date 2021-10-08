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

    public int toSwitch;


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
        SceneManager.LoadScene(sceneStr);
    }
}
