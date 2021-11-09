using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public SaveFile so;
    public InputManager inputMan;
    public AudioSource seSource;
    public AudioSource musicSource;

    public Image story1;
    public Image story2;

    public Text storyText;

    private List<string> write_queue;
    private List<string> image_queue;
    public List<string> dialogueText;
    public float scroll_speed;
    private bool active;
    private bool writing;
    private bool ender = false;

    // Start is called before the first frame update
    void Start()
    {
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        AudioSource[] tmp = GameObject.FindObjectsOfType<AudioSource>();
        seSource = tmp[0];
        musicSource = tmp[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMan.inputSubmit_D)
        {
            SaveManager.Save(so);
        }
        if (inputMan.inputCancel_D)
        {
            so = SaveManager.Load();
        }
    }
}
