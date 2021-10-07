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

    public void sceneSwitch(int index)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
