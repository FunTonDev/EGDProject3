using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public SaveFile so;
    public InputManager inputMan;
    public TransitionManager tranMan;
    public AudioSource seSource;
    public AudioSource musicSource;

    public Image bg;

    public Image Left;
    public Image Right;
    public Image Center;
    public Image UpperLeft;
    public Image UpperRight;

    public Image mainImage;

    public Image fader;

    public Text storyText;

    public string nextScene;
    

    private List<string> storeText;
    private List<string> write_queue;
    private List<Sprite> image_queue;
    private List<string> bg_queue;
    private List<string> left_queue;
    private List<string> right_queue;
    private List<string> center_queue;
    private List<string> upperLeft_queue;
    private List<string> upperRight_queue;
    public float scroll_speed;
    private bool active;
    private bool writing;
    private bool ender = false;
    private bool advance = false;
    private bool trackPos = false;
    private bool theEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        storeText = new List<string>();
        write_queue = new List<string>();
        bg_queue = new List<string>();
        left_queue = new List<string>();
        right_queue = new List<string>();
        center_queue = new List<string>();
        upperLeft_queue = new List<string>();
        upperRight_queue = new List<string>();
        tranMan = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
        inputMan = GameObject.Find("[MANAGER]").GetComponent<InputManager>();
        AudioSource[] tmp = GameObject.FindObjectsOfType<AudioSource>();
        seSource = tmp[0];
        musicSource = tmp[1];
        so = SaveManager.Load();
        /*
        if (!so.gameStart)
        {
            so.currentGenre = 0;
        }
        */
        switch (so.currentGenre)
        {
            case 0:         //Hub events
                if (!so.gameStart)
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/Cutscene1-Intro"));
                    image_queue.Add(Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    List<Sprite> temp = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/Cutscene2-Intro2"));
                    Debug.Log("TempSize == " + temp.Count);
                    for (int i = 0; i < temp.Count; i++)
                    {
                        image_queue.Add(temp[i]);
                    }
                    //Conveyor Belt Scene (4)
                    storeText.Add("Ever wonder how Pixels are made?");
                    storeText.Add("Each one is made to serve a purpose");
                    storeText.Add("However, sometimes things don’t always go according to plan...");
                    storeText.Add("Especially when things get...");
                    storeText.Add("...Interesting...");
                    //Pixal Falls, screen fades to black once pixal falls off
                    //Pixal Meets Coder (3)
                    storeText.Add("");
                    storeText.Add("Hmm...this is quite troubling...");
                    storeText.Add("With the glitches spreading so quickly, it won't be long until...");
                    storeText.Add("*sigh* Well, its not like a solution is going to fall from the sky, might as well-");
                    storeText.Add("Egads! What was that noise?");
                    storeText.Add("Huh, I wonder how that managed to happen. Maybe a context switch gone astray...");
                    storeText.Add("No matter, I had debugging scheduled for today anyway.");

                    //Pixal has goop hat by this point (1)
                    storeText.Add("Ack! Good grief, remove that corrupted goop from yourself this instant!");
                    storeText.Add("*SMACK*   Reeeeeeeeeeee...");
                    storeText.Add(".....*splat*");
                    //Pixal loses hat
                    //Pixal learns stuff (8)
                    storeText.Add("My my, you weren’t even phased. Hold on a moment...maybe this is the solution I have been looking for all along!");
                    storeText.Add("Ugh, where are my manners.");
                    storeText.Add("*Ahem*");
                    storeText.Add("Hello tiny pixel, I am the great C0D3R, the game manager responsible for making sure all the interconnected processes run smoothly across all the current games hosted here.");
                    storeText.Add("However, recently these processes have been getting interrupted more and more, and my hypothesis is that this strange goop is the culprit.");
                    storeText.Add("But you… you seem to be unaffected by it.");
                    storeText.Add("Hmm, that’s interesting, I can’t seem to get a read on you. Regardless, something must be done as this goop is everywhere.");
                    storeText.Add("Please, I need your help in getting rid of this virus. Will you aid my cause?");
                    storeText.Add("Ahh, thank you so much uhhh...Pixal! Ah yes, that’s right, that is your name. So, let us get started then...");
                    storeText.Add("...");
                    //Coder displays map on screen (7)
                    for (int i = 0; i < 7; i++)
                    {
                        image_queue.Add(Resources.Load<Sprite>("Art/BGArt/egd3MAP"));
                    }
                    storeText.Add("What you are looking at is a map of the games I manage, and as you can see here, these genres have had a high concentration of the virus.");
                    storeText.Add("Unfortunately, the map hasn’t been elucidated since these areas have already been contaminated, so you will be walking among uncharted territories.");
                    storeText.Add("Your job is to go into these areas and exterminate the virus. Be warned, each area is different so you should know what you’re getting yourself into.");
                    storeText.Add("Here we have the RPG, go there if you want a fantastical journey and meet some friends.");
                    storeText.Add("The Shooter is where bullet hell mayhem will ensue.");
                    storeText.Add("Finally, the platformer where a princess must be saved.");
                    storeText.Add("So, have at it and take your pick.");
                    image_queue.Add(Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    storeText.Add("");
                    //Screen fades to black, back to HUB world
                    nextScene = "HubWorld";

                    for (int i = 0; i < 4; i++)
                    {
                        right_queue.Add(null);
                        upperRight_queue.Add(null);

                    }
                    so.gameStart = true;
                    SaveManager.Save(so);
                }
                else if (so.platDone && so.shotDone && so.rpgDone)
                {
                    storeText.Add("Eureka! Now that we have more information and the games are decontaminated, we can finally move on to exterminating the source.");
                    //A new doorway opens up, leading to forbo
                    storeText.Add("I am not sure what will happen here. Whatever it is though, I have faith it can’t hurt you, as long as you do not let it. Good luck, friend");
                    image_queue.Add(Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    image_queue.Add(Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    image_queue.Add(Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    storeText.Add("...");
                    storeText.Add("To Be Continued...");
                    nextScene = "MenuScene";
                }
                else
                {
                    nextScene = "HubWorld";
                    tranMan.SceneSwitch(nextScene);
                }
                break;
            case 1:         //Platformer events
                if (!so.platStart)
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneP1-Controls"));
                    //Pixal finds castle wall, jumps over it using a platform into the castle
                    //No dialogue
                    storeText.Add("...");
                    storeText.Add("");
                    storeText.Add("");
                    storeText.Add("*Pixal learned to jump!*");
                    nextScene = "PlatformerWorld";
                    so.platStart = true;
                }
                else if (!so.inPlat) //Cutscene-P2-Cecilia
                {   //Numbers are the corresponding scenes
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneP2-Cecilia"));
                    storeText.Add(""); //1
                    storeText.Add(""); //2
                    storeText.Add("“Hey you! Uhh, do you think you can help me out here? Just uhh, jump and get me, I know you can do it! I believe in you!”"); //3
                    storeText.Add(""); //4
                    storeText.Add(""); //5
                    storeText.Add(""); //6
                    storeText.Add(""); //7
                    storeText.Add(""); //8
                    storeText.Add(""); //9
                    storeText.Add(""); //10
                    for (int i = 0; i < 7; i++)
                    {
                        image_queue.Insert(9, image_queue[9]);
                    }
                    //Pixal then air dashes and hits the cage Cecilia is in. As the cage breaks, she falls down and sticks the landing, thus talking to Pixal-
                    storeText.Add("Thank you so much, when the princess said ‘hang out’, I thought she meant something different."); //11
                    storeText.Add("Huh, looks like you haven’t been assigned yet, what is your name by the way?"); //11
                    storeText.Add("Well Pixal, thank you for saving my rump, much appreciated."); //11
                    storeText.Add("Man, seeing the way you look reminds me of the good ol’ factory days where we all would just bum around, waiting for our calling..."); //11
                    storeText.Add("Thankfully though, I feel like I became who I was meant to be: a Jokester whose job it was to make people smile and protect those I love."); //11
                    storeText.Add("What I will say though is that it is hard sometimes to always put up a positive front, even when sometimes deep down I do get sad,"); //11
                    storeText.Add("but seeing those smiles always warms my heart and reminds why I do what I do."); //11
                    storeText.Add("Lately however, the princess ain’t doing much smiling, and that don’t make me smile."); //11
                    //Pixal shows Cecilia the glitch goop (either as hat or in central area
                    storeText.Add(""); //12
                    storeText.Add("Now wait a minute, that’s the same stuff the princess got on her crown, maybe thats why she’s been acting up lately.");// Please save her, she’s that way!"); //13
                    storeText.Add("Please save her, she’s that way!"); //14
                    storeText.Add(""); //15
                    //Pixal goes off to save the princess (moves right off screen
                    storeText.Add(""); //16
                    storeText.Add(""); //17
                    nextScene = "PlatformerWorld";
                    so.inPlat = true;
                    so.lastPosition.x += 10;
                }
                else if (!so.inPlat2) //Cutscene-P3-Princess
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneP3-Princess"));
                    for (int i = 0; i < 10; i++)
                    {
                        storeText.Add("");
                    }
                    //-Pixal enters the princesss throne-
                    //-The princess is seen crying, then she sees Pixal, and then proceeds to attack-
                    //Use all cutscenes
                    so.inPlat2 = true;
                    so.lastPosition.x -= 3;
                    nextScene = "PlatformerWorld";
                }
                else //Cutscene-P4-Final
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneP4-Final"));
                    //-Pixal eventually knocks the goop off the princesss crown. The princess then sits back on her rump and begins to cry while pointing at the goop-
                    storeText.Add("I want my daddy!!!"); //1
                    //-Pixal then approaches the goop and purifies the data- //2-3
                    storeText.Add("");
                    storeText.Add("");
                    //Black Screen //4
                    for (int i = 0; i < 6; i++)
                    {
                        image_queue.Insert(3, Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    }
                    storeText.Add("Daddy is very nice");
                    storeText.Add("Daddy would play castle with me");
                    storeText.Add("Daddy would call me his princess");
                    storeText.Add("Daddy started making more money");
                    storeText.Add("Daddy disappeared all day");
                    storeText.Add("He never plays anymore");
                    storeText.Add("I miss my daddy");
                    //
                    //-Seeing the data, Pixal then makes a new crown that the Queen can wear.Offering it to her, she takes it, then disappears, leaving Pixal be, where it can return back to the hub -
                    storeText.Add("");
                    storeText.Add("");
                    storeText.Add("");
                    storeText.Add("");
                    //5-9
                    //BlackScreen //10
                    storeText.Add("");// -Pixal Shows Coder the data from the goop he purified- //11  
                    storeText.Add("I see, so the file was vulnerable for that reason. Well, I know what to fix for the next patch update. Good work.");//12
                    storeText.Add("");// End Platformer section //13
                    so.platDone = true;
                    nextScene = "HubWorld";
                    if (so.shotDone && so.rpgDone)
                    {
                        theEnd = true;
                    }
                    trackPos = true;
                }
                break;
            case 2:         //Shooter events
                if (!so.shotStart) //CutsceneS1-Start
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneS1-Start"));
                    /*
                     * -Scene: Pixal enters the jungle level, where he is trapped in a cage (or Bear Trap) by the main soldier guy of the level that 
                     * Pixal will save. After capturing Pixal, the main soldier guy appears out of the bushes-
                     * */
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("Well well well, look what we have here, a Pixel. I wonder if you’re with em"); //3
                    //-MS then whips out a piece of glitch goop-
                    storeText.Add("We’ll put it to the test then..."); //4
                    storeText.Add(""); //Soldier puts goop on pixal //5
                    storeText.Add("Well I'll be a son of a bitch, looks like ye ain’t affected by this slop, ya should have said somethin sooner!"); //6
                    storeText.Add(""); //-The soldier then releases Pixal from the trap- //7
                    storeText.Add("Thank goodness they sent in reinforcements, although I’m not sure why they sent in a plain ol’ pixel to help with this crap we’re dealing with."); //8
                    storeText.Add("There has been a damn trojan infestation here and its been messin things up, but then that thing... that damn thing... It morphed with it!"); //9
                    storeText.Add("And now it’s out here shootin the shit and making everything worse! At the rate things are goin, we might even have to go offli-"); //10
                    //Sudenly, the monster MS was talking about appears out of nowhere and then takes away MS. As MS is being dragged off, he drops his gun. Pixal picks up the dropped gun and then follows the trail of the monster-
                    storeText.Add("");//11
                    storeText.Add("");//12
                    storeText.Add("");//13
                    storeText.Add("");//14
                    storeText.Add("");//15
                    storeText.Add("'Obtained GUN'");//16
                    storeText.Add("");//17
                    storeText.Add("");//18
                    nextScene = "ShooterWorld";
                    nextScene = "ShooterWorld";
                    so.shotStart = true;
                }
                else if (!so.inShot) //CutsceneS2-Soldier
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneS2-Soldier"));
                    //storeText.Add(""); 
                    //Normal soldier talking
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("Run, run while ya still can! It may be too late for me, but not you.");//3
                    storeText.Add("You shouldn’t even be here, you...you pixel?");//4
                    storeText.Add("Why the hell would the task force send something that hasn’t even been assigned to a game yet?!");//5
                    //Injured soldier talking
                    storeText.Add("You lucky son of a bitch! I wish I was never assigned! Do you think I like being a soldier in a shooting game?!");//6
                    storeText.Add("Do you know what it’s like to die in combat everyday just for someone else’s enjoyment?!");//6
                    storeText.Add("Fuck, this thing didn’t even finish me off! Kill me please!!!");//6
                    /*NOTE: Past three lines were all Img06*/
                    storeText.Add(""); //Pixal brings out glitch goop //7
                    storeText.Add("Oh shit, yeah that thing went that way."); //8
                    //storeText.Add(""); //Soldier shows what way the glitch went
                    storeText.Add("I don’t have much time left, so please take this from me, I know you will make good use of it, just tell me family I love them..."); //9
                    storeText.Add("");//10
                    storeText.Add("");//11
                    storeText.Add("Just go...");//12
                    storeText.Add("");//13 // End this scene
                    nextScene = "ShooterWorld";
                    so.inShot = true;
                    so.lastPosition.z -= 2;
                }
                else if (!so.inShot1) //CutsceneS3-Horde
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneS3-Horde"));
                    //
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("");//3
                    storeText.Add("");//4
                    storeText.Add("");//5
                    storeText.Add("");//6
                    storeText.Add("");//7
                    storeText.Add("");//8
                    storeText.Add("");//9
                    nextScene = "ShooterWorld";
                    so.inShot1 = true;
                    so.lastPosition.z += 2;
                }
                //After shooter horde battle
                else //CutsceneS4-Final
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneS4-Final"));
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("");//3
                    storeText.Add("");//4
                    storeText.Add("");//5
                    storeText.Add("");//6
                    storeText.Add("");//7 //Black Screen
                    /*BLACK SCREEN IS IMG07*/
                    for (int i = 0; i < 7; i++)
                    {
                        image_queue.Insert(6, Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    }
                    storeText.Add("Alright, if I want to work in that company someday, its time to put in the grind!");
                    storeText.Add("...");
                    storeText.Add("I don’t know how dad does this, maybe he’ll have a solution, I hope he won’t mind if I login to his laptop to see how he does some pathfinding algorithms.");
                    storeText.Add("Oh man, there’s so much stuff on here... Wait, what’s this link?");
                    storeText.Add("*click*");
                    storeText.Add("SHIT, NOO!!!");
                    storeText.Add("I DIDN’T MEAN TO SEARCH FOR HOT SINGLES IN MY AREA!");
                    storeText.Add("I Need to fix this, hopefully I can get rid of the virus before before dad checks...");
                    //End of black screen
                    /*END BLACK SCREEN OF IMG07*/
                    //pixal helps up soldier guy
                    storeText.Add("");//8
                    storeText.Add("");//9
                    storeText.Add("Thanks for saving my ass Pixal, you not only helped clear the malware, but also defeated that glitch as well. For that, you have my gratitude."); //10
                    storeText.Add("");//11 //Pixal tries giving back gun
                    storeText.Add("No no, that’s yours soldier, you made better use of it than I did.");//11
                    storeText.Add("");//12 //Pixal nods and leaves
                    storeText.Add("");//13
                    storeText.Add("");//14 //Black Screen
                    storeText.Add("");// -Pixal Shows Coder the data from the goop he purified- //15  
                    storeText.Add("Excuse my language, but that sounds fucked up. I can’t believe my game was already bugged by a fake ad.");//16
                    storeText.Add("Rookie mistakes these players. Well, I know what to patch next, so keep up the good work!");// End Shooter section //17
                    so.shotDone = true;
                    if (so.platDone && so.rpgDone)
                    {
                        theEnd = true;
                    }
                    nextScene = "HubWorld";
                    trackPos = true;
                    //nextScene = "ShooterWorld";
                }
                break;
            case 3:         //RPG events
                if (!so.rpgStart) //CutsceneR1-Start
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneR1-Start"));
                    //Background = forest/plains
                    //Pixal finds trail of slime, follows it
                    //Some gameplay/movement
                    //storeText.Add("");
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("");//3
                    storeText.Add("");//4
                    storeText.Add("");//5
                    so.playerMana = 10f;
                    nextScene = "RPGWorld";
                    so.rpgStart = true;
                }
                else if (!so.inRPG) //CutsceneR2-Rogue
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneR2-Rogue"));
                    //Pixal attacked by slime, but slime is defeated by Mama Rogue (jumps out of nowhere)
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("");//3
                    storeText.Add("");//4
                    storeText.Add("Well, I haven’t seen the likes of you before, luckily I came just in time."); //5
                    storeText.Add("");//6 //Pixal thanks her and moves to leave
                    storeText.Add("Wait, don’t go yet! You and I are both on the same mission; To figure out where this pestilence comes from!");//7
                    //As she finishes her statement, she starts coughing up a liquid that looks similar to the glitch goop. Some of the goop lands on Pixal.
                    storeText.Add("");//8
                    storeText.Add("Oh heavens, I am so sorry! Hurry, we must clean before you’re infected!");//9
                    //As the woman is reaching for her things, she notices that Pixal is unaffected.-
                    storeText.Add("");//10
                    storeText.Add("Good grief, you must be some kind of panacea. Please, let me join you, I must help relieve my village from this sickness.");//11
                    //-Pixal nods and they both head toward the goop.-
                    storeText.Add("'The Motherly Rogue joined your Party'");//12
                    so.mamaGot = true;
                    so.helperGot = true;
                    so.inRPG = true;
                    so.lastPosition.x -= 10;
                    nextScene = "RPGWorld";
                }
                else if (!so.inRPG2) //CutsceneR3-Meaning
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneR3-Meaning"));
                    //As they continue walking thru the forest, mama rogue stops and walks infront of Pixal to tell it something-
                    storeText.Add("");//1
                    storeText.Add("I was wondering where I’ve seen you from, and it hit me: you’re a pixel.");//2
                    storeText.Add("So, how did you escape the factory?");//3
                    storeText.Add("...");//4
                    storeText.Add("Not much of a talker I see, just like someone I used to know.");//5
                    storeText.Add("Well, I’d say you’re a lucky pixel, it’s rare when to see one loose, it’s been years.");//6
                    storeText.Add("You can become whatever you want, so choose wisely when you do.");//7
                    storeText.Add("");//8
                    storeText.Add("");//9
                    so.lastPosition.z -= 10;
                    so.inRPG2 = true;
                }
                else if (!so.inRPG3) //CutsceneR4-Boss
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneR4-Boss"));
                    storeText.Add("");//1
                    storeText.Add("We’re close");//2
                    storeText.Add("");//3
                    storeText.Add("");//4
                    storeText.Add("");//5
                    //They approach a giant blob (Setting is either in a cave or deep in the forest). The giant blob looks at Pixal and the woman and then attacks.
                    storeText.Add("Here we go!");//6
                    nextScene = "RPGBattle";
                    so.bossFight = true;
                    so.inRPG3 = true;
                }
                else //CutsceneR5-Final
                {
                    image_queue = new List<Sprite>(Resources.LoadAll<Sprite>("CutsceneAssets/Game_Select_Images/CutsceneR5-Final"));
                    //The glitch is defeated and is deflated. The goop then falls off from the shape of a heart shaped flask.-
                    storeText.Add("");//1
                    storeText.Add("");//2
                    storeText.Add("");//3
                    storeText.Add("The Panacea... We did it...");//4
                    //She opens it up and takes a swig. The woman then starts to heal. As the woman is healing, Pixal grabs the goop and then starts to purify and absorb the data.-
                    /*SCENE 5*/
                    for (int i = 0; i < 15; i++)
                    {
                        image_queue.Insert(4, Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
                    }
                    storeText.Add("Do you take this man to be your lawfully wedded husband?");
                    storeText.Add("I do");
                    storeText.Add("Do you take this woman to be your lawfully wedded wife?");
                    storeText.Add("I do");
                    storeText.Add("Then you may kiss the bride.");
                    storeText.Add("I love you.");
                    storeText.Add("*Several years later...*");
                    storeText.Add("Are we still going out for dinner tonight?");
                    storeText.Add("I can’t, the game is close to release and there’s still much to be done. My boss has been telling me there’s a breach in the system and it’s my job to fix it.");
                    storeText.Add("Ugh, you’re always working, when was the last time you even checked in on the kids?");
                    storeText.Add("Listen, I understand but-");
                    storeText.Add("But nothing! ANTHONY hasn’t even left his room in weeks! And you don’t seem to care.");
                    storeText.Add("It’s like you don’t even love us!");
                    storeText.Add("You know that’s not true, what I do is for you guys...");
                    storeText.Add("Shut up! You do this for yourself! Your precious game is SO IMPORTANT, I get it, now get out of my face! *coughs*");
                    //End Black Screen & Scene 5
                    //As Pixal is leaving the forest, he looks behind and notices the woman isn’t following him. It sees that the woman is collapsed over the panacea, holding it dearly.-
                    storeText.Add("");//6
                    storeText.Add("");//7
                    storeText.Add("");//8
                    storeText.Add("Here, go on ahead and deliver it to my people");//9
                    //Pixal is worried (show expression)
                    storeText.Add("");//10
                    storeText.Add("");//11
                    storeText.Add("Don’t worry about me... I'll be fine, I just need to... rest this weary body for a bit.");//12
                    storeText.Add("");//13
                    storeText.Add("The name is Barbara by the way");//14
                    storeText.Add("");//15
                    storeText.Add("");//16
                    storeText.Add("");//17 //Black Screen to Hub world
                    storeText.Add("");//18
                    storeText.Add("Well, be glad that at least the sacrifice was not in vain. I know it hurts, I feel it too.");//19
                    storeText.Add("That woman was named after someone special to me...");//20
                    //Fade to black
                    so.rpgDone = true;
                    if (so.platDone && so.shotDone)
                    {
                        theEnd = true;
                    }
                    nextScene = "HubWorld";
                    trackPos = true;
                }
                break;
        }
        if (storeText == null)
        {
            storeText = new List<string>();
            storeText.Add("");
        }
        if (image_queue == null)
        {
            image_queue = new List<Sprite>();
            image_queue.Add(Resources.Load<Sprite>("CutsceneAssets/Game_Select_Images/BlackScreen"));
        }
        StartCoroutine(displayAllText(storeText));

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (inputMan.inputSubmit_D)
        {
            SaveManager.Save(so);
        }
        if (inputMan.inputCancel_D)
        {
            so = SaveManager.Load();
        }
        */
    }

    //public function for clearing the text of the textbox
    public void Clear()
    {
        storyText.text = "";
    }

    public void AdvanceButton()
    {
        advance = true;
    }

    public IEnumerator displayAllText(List<string> tts)
    {
        Debug.Log("Images - " + image_queue.Count);
        Debug.Log("Text - " + tts.Count);
        for (int i = 0; i < tts.Count; i++)
        {
            Debug.Log("i == " + i + ", image == " + image_queue[i]);

            bg.sprite = image_queue[i];
            yield return textDisplay(tts[i], true);
            advance = false;
        }
        SaveManager.Save(so);
        Debug.Log("RStart - " + so.rpgStart);
        Debug.Log("Next current scene == " + nextScene);
        Debug.Log("The end -- " + theEnd);
        if (nextScene.Equals("HubWorld") && theEnd)
        {
            Debug.Log("THis shouldn't activate");
            nextScene = "CutsceneScene";
        }
        Debug.Log("Next scene == " + nextScene);
        tranMan.SceneSwitch(nextScene);
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
            if ((inputMan.inputSubmit != 0.0f || advance)  && stop && !advance)
            {
                Debug.Log("Should stop now");
                Clear();
                storyText.text = tt;
                write_queue.RemoveAt(0);
                writing = false;
                advance = false;
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
