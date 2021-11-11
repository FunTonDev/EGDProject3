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

    private List<string> storeText;
    private List<string> write_queue;
    private List<string> image_queue;
    private List<string> left_queue;
    private List<string> right_queue;
    private List<string> center_queue;
    private List<string> upperLeft_queue;
    private List<string> upperRight_queue;
    public float scroll_speed;
    private bool active;
    private bool writing;
    private bool ender = false;

    // Start is called before the first frame update
    void Start()
    {
        storeText = new List<string>();
        write_queue = new List<string>();
        image_queue = new List<string>();
        left_queue = new List<string>();
        right_queue = new List<string>();
        center_queue = new List<string>();
        upperLeft_queue = new List<string>();
        upperRight_queue = new List<string>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        AudioSource[] tmp = GameObject.FindObjectsOfType<AudioSource>();
        seSource = tmp[0];
        musicSource = tmp[1];
        so = SaveManager.Load();
        switch (so.currentGenre)
        {
            case 0:         //Hub events
                if (!so.gameStart)
                {
                    //Conveyor Belt Scene (4)
                    storeText.Add("Ever wonder how Pixels are made?");
                    storeText.Add("Each one is made to serve a purpose");
                    storeText.Add("However, sometimes things don’t always go according to plan...");
                    storeText.Add("Especially when things get...Interesting...");

                    //Pixal Meets Coder (3)
                    storeText.Add("Egads! What was that noise?");
                    storeText.Add("Ahh I see, I wonder how that managed to happen. Maybe a context switch gone astray…");
                    storeText.Add("No matter, I had debugging scheduled for today anyway.");

                    //Pixal has goop hat by this point (1)
                    storeText.Add("Ack! Good grief, remove that corrupted goop from yourself this instant!");
                    //Pixal loses hat
                    //Pixal learns stuff (8)
                    storeText.Add("My my, you weren’t even phased. Hold on a moment...maybe this is the solution I have been looking for all along!");
                    storeText.Add("Ugh, where are my manners.");
                    storeText.Add("*Ahem*");
                    storeText.Add("Hello tiny pixel, I am the great C0D3R, the game manager responsible for making sure all the interconnected processes run smoothly across all the current games hosted here.");
                    storeText.Add("However, recently these processes have been getting interrupted more and more, and my hypothesis is that this strange goop is the culprit.");
                    storeText.Add("But you… you seem to be unaffected by it.");
                    storeText.Add("Hmm, that’s interesting, I can’t seem to get a read on you. Regardless, something must be done as this goop is everywhere. Please, I need your help in getting rid of this virus.");
                    storeText.Add("Ahh, thank you so much uhhh...Pixal! Ah yes, that’s right, that is your name. So, let us get started then...");
                    //Coder displays map on screen (7)
                    storeText.Add("What you are looking at is a map of the games I manage, and as you can see here, these genres have had a high concentration of the virus.");
                    storeText.Add("Unfortunately, the map hasn’t been elucidated since these areas have already been contaminated, so you will be walking among uncharted territories.");
                    storeText.Add("Your job is to go into these areas and exterminate the virus. Be warned, each area is different so you should know what you’re getting yourself into.");
                    storeText.Add("Here we have the RPG, go there if you want a fantastical journey and meet some friends.");
                    storeText.Add("The Shooter is where bullet hell mayhem will ensue.");
                    storeText.Add("Finally, the platformer where a queen must be saved.");
                    storeText.Add("So, have at it and take your pick.");
                    //Screen fades to black, back to HUB world

                    for (int i = 0; i < 4; i++)
                    {
                        right_queue.Add(null);
                        upperRight_queue.Add(null);

                    }
                    so.gameStart = true;
                    SaveManager.Save(so);
                }
                break;
            case 1:         //Platformer events
                if (!so.platStart)
                {
                    //Pixal finds castle wall, jumps over it using a platform into the castle
                    //No dialogue
                }
                else if (!so.inPlat)
                { 

                    storeText.Add("“Hey you! Uhh, do you think you can help me out here? Just uhh, jump and get me, I know you can do it! I believe in you!”");
                    //Pixal then air dashes and hits the cage Cecilia is in. As the cage breaks, she falls down and sticks the landing, thus talking to Pixal-
                    storeText.Add("Thank you so much, when the queen said ‘hang out’, I thought she meant something different.");
                    storeText.Add("Huh, looks like you haven’t been assigned yet, what is your name by the way?");
                    storeText.Add("Well Pixal, thank you for saving my rump, much appreciated.");
                    storeText.Add("Man, seeing the way you look reminds me of the good ol’ factory days where we all would just bum around, waiting for our calling...");
                    storeText.Add("Thankfully though, I feel like I became who I was meant to be: a Jokester whose job it was to make people smile and protect those I love.");
                    storeText.Add("What I will say though is that it is hard sometimes to always put up a positive front, even when sometimes deep down I do get sad,");
                    storeText.Add("but seeing those smiles always warms my heart and reminds why I do what I do.");
                    storeText.Add("Lately however, the queen ain’t doing much smiling, and that don’t make me smile.");
                    //Pixal shows Cecilia the glitch goop (either as hat or in central area
                    storeText.Add("Now wait a minute, that’s the same stuff the queen got on her crown, maybe thats why she’s been acting up lately. Please save her, she’s that way!");
                    //Pixal goes off to save the queen (moves right off screen
                }
                else
                {

                }
                break;
            case 2:         //Shooter events
                if (!so.shotStart)
                {

                }
                else
                {

                }
                break;
            case 3:         //RPG events
                if (!so.rpgStart)
                {

                }
                else
                {

                }
                break;
        }

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
