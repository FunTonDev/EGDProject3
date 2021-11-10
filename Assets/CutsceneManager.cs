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

    public Image Left;
    public Image Right;
    public Image Center;
    public Image UpperRight;

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
        so = SaveManager.Load();
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

    //public function for clearing the text of the textbox
    public void Clear()
    {
        storyText.text = "";
    }

    //Display text
    IEnumerator textDisplay(string tt, bool stop = false)
    {
        ender = stop;
        scroll_speed = 20;
        //StopCoroutine("textDisplay");
        Clear();
        write_queue.Add(tt);
        writing = true;
        for (int i = 0; i < write_queue[0].Length && writing; i++)
        {
            if (inputMan.inputSubmit != 0.0f && stop)
            {
                Debug.Log("Should stop now");
                Clear();
                storyText.text = tt;
                write_queue.RemoveAt(0);
                writing = false;
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                break;
            }
            if (writing)
            {
                yield return new WaitForSeconds(1f / scroll_speed);
                storyText.text += write_queue[0][i];
            }
        }
        writing = false;
        if (write_queue.Count >= 1)
        {
            Debug.Log("write queue count == " + write_queue.Count);
            write_queue.RemoveAt(0);
        }
        storyText.text = tt;
        if (stop)
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(new System.Func<bool>(() => inputMan.inputSubmit != 0.0f));
        }
    }
}
