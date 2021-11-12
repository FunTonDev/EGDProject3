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
                    //Pixal Falls, screen fades to black once pixal falls off

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
                    storeText.Add("Finally, the platformer where a princess must be saved.");
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
                    storeText.Add("Thank you so much, when the princess said ‘hang out’, I thought she meant something different.");
                    storeText.Add("Huh, looks like you haven’t been assigned yet, what is your name by the way?");
                    storeText.Add("Well Pixal, thank you for saving my rump, much appreciated.");
                    storeText.Add("Man, seeing the way you look reminds me of the good ol’ factory days where we all would just bum around, waiting for our calling...");
                    storeText.Add("Thankfully though, I feel like I became who I was meant to be: a Jokester whose job it was to make people smile and protect those I love.");
                    storeText.Add("What I will say though is that it is hard sometimes to always put up a positive front, even when sometimes deep down I do get sad,");
                    storeText.Add("but seeing those smiles always warms my heart and reminds why I do what I do.");
                    storeText.Add("Lately however, the princess ain’t doing much smiling, and that don’t make me smile.");
                    //Pixal shows Cecilia the glitch goop (either as hat or in central area
                    storeText.Add("Now wait a minute, that’s the same stuff the princess got on her crown, maybe thats why she’s been acting up lately. Please save her, she’s that way!");
                    //Pixal goes off to save the princess (moves right off screen
                    storeText.Add("");
                }
                else if (!so.inPlat2)
                {
                    //-Pixal enters the princesss throne-
                    //-The princess is seen crying, then she sees Pixal, and then proceeds to attack-
                }
                else
                {
                    //-Pixal eventually knocks the goop off the princesss crown. The princess then sits back on her rump and begins to cry while pointing at the goop-
                    storeText.Add("I want my daddy!!!");
                    //-Pixal then approaches the goop and purifies the data-
                    //Black Screen
                    storeText.Add("Daddy is very nice");
                    storeText.Add("Daddy would play castle with me");
                    storeText.Add("Daddy would call me his princess");
                    storeText.Add("Daddy started making more money");
                    storeText.Add("Daddy disappeared all day");
                    storeText.Add("He never plays anymore");
                    storeText.Add("I miss my daddy");
                    //
                    //-Seeing the data, Pixal then makes a new crown that the Queen can wear.Offering it to her, she takes it, then disappears, leaving Pixal be, where it can return back to the hub -
                }
                break;
            case 2:         //Shooter events
                if (!so.shotStart)
                {
                    /*
                     * -Scene: Pixal enters the jungle level, where he is trapped in a cage (or Bear Trap) by the main soldier guy of the level that 
                     * Pixal will save. After capturing Pixal, the main soldier guy appears out of the bushes-
                     * */
                    storeText.Add("Well well well, look what we have here, a Pixel. I wonder if you’re with em");
                    //-MS then whips out a piece of glitch goop-
                    storeText.Add("We’ll put it to the test then...");
                    //Soldier puts goop on pixal
                    storeText.Add("Well son of a Gun, looks like ye ain’t affected by this slop, ya should have said somethin sooner!");
                    //-The soldier then releases Pixal from the trap-
                    storeText.Add("Thank goodness they sent in reinforcements, although I’m not sure why they sent in a plain ol’ pixel to help with this crap we’re dealing with.");
                    storeText.Add("There has been a damn trojan infestation here and its been messin things up, but then that thing... that damn thing... It morphed with it!");
                    storeText.Add("And now it’s out here shootin the shit and making everything worse! At the rate things are goin, we might even have to go offli-");
                    //Sudenly, the monster MS was talking about appears out of nowhere and then takes away MS. As MS is being dragged off, he drops his gun. Pixal picks up the dropped gun and then follows the trail of the monster-
                    storeText.Add("'Obtained GUN'");
                }
                //After shooter horde battle
                else
                {
                    //Black Screen
                    storeText.Add("Alright, if I want to work in that company someday, its time to put in the grind!");
                    storeText.Add("...");
                    storeText.Add("I don’t know how dad does this, maybe he’ll have a solution, I hope he won’t mind if I login to his laptop to see how he does some pathfinding algorithms.");
                    storeText.Add("Oh man, there’s so much stuff on here... Wait, what’s this link?");
                    storeText.Add("*click*");
                    storeText.Add("SHIT, NOO!!!");
                    storeText.Add("I DIDN’T MEAN TO SEARCH FOR HOT SINGLES IN MY AREA!");
                    storeText.Add("I Need to fix this, hopefully I can get rid of the virus before before dad checks...");
                    //End of black screen, pixal helps up soldier guy
                    storeText.Add("Thanks for saving my ass Pixal, you not only helped clear the malware, but also defeated that glitch as well. For that, you have my gratitude.");
                    //Pixal tries giving back gun
                    storeText.Add("No no, that’s yours soldier, you made better use of it than I did.");
                }
                break;
            case 3:         //RPG events
                if (!so.rpgStart)
                {

                }
                else if (!so.inRPG)
                {

                }
                else
                {

                }
                break;
        }
        StartCoroutine(displayAllText(storeText));

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

    public IEnumerator displayAllText(List<string> tts)
    {
        for (int i = 0; i < tts.Count; i++)
        {
            yield return textDisplay(tts[i]);
        }
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
