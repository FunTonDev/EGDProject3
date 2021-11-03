using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public InputManager inputMan;
    public AudioSource confirmSource;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
