using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Different states of battle (turns)
public enum battleState { START, PLAYER, ATTACK, ENEMY, WIN, LOSE, FLEE, HUH }

public class actionTag
{
    public actionTag()
    {
        id = 0;
        type = "none";
        index = 0;
        target = 0;
        speed = 0;
        priority = false;
    }
    //who == index of the user
    //todo == type of action
    public actionTag(int who, string todo, int what, int where, int agi, bool p = false)
    {
        id = who;
        type = todo;
        index = what;
        target = where;
        speed = agi;
        priority = p;
    }
    public int getID() { return id; }
    public string getType() { return type; }
    public int getIndex() { return index; }
    public int getTarget() { return target; }
    public int getSPD() { return speed; }
    public bool getFast() { return priority; }

    int id = 0;                         //Index (who is doing the action)
    string type;                        //String (represents what the action is)
    int index = 0;                      //Numerical index of the ability/some other value
    int target = 0;                     //Index (target of any effects the action has)
    int speed = 0;                      //Speed at which the action happens
    bool priority = false;              //Whether the action should happen first
}


public class BattleManager : MonoBehaviour
{
    //Use to determine state of the battle (turns, win/loss, etc.)
    public battleState state;

    public SaveFile sv;

    public InputManager inputMan;

    //List of the positions of menus
    [System.Serializable]
    public struct MenuPositions
    {
        public List<Transform> positions;
    }

    public int cursor_position;     //Current position (index) the cursor is at
    public int active_menu;         //Current menu being moved through

    //A list of positions the cursor can go through
    [SerializeField]
    public List<MenuPositions> cursor_positions;

    private List<GameObject> menus;                 //The list of menu objects

    public Text DisplayText;

    //Units that are currently in battle
    public List<Unit> PartyMembers;
    public List<Unit> EnemyMembers;

    public GameObject actionBackground;
    
    public int mainIndex;
    public int actionIndex;
    public int enemyIndex;
    public int allyIndex;

    public List<Button> mainButtons;
    public List<Button> actionButtons;
    public List<Button> enemyButtons;
    public List<Button> allyButtons;

    //Current ability being highlighted
    private int highlighted_ability;
    //Current action in base menu (index) being highlighed by cursor
    private int highlighted_action;
    //Bool to check whether the menu is accepting input
    private bool menu_input;

    public GameObject background;

    //GameObjects to use as basis for battle characters
    public List<GameObject> partyPrefabs;
    public List<Image> partyIcons;

    public List<GameObject> enemyPrefabs;
    public List<Image> enemyIcons;

    private List<actionTag> actions;

    public AudioClip navigateSound;
    public AudioClip confirmSound;
    public AudioClip music;

    public AudioSource seSource;
    public AudioSource musicSource;

    public Image fader;


    //Int to track the number of units actually in the party
    int activeUnits = 0;

    //Int to track the number of deaths in the party
    int partyDeaths = 0;

    //Int to track the number of enemies encountered in the battle
    int activeEnemies = 0;

    //Number of enemies that have died
    int enemyDeaths = 0;

    bool bossBattle = false;

    //The current unit in the party that is choosing an action
    private int currentUnit = 0;

    //Current ally being selected for using an ability on/swapping
    private int currentAlly = 0;

    //The number of moves that should be done by the party
    private int moves = 0;

    //The enemy currently being highlighted
    private int currentEnemy = 0;
    private int currentAction = 0;
    private string currentActionType = "";

    //Bool to check whether text is displayed that have button delays
    bool skipper = false;
    private List<string> write_queue;
    private List<string> image_queue;
    public List<string> dialogueText;
    public float scroll_speed;

    //Main text to let player know state of battle
    private TMP_Text dialogue;

    private bool active;
    private bool writing;
    private bool ender = false;
    
    //Play Sound effects --- 0 = Hit/Damage, 1 = Heal, 2 = Dead
    public void playSound(int num)
    {
        switch (num)
        {
            case 0:
                seSource.clip = Resources.Load<AudioClip>("Audio/Sound Effects/Hit 1");
                seSource.Play();
                break;
            case 1:
                seSource.clip = Resources.Load<AudioClip>("Audio/Sound Effects/healspell1");
                seSource.Play();
                break;
            case 2:
                seSource.clip = Resources.Load<AudioClip>("Audio/Sound Effects/whoosh1");
                seSource.Play();
                break;
        }

    }

    public void playerTurn()
    {
        DisplayText.text = PartyMembers[currentUnit].unitName + "'s turn\nHP = " + PartyMembers[currentUnit].currentHP + ", MP = " + PartyMembers[currentUnit].currentStamina;
    }

    //Function to enter the target/action for an attack
    public void takeAction(int target)
    {
        if (!currentActionType.Equals(""))
        {
            if (currentActionType == "Action")
            {
                Debug.Log("Is doing action");
                if (EnemyMembers[target] != null)
                {
                    actions.Add(new actionTag(currentUnit, "Action", currentAction, target, PartyMembers[currentUnit].spd));
                }
                else
                {
                    DisplayText.text = "Can't attack an empty Space";
                }
            }
            else if (currentActionType == "Support")
            {
                Debug.Log("Is doing support");
                if (PartyMembers[target] != null)
                {
                    actions.Add(new actionTag(currentUnit, "Support", currentAction, target, PartyMembers[currentUnit].spd));
                }
                else
                {
                    DisplayText.text = "Can't support an empty Space";
                }
            }
        }
        else
        {
            actions.Add(new actionTag(currentUnit, "Attack", currentAction, target, PartyMembers[currentUnit].spd));
        }
        currentUnit += 1;
        currentActionType = "";
        currentAction = 0;
        
        if (currentUnit >= activeUnits)
        {
            state = battleState.ATTACK;
            //Calculate enemy actions, then go to perform actions
            StartCoroutine(PerformActions());
        }
        else
        {
            makeMenuVisible(0);
            mainIndex = 0;
            mainButtons[mainIndex].Select();

            playerTurn();
        }
    }

    //Function to use an action and move to select a spot
    public void startAction(int num)
    {
        if (num > PartyMembers[currentUnit].abilities.Count)
        {
            DisplayText.text = "Can't use a Locked Ability";
        }
        else
        {
            currentAction = num;
            if (PartyMembers[currentUnit].abilities[num].type == 1)
            {
                currentActionType = "Support";
                makeMenuVisible(2);
            }
            else
            {
                currentActionType = "Action";
                makeMenuVisible(1);
            }
        }
    }

    public void makeMenuVisible(int num)
    {
        //Make actions menu visible
        if (num == 0)
        {
            actionBackground.transform.GetChild(0).GetComponent<Text>().text = "Actions";
            active_menu = 1;
            menus[1].SetActive(true);
            menus[2].SetActive(false);
            menus[3].SetActive(false);
            if (PartyMembers[currentUnit].abilities.Count < 4)
            {
                actionButtons[6].gameObject.SetActive(false);
            }
            else
            {
                actionButtons[6].gameObject.SetActive(true);
                actionButtons[6].transform.GetChild(0).GetComponent<Text>().text = PartyMembers[currentUnit].abilities[3].actionName + ": " +
                    PartyMembers[currentUnit].abilities[3].cost + "MP ";
            }
            if (PartyMembers[currentUnit].abilities.Count < 3)
            {
                actionButtons[5].gameObject.SetActive(false);
            }
            else
            {
                actionButtons[5].gameObject.SetActive(true);
                actionButtons[5].transform.GetChild(0).GetComponent<Text>().text = PartyMembers[currentUnit].abilities[2].actionName + ": " +
                    PartyMembers[currentUnit].abilities[2].cost + "MP ";
            }
            if (PartyMembers[currentUnit].abilities.Count < 2)
            {
                actionButtons[4].gameObject.SetActive(false);
            }
            else
            {
                actionButtons[4].gameObject.SetActive(true);
                actionButtons[4].transform.GetChild(0).GetComponent<Text>().text = PartyMembers[currentUnit].abilities[1].actionName + ": " +
                    PartyMembers[currentUnit].abilities[1].cost + "MP ";
            }
            if (PartyMembers[currentUnit].abilities.Count < 1)
            {
                actionButtons[3].gameObject.SetActive(false);
                actionBackground.transform.GetChild(0).GetComponent<Text>().text = "No Actions Unlocked";
            }
            else
            {
                actionButtons[3].gameObject.SetActive(true);
                actionButtons[3].transform.GetChild(0).GetComponent<Text>().text = PartyMembers[currentUnit].abilities[0].actionName + ": " +
                    PartyMembers[currentUnit].abilities[0].cost + "MP ";
            }
        }
        //Make attack menu visible
        else if (num == 1)
        {
            if (currentActionType.Equals(""))
            {
                actionBackground.transform.GetChild(0).GetComponent<Text>().text = "Attack";
            }
            else
            {
                actionBackground.transform.GetChild(0).GetComponent<Text>().text = "Choose Target";
            }
            active_menu = 2;
            menus[1].SetActive(false);
            menus[2].SetActive(true);
            menus[3].SetActive(false);
            for (int i = 0; i < 3; i++)
            {
                if (EnemyMembers[i] != null)
                {
                    if (EnemyMembers[i].currentHP <= 0)
                    {
                        menus[2].transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else
                {
                    menus[2].transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        //Make support menu visible
        else if (num == 2)
        {
            if (currentActionType.Equals(""))
            { 
                actionBackground.transform.GetChild(0).GetComponent<Text>().text = "Support";
            }
            else
            {
                actionBackground.transform.GetChild(0).GetComponent<Text>().text = "Choose Target";
            }
            active_menu = 3;
            menus[1].SetActive(false);
            menus[2].SetActive(false);
            menus[3].SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                if (PartyMembers[i] != null)
                {
                    if (PartyMembers[i].currentHP <= 0)
                    {
                        menus[3].transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else
                {
                    menus[3].transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        else if (num == 3)
        {
            actions.Add(new actionTag(currentUnit, "Defend", currentAction, currentUnit, PartyMembers[currentUnit].spd));
            currentUnit += 1;
            currentActionType = "";
            currentAction = 0;
            if (currentUnit >= activeUnits)
            {
                state = battleState.ATTACK;
                //Calculate enemy actions, then go to perform actions
                StartCoroutine(PerformActions());
            }
            else
            {
                playerTurn();
            }
        }
        else
        {
            actionBackground.transform.GetChild(0).GetComponent<Text>().text = "Battling Time";
            active_menu = 0;
            menus[1].SetActive(false);
            menus[2].SetActive(false);
            menus[3].SetActive(false);
        }
    }

    //Start the enemy attack routine
    public void enemyAttacks()
    {
        int other = 0;

        for (int i = 0; i < EnemyMembers.Count; i++)
        {
            if (EnemyMembers[i] != null)
            {
                if (EnemyMembers[i].currentHP > 0)
                {
                    other += 1;
                }
            }
        }
        //For each of the enemies present
        for (int i = 0; i < EnemyMembers.Count; i++)
        {
            //If the enemy is there
            if (EnemyMembers[i] != null)
            {
                //If the enemy isn't dead
                if (EnemyMembers[i].currentHP > 0)
                {
                    bool self = false;
                    bool self2 = false;
                    //Check if the enemy has any support abilities
                    for (int j = 0; j < EnemyMembers[i].abilities.Count; j++)
                    {
                        if (EnemyMembers[i].abilities[j].type == 1 &&
                            EnemyMembers[i].abilities[j].priority > 0 &&
                            (EnemyMembers.Count - enemyDeaths) > 1 && other > 1)
                        {
                            self = true;
                        }
                        if (EnemyMembers[i].abilities[j].type == 2 &&
                            EnemyMembers[i].abilities[j].priority > 0)
                        {
                            self2 = true;
                        }
                    }
                    //Randomly choose a type of ability to do
                    List<bool> choices = new List<bool>();
                    choices.Add(true);
                    choices.Add(self);
                    choices.Add(self2);
                    int vals = Random.Range(0, 3);
                    while (choices[vals] == false)
                    {
                        vals = Random.Range(0, 3);
                    }

                    //If attack is chosen
                    if (vals == 0)
                    {
                        int x = 0;
                        //Randomly choose target
                        List<int> tochoos = new List<int>();
                        for (int f = 0; f < PartyMembers.Count; f++)
                        {
                            tochoos.Add(f);
                        }
                        int r = tochoos[Random.Range(0, tochoos.Count)];
                        while (PartyMembers[r] == null || PartyMembers[r].currentHP <= 0)
                        {
                            r = tochoos[Random.Range(0, tochoos.Count)];
                        }
                        //Add ability index (priority) number of times to list
                        List<int> probos = new List<int>();
                        for (int d = 0; d < EnemyMembers[i].abilities.Count; d++)
                        {
                            for (int c = 0; c < EnemyMembers[i].abilities[d].priority; c++)
                            {
                                probos.Add(d);
                            }
                        }
                        x = probos[Random.Range(0, probos.Count)];
                        //While ability type isn't offensive
                        while (EnemyMembers[i].abilities[x].type != 0)
                        {
                            x = probos[Random.Range(0, probos.Count)];
                        }

                        //Set speed to modified enemy agility
                        int speed = EnemyMembers[i].spd;
                        actionTag now = new actionTag(i, "enemyAttack", x, r, speed);
                        actions.Add(now);
                    }
                    //If support ability is chosen (user is not the target)
                    else if (vals == 1)
                    {
                        int x = 0;

                        List<int> probos = new List<int>();
                        for (int d = 0; d < EnemyMembers[i].abilities.Count; d++)
                        {
                            for (int c = 0; c < EnemyMembers[i].abilities[d].priority; c++)
                            {
                                probos.Add(d);
                            }
                        }
                        while (EnemyMembers[i].abilities[x].type != 1)
                        {
                            x = probos[Random.Range(0, probos.Count)];
                        }

                        int r = Random.Range(0, EnemyMembers.Count);
                        bool selfie = true;
                        if (EnemyMembers[i].unitName.Equals(EnemyMembers[r].unitName) && i == r)
                        {
                            selfie = false;
                        }
                        while (EnemyMembers[r].currentHP <= 0 || !selfie)
                        {
                            r = Random.Range(0, EnemyMembers.Count);
                            if (!EnemyMembers[i].unitName.Equals(EnemyMembers[r].unitName) && i != r)
                            {
                                selfie = true;
                            }
                        }

                        int speed = EnemyMembers[i].spd;
                        actionTag now = new actionTag(i, "enemyAction", x, r, speed);
                        actions.Add(now);
                    }
                    //If self-buff ability is chosen
                    else if (vals == 2)
                    {
                        int x = 0;
                        List<int> probos = new List<int>();
                        for (int d = 0; d < EnemyMembers[i].abilities.Count; d++)
                        {
                            for (int c = 0; c < EnemyMembers[i].abilities[d].priority; c++)
                            {
                                probos.Add(d);
                            }
                        }
                        while (EnemyMembers[i].abilities[x].type != 2)
                        {
                            x = probos[Random.Range(0, probos.Count)];
                        }
                        int speed = EnemyMembers[i].spd;
                        actionTag now = new actionTag(i, "enemyAbility", x, i, speed);
                        actions.Add(now);
                    }
                }
            }
        }
    }

    //Setup stats/game objects for battle
    IEnumerator setupBattle()
    {
        sv = SaveManager.Load();
        //Add placeholders
        currentUnit = 0;
        for (int i = 0; i < 3; i++) PartyMembers.Add(null);
        for (int i = 0; i < 3; i++) EnemyMembers.Add(null);

        //Load in from json file or other source. Until available, load in prefabs
        PartyMembers[0] = new Pixal();
        Debug.Log("H&M - " + sv.helperGot + sv.mamaGot);
        if (sv.mamaGot)
        {
            PartyMembers[1] = new Mama();
        }
        else
        {
            PartyMembers[1] = null;
        }
        if (sv.helperGot)
        {
            PartyMembers[2] = new Helper();
        }
        else
        {
            PartyMembers[2] = null;
        }

        //Set up party unit visuals
        for (int i = 0; i < 3; i++)
        {
            if (PartyMembers[i] != null)
            {
                PartyMembers[i].enemy = false;
                if (PartyMembers[i].sprites[0] != null)
                {
                    partyPrefabs[i].GetComponent<SpriteRenderer>().sprite = PartyMembers[i].sprites[0];
                    partyIcons[i].gameObject.SetActive(true);
                    partyIcons[i].sprite = PartyMembers[i].sprites[0];
                }
                else
                {
                    //partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
                }
                PartyMembers[i].currentHP = PartyMembers[i].maxHP;
                PartyMembers[i].currentStamina = PartyMembers[i].maxStamina;
                Debug.Log("Party " + i + " HP == " + PartyMembers[i].currentHP + ", MP == " + PartyMembers[i].currentStamina);
                partyIcons[i].transform.Find("HPBar").GetChild(1).GetComponent<Image>().fillAmount = PartyMembers[i].currentHP / PartyMembers[i].maxHP;
                partyIcons[i].transform.Find("MPBar").GetChild(1).GetComponent<Image>().fillAmount = PartyMembers[i].currentStamina / PartyMembers[i].maxStamina;
                activeUnits += 1;
            }
            else
            {
                partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                partyPrefabs[i].SetActive(false);
                partyIcons[i].gameObject.SetActive(false);
            }
        }

        //Get random enemies
        if (!sv.bossFight)
        {
            for (int i = 0; i < sv.fightLevel + 1; i++)
            {
                int randy = Random.Range(0, 4);
                switch (randy)
                {
                    case 0:
                        EnemyMembers[i] = new Slime();
                        break;
                    case 1:
                        EnemyMembers[i] = new Wizard();
                        break;
                    case 2:
                        EnemyMembers[i] = new Soldier();
                        break;
                    case 3:
                        EnemyMembers[i] = new Bear();
                        break;
                }
                activeEnemies += 1;
            }
        }
        else
        {
            EnemyMembers[0] = new Slime();
            EnemyMembers[1] = new BossSlime();
            EnemyMembers[2] = new Slime();
            activeEnemies = 3;
        }
        

        //Set up enemy unit visuals
        for (int i = 0; i < 3; i++)
        {
            if (EnemyMembers[i] != null)
            {
                EnemyMembers[i].enemy = true;
                if (EnemyMembers[i].sprites[0] != null)
                {
                    enemyPrefabs[i].GetComponent<SpriteRenderer>().sprite = EnemyMembers[i].sprites[0];
                    enemyIcons[i].gameObject.SetActive(true);
                    enemyIcons[i].sprite = EnemyMembers[i].sprites[0];
                }
                else
                {
                   // enemyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
                }
                EnemyMembers[i].currentHP = EnemyMembers[i].maxHP;
                EnemyMembers[i].currentStamina = EnemyMembers[i].maxStamina;
                enemyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale =
                    new Vector3(1.0f * EnemyMembers[i].currentHP / EnemyMembers[i].maxHP,
                    enemyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                activeEnemies += 1;
            }
            else
            {
                enemyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                enemyPrefabs[i].SetActive(false);
                enemyIcons[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(fadeIn());
        yield return new WaitForSeconds(0.5f);
        //Change music to play depending on enemies being fought (will check for boss names later)
        if (!bossBattle)
        {
            music = Resources.Load<AudioClip>("Audio/Music/Fighting is not an option");
        }
        else
        {
            music = Resources.Load<AudioClip>("Audio/Music/Battle Theme");
        }
        musicSource.clip = music;
        musicSource.Play();

        actions = new List<actionTag>();

        if (activeEnemies == 1)
        {
            yield return textDisplay("A " + EnemyMembers[0].unitName + " has appeared.");
        }
        else if (activeEnemies >= 2)
        {
            yield return textDisplay("A group of enemies appeared");
        }
        yield return new WaitForSeconds(1.0f);
        currentUnit = 0;

        state = battleState.PLAYER;
        playerTurn();

        yield return new WaitForSeconds(1.0f);
    }

    //public function for clearing the text of the textbox
    public void Clear()
    {
        DisplayText.text = "";
    }

    //Display text
    IEnumerator textDisplay(string tt, bool stop = false)
    {
        ender = stop;
        if (!stop && state != battleState.START)
        {
            scroll_speed = 40;
        }
        else
        {
            scroll_speed = 40;
        }
        //StopCoroutine("textDisplay");
        Clear();
        write_queue.Add(tt);
        writing = true;
        for (int i = 0; i < write_queue[0].Length && writing; i++)
        {
            if (inputMan.inputSubmit_D && stop)
            {
                Debug.Log("Should stop now");
                Clear();
                DisplayText.text = tt;
                write_queue.RemoveAt(0);
                writing = false;
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                break;
            }
            if (writing)
            {
                yield return new WaitForSeconds(1f / scroll_speed);
                DisplayText.text += write_queue[0][i];
            }
        }
        writing = false;
        if (write_queue.Count >= 1)
        {
            Debug.Log("write queue count == " + write_queue.Count);
            write_queue.RemoveAt(0);
        }
        DisplayText.text = tt;
        if (stop)
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(new System.Func<bool>(() => inputMan.inputSubmit != 0.0f));
        }
    }

    //Perform the selected actions, after they have been selected
    public IEnumerator PerformActions()
    {
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
        enemyAttacks();
        if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE && EnemyMembers.Count - enemyDeaths > 0 && activeUnits - partyDeaths > 0)
        {
            actions.Sort((a, b) => { return b.getSPD().CompareTo(a.getSPD()); });
            actions.Sort((a, b) => { return b.getFast().CompareTo(a.getFast()); });
            for (int z = 0; z < actions.Count; z++)
            {
                string sc = actions[z].getType();
                yield return new WaitForSeconds(0.5f);
                int ind = actions[z].getID();

                //Check if player should take damage from a status effect
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack"
                    || sc == "Flee" || sc == "revive")
                {
                    if (PartyMembers[ind].currentHP <= 0)
                    {
                        continue;
                    }
                    bool newd = false;
                    //Check for poison
                    if (PartyMembers[ind].statuses[0] != -1)
                    {
                        newd = PartyMembers[ind].takeDamage(4);
                        StartCoroutine(flash(ind, false, 0));
                        yield return textDisplay(PartyMembers[ind].unitName + " took damage from Poison.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        yield return textDisplay(PartyMembers[ind].unitName + " was taken out by the Poison.", true);
                        StartCoroutine(unitDeath(PartyMembers[ind]));
                        partyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }
                }
                //Check if an enemy should take damage from a status effect
                else
                {
                    if (EnemyMembers[ind] != null)
                    {
                        if (EnemyMembers[ind].currentHP <= 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    bool newd = false;
                    //Check for vomiting
                    if (EnemyMembers[ind].statuses[0] != -1)
                    {
                        newd = EnemyMembers[ind].takeDamage(4);
                        StartCoroutine(flash(ind, true, 0));
                        yield return textDisplay(EnemyMembers[ind].unitName + " took damage from Poison", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        yield return textDisplay(EnemyMembers[ind].unitName + " was taken out by the Poison", true);
                        StartCoroutine(unitDeath(EnemyMembers[ind]));
                        enemyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (enemyDeaths >= activeEnemies)
                        {
                            state = battleState.WIN;
                            yield return battleEnd();
                        }
                        continue;
                    }
                }

                //Check if the player is stopped by a status
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack"
                    || sc == "Flee" || sc == "revive")
                {
                    if (PartyMembers[ind].statuses[1] != -1)
                    {
                        int rando = Random.Range(0, 5);
                        if (rando > 2)
                            yield return textDisplay(PartyMembers[ind].unitName + " is Paralyzed");
                        //yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        continue;
                    }
                }
                //Same for the enemy
                else
                {
                    if (EnemyMembers[ind].statuses[1] != -1)
                    {
                        int rando = Random.Range(0, 5);
                        if (rando > 2)
                            yield return textDisplay(EnemyMembers[ind].unitName + " is Paralyzed");
                        //yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        continue;
                    }
                }

                Debug.Log("Current action == " + actions[z].getType());
                Debug.Log("Index == " + actions[z].getID());

                //Use offensive ability
                if (actions[z].getType().Equals("Action") && state == battleState.ATTACK)
                {
                    if (PartyMembers[ind].currentHP > 0)
                    {
                        int toget = actions[z].getTarget();
                        if (EnemyMembers[toget].currentHP <= 0)
                        {
                            while (EnemyMembers[toget].currentHP <= 0 && toget > 0)
                            {
                                toget--;
                            }
                            if (toget == 0 && EnemyMembers[toget].currentHP <= 0)
                            {
                                while (EnemyMembers[toget].currentHP <= 0 && toget < EnemyMembers.Count - 1)
                                {
                                    toget++;
                                }
                            }
                            if (EnemyMembers[toget].currentHP <= 0)
                            {
                                state = battleState.WIN;
                                yield return battleEnd();
                            }
                        }
                        else
                        {

                            string abiName = PartyMembers[ind].abilities[actions[z].getIndex()].actionName;


                            yield return textDisplay(PartyMembers[ind].unitName + " used " + abiName, true);
                            yield return playerAbility(actions[z].getIndex(), toget, PartyMembers[ind], EnemyMembers[toget]);
                            PartyMembers[ind].currentStamina -= PartyMembers[ind].abilities[actions[z].getIndex()].cost;
                            partyPrefabs[ind].transform.GetChild(1).localScale = new Vector3(1.0f * PartyMembers[ind].currentStamina / PartyMembers[ind].maxStamina,
                    partyPrefabs[ind].transform.GetChild(1).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                        }
                    }
                }
                //Use Buff/Support ability (player)
                else if (actions[z].getType().Equals("Support") && state == battleState.ATTACK)
                {
                    if (PartyMembers[ind].currentHP > 0)
                    {
                        int pose = actions[z].getTarget();
                        string abiName = PartyMembers[ind].abilities[actions[z].getIndex()].actionName;
                        if (PartyMembers[pose] != null)
                        {
                            if (PartyMembers[pose].currentHP > 0)
                            {
                                yield return textDisplay(PartyMembers[ind].unitName + " used " + abiName);
                                yield return playerAbility(actions[z].getIndex(), pose, PartyMembers[ind], PartyMembers[pose]);
                                PartyMembers[ind].currentStamina -= PartyMembers[ind].abilities[actions[z].getIndex()].cost;
                            }
                            else
                            {
                                StartCoroutine(textDisplay(PartyMembers[ind].unitName + " used " + abiName + ", but they were too late"));
                            }
                        }
                        else
                        {
                            StartCoroutine(textDisplay(PartyMembers[ind].unitName + " used " + abiName + ", but nobody was there"));
                        }
                    }
                }
                //Use basic attack
                else if (actions[z].getType().Equals("Attack") && state == battleState.ATTACK)
                {
                    if (PartyMembers[ind].currentHP > 0)
                    {
                        int toget = actions[z].getTarget();

                        if (EnemyMembers[toget].currentHP <= 0)
                        {
                            while (EnemyMembers[toget].currentHP <= 0 && toget > 0)
                            {
                                toget--;
                            }
                            if (toget == 0 && EnemyMembers[toget].currentHP <= 0)
                            {
                                while (EnemyMembers[toget].currentHP <= 0 && toget < EnemyMembers.Count - 1)
                                {
                                    toget++;
                                }
                            }
                            if (EnemyMembers[toget].currentHP <= 0)
                            {
                                state = battleState.WIN;
                                yield return battleEnd();
                            }
                        }

                        yield return textDisplay(PartyMembers[ind].unitName + " attacked the enemy", true);
                        yield return basicAttack(PartyMembers[ind], EnemyMembers[toget]);
                    }
                }
                //Have unit defend themselves
                else if (actions[z].getType().Equals("Defend") && state == battleState.ATTACK)
                {
                    if (PartyMembers[ind].currentHP > 0)
                    {
                        int toget = actions[z].getTarget();
                        PartyMembers[toget].defending = true;
                        partyPrefabs[toget].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1.0f);
                        yield return textDisplay(PartyMembers[ind].unitName + " defended themself", true);
                    }
                }
                //Have enemy use offensive ability
                else if (actions[z].getType().Equals("enemyAttack") && state == battleState.ATTACK)
                {
                    if (EnemyMembers[ind].currentHP > 0)
                    {
                        Debug.Log("Enemy Attacking");
                        //EnemyMembers[ind].changeSprite(1);
                        int toget = actions[z].getTarget();
                        if (PartyMembers[toget] != null)
                        {
                            Debug.Log("Party " + toget + " not null");
                            if (PartyMembers[toget].currentHP > 0)
                            {
                                Debug.Log("Party has HP");
                                yield return textDisplay(EnemyMembers[ind].unitName + " used " +
                                    EnemyMembers[ind].abilities[actions[z].getIndex()].actionName, true);
                                yield return enemyAttack(actions[z].getIndex(), toget, EnemyMembers[ind], PartyMembers[toget]);
                            }
                            //If dead unit at position
                            else
                            {
                                if (partyDeaths < activeUnits)
                                {
                                    int baseNum = toget;
                                    while (PartyMembers[toget] == null || PartyMembers[toget].currentHP <= 0 || toget == baseNum)
                                    {
                                        toget = Random.Range(0, PartyMembers.Count);
                                    }
                                    yield return textDisplay(EnemyMembers[ind].unitName + " used " +
                                    EnemyMembers[ind].abilities[actions[z].getIndex()].actionName);
                                    yield return enemyAttack(actions[z].getIndex(), toget, EnemyMembers[ind], PartyMembers[toget]);
                                }
                                else
                                {
                                    state = battleState.WIN;
                                    yield return battleEnd();
                                }
                            }
                        }
                        else
                        {
                            if (partyDeaths < activeUnits)
                            {
                                int baseNum = toget;
                                while (PartyMembers[toget] == null || PartyMembers[toget].currentHP <= 0 || toget == baseNum)
                                {
                                    toget = Random.Range(0, PartyMembers.Count);
                                }
                                yield return textDisplay(EnemyMembers[ind].unitName + " used " +
                                    EnemyMembers[ind].abilities[actions[z].getIndex()].actionName);
                                yield return enemyAttack(actions[z].getIndex(), toget,
                                    EnemyMembers[ind], PartyMembers[toget]);
                            }
                            else
                            {
                                state = battleState.WIN;
                                yield return battleEnd();
                            }
                        }
                        yield return new WaitForSeconds(0.5f);
                        //EnemyMembers[ind].changeSprite(0);
                    }
                }
                //Enemy performs a non-offensive ability
                else if (actions[z].getType().Equals("enemyAction") && state == battleState.ATTACK)
                {
                    if (EnemyMembers[ind].currentHP > 0)
                    {
                        //EnemyMembers[ind].changeSprite(1);
                        if (EnemyMembers[actions[z].getTarget()] != null)
                        {
                            if (EnemyMembers[actions[z].getTarget()].currentHP > 0)
                            {
                                yield return textDisplay(EnemyMembers[ind].unitName + " used " +
                                    EnemyMembers[ind].abilities[actions[z].getIndex()].actionName);
                                yield return enemyAbility(actions[z].getIndex(), actions[z].getTarget(),
                                    EnemyMembers[ind], EnemyMembers[actions[z].getTarget()]);
                            }
                            else
                            {
                                yield return textDisplay(EnemyMembers[ind].unitName + " tried supporting " +
                                    EnemyMembers[actions[z].getTarget()].unitName + ", but they weren't there");
                            }
                        }
                        else
                        {
                            yield return textDisplay(EnemyMembers[ind].unitName + " tried using ability," +
                                " but nobody was there");
                        }
                        yield return new WaitForSeconds(0.5f);
                        //EnemyMembers[ind].changeSprite(0);
                    }
                }
                else
                {
                    Debug.Log(actions[z].getType());
                    yield return textDisplay("Invalid action selected");
                }
                yield return new WaitForSeconds(0.5f);
                if (!skipper)
                {
                    yield return new WaitUntil(new System.Func<bool>(() => inputMan.inputSubmit != 0.0f));
                }
                else skipper = false;
                int tempPD = 0;
                int tempPA = 0;
                for (int i = 0; i < PartyMembers.Count; i++)
                {
                    if (PartyMembers[i] != null)
                    {
                        tempPA += 1;
                        if (PartyMembers[i].currentHP <= 0)
                        {
                            tempPD += 1;
                        }
                    }
                }
                int tempED = 0;
                for (int i = 0; i < EnemyMembers.Count; i++)
                {
                    if (EnemyMembers[i] != null)
                    {
                        if (EnemyMembers[i].currentHP <= 0)
                        {
                            tempED += 1;
                        }
                    }
                }
                if (partyDeaths >= activeUnits || tempPA == tempPD)
                {
                    state = battleState.LOSE;
                    yield return battleEnd();
                }
                else if (enemyDeaths >= EnemyMembers.Count || tempED >= activeEnemies)
                {
                    state = battleState.WIN;
                    yield return battleEnd();
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (PartyMembers[i] != null)
                {
                    Debug.Log("Party - " + PartyMembers[i].unitName + " HP == " + PartyMembers[i].currentHP);
                    if (PartyMembers[i].currentHP > 0)
                    {
                        PartyMembers[i].defending = false;
                        PartyMembers[i].statusTurn();
                        if (PartyMembers[i].statuses[0] <= 0)
                        {
                            partyIcons[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                        if (PartyMembers[i].statuses[1] <= 0)
                        {
                            partyIcons[i].transform.GetChild(1).gameObject.SetActive(false);
                        }
                        if (PartyMembers[i].statuses[2] <= 0)
                        {
                            partyIcons[i].transform.GetChild(2).gameObject.SetActive(false);
                        }
                        if (PartyMembers[i].statuses[3] <= 0)
                        {
                            partyIcons[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                        }
                        partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (EnemyMembers[i] != null)
                {
                    if (EnemyMembers[i].currentHP > 0)
                    {
                        EnemyMembers[i].statusTurn();
                    }
                }
            }
            actions.Clear();

            if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE && enemyDeaths < EnemyMembers.Count && partyDeaths < activeUnits)
            {
                yield return new WaitForSeconds(1.5f);
                state = battleState.PLAYER;
                currentUnit = 0;
                while (PartyMembers[currentUnit] == null || PartyMembers[currentUnit].currentHP <= 0) currentUnit++;
                playerTurn();
            }
            else
            {
                yield return battleEnd();
            }
        }
        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
        active_menu = 0;
    }


    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    //ata - the index of the ability
    //val - the index of the target(enemy if type 0)
    //uni - the user of the ability
    //target - the target of the ability
    IEnumerator playerAbility(int ata, int val, Unit uni, Unit target)
    {
        Debug.Log("val == " + val + ", target == " + target);
        bool crite = false;
        bool good = false;
        bool bad = false;

        yield return new WaitForSeconds(1f);
        int dami = uni.abilities[ata].damage * (uni.getAtk() / target.getDef());

        if (uni.abilities[ata].type == 0)
        {
            if (uni.abilities[ata].target == 0)
            {
                int crit = Random.Range(1, 101);
                if (crit <= (uni.lck / 4) + 3)
                {
                    dami += (dami / 2);
                    crite = true;
                }

                target.takeDamage(dami);
                StartCoroutine(flash(val, true, 0));
                enemyPrefabs[val].transform.GetChild(0).localScale = new Vector3(1.0f * EnemyMembers[val].currentHP / EnemyMembers[val].maxHP,
                    enemyPrefabs[val].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                if (crite)
                {
                    yield return textDisplay("It's a critical hit!", true);
                    skipper = true;
                }
                if (EnemyMembers[val].currentHP <= 0)
                {
                    enemyDeaths++;
                    yield return unitDeath(EnemyMembers[val]);                   
                    if (enemyDeaths >= EnemyMembers.Count)
                    {
                        state = battleState.WIN;
                        yield return battleEnd();
                    }
                }
                if (uni.abilities[ata].statusEffect != 0 && uni.abilities[ata].statusEffect != -1)
                {
                    if (target.statuses[uni.abilities[ata].statusEffect] == -1)
                    {
                        int rando = Random.Range(0, target.lck);
                        if (rando == 0)
                        {
                            target.statuses[uni.abilities[ata].statusEffect] = 3;
                            switch(uni.abilities[ata].statusEffect)
                            {
                                case 0:
                                    yield return textDisplay(target.unitName + "was inflicted with corruption", true);
                                    enemyIcons[val].transform.GetChild(0).gameObject.SetActive(true);
                                    break;
                                case 1:
                                    yield return textDisplay(target.unitName + "was inflicted with glitchy-ness", true);
                                    enemyIcons[val].transform.GetChild(1).gameObject.SetActive(true);
                                    break;
                                case 2:
                                    yield return textDisplay(target.unitName + "was inflicted with degradation", true);
                                    enemyIcons[val].transform.GetChild(2).gameObject.SetActive(true);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                { 
                    dami = uni.abilities[ata].damage * (uni.getAtk() / EnemyMembers[i].getDef());
                    int crit = Random.Range(1, 101);
                    if (crit <= (uni.lck / 4) + 3)
                    {
                        dami += (dami / 2);
                        crite = true;
                    }
                    else
                    {
                        crite = false;
                    }
                    EnemyMembers[i].takeDamage(dami);
                    StartCoroutine(flash(i, true, 0));
                    enemyPrefabs[i].transform.GetChild(0).localScale = new Vector3(1.0f * EnemyMembers[i].currentHP / EnemyMembers[i].maxHP,
                        enemyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                    if (crite)
                    {
                        yield return textDisplay("It's a critical hit!", true);
                        skipper = true;
                    }
                    if (EnemyMembers[i].currentHP <= 0)
                    {
                        enemyDeaths++;
                        yield return unitDeath(EnemyMembers[i]);
                        if (enemyDeaths >= EnemyMembers.Count)
                        {
                            state = battleState.WIN;
                            yield return battleEnd();
                        }
                    }
                    if (uni.abilities[ata].statusEffect != 0 && uni.abilities[ata].statusEffect != -1)
                    {
                        if (EnemyMembers[i].statuses[uni.abilities[ata].statusEffect] == -1)
                        {
                            int rando = Random.Range(0, EnemyMembers[i].lck);
                            if (rando == 0)
                            {
                                EnemyMembers[i].statuses[uni.abilities[ata].statusEffect] = 3;
                                switch (uni.abilities[ata].statusEffect)
                                {
                                    case 0:
                                        enemyIcons[i].transform.GetChild(0).gameObject.SetActive(true);
                                        yield return textDisplay(EnemyMembers[i].unitName + "was inflicted with corruption", true);
                                        break;
                                    case 1:
                                        enemyIcons[i].transform.GetChild(1).gameObject.SetActive(true);
                                        yield return textDisplay(EnemyMembers[i].unitName + "was inflicted with glitchy-ness", true);
                                        break;
                                    case 2:
                                        enemyIcons[i].transform.GetChild(2).gameObject.SetActive(true);
                                        yield return textDisplay(EnemyMembers[i].unitName + "was inflicted with degradation", true);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (uni.abilities[ata].type == 1)
        {
            if (uni.abilities[ata].target == 0)
            {
                target.takeDamage(-uni.abilities[ata].damage);
                StartCoroutine(flash(val, false, 1));
                if (uni.abilities[ata].statusEffect != -1 && uni.abilities[ata].statusEffect != 0)
                {
                    target.statuses[uni.abilities[ata].statusEffect] = 3;
                    partyIcons[val].GetComponent<Image>().color = new Color(1.0f, 212f/255f, 0.0f);
                    yield return textDisplay(target.unitName + " was empowered by their teammate");
                }
                partyPrefabs[val].transform.GetChild(0).localScale = new Vector3(1.0f * PartyMembers[val].currentHP / PartyMembers[val].maxHP,
                partyPrefabs[val].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (PartyMembers[i] != null)
                    {
                        if (PartyMembers[i].currentHP > 0)
                        {
                            PartyMembers[i].takeDamage(-uni.abilities[ata].damage);
                            StartCoroutine(flash(i, false, 1));
                            
                            if (uni.abilities[ata].statusEffect != -1 && uni.abilities[ata].statusEffect != 0)
                            {
                                PartyMembers[i].statuses[uni.abilities[ata].statusEffect] = 3;
                                partyIcons[i].GetComponent<Image>().color = new Color(1.0f, 212f / 255f, 0.0f);
                                yield return textDisplay(PartyMembers[i].unitName + " was empowered by their teammate");
                            }
                            partyPrefabs[i].transform.GetChild(0).localScale = new Vector3(1.0f * PartyMembers[i].currentHP / PartyMembers[i].maxHP,
                                partyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.0f);
    }

    //Use a basic attack against the target
    IEnumerator basicAttack(Unit uni, Unit target)
    {
        //dialogue.text = "Player used " + ata.name;
        bool crite = false;
        bool good = false;
        bool bad = false;

        yield return new WaitForSeconds(1f);
        int val = 5 * (uni.getAtk() / target.getDef());
        //val = uni.takeDamageCalc(target, val, op);

        //Check if the unit gets a crit
        int crit = Random.Range(1, 101);
        if (crit <= (uni.lck / 4) + 3)
        {
            val += (val / 2);
            crite = true;
        }

        int tv = 0;
        while (target != EnemyMembers[tv] && tv < EnemyMembers.Count)
        {
            tv++;
        }
        float dif = target.currentHP;
        bool dead = target.takeDamage(val);
        StartCoroutine(flash(tv, true, 0));
        

        if (dif > 0)
        {
            //StartCoroutine(showDamage(dif, tv, op));
        }
        for (int i = 0; i < EnemyMembers.Count; i++)
        {
            if (EnemyMembers[i].currentHP < 0) EnemyMembers[i].currentHP = 0;
            enemyPrefabs[i].transform.GetChild(0).localScale = new Vector3(1.0f * EnemyMembers[i].currentHP / EnemyMembers[i].maxHP,
                enemyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
        }
        //uni.setSP(uni.currentSP - 2);

        if (crite)
        {
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            yield return textDisplay("It's a critical hit!", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            skipper = true;
        }
        if (good)
        {
            yield return textDisplay("It did a lot of damage!", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            skipper = true;
        }
        if (bad)
        {
            yield return textDisplay("It didn't do too much damage..", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            skipper = true;
        }

        //yield return new WaitForSeconds(0.5f);

        //If enemy is dead, battle is won
        if (dead)
        {
            enemyDeaths++;
            yield return unitDeath(target);
            //yield return levelUp(target.giveEXP());
            if (enemyDeaths >= EnemyMembers.Count)
            {
                state = battleState.WIN;
                yield return battleEnd();
            }
        }
    }

    //An enemy attack function, used with enemies that have a list of abilities
    //ata - index of attack
    //val - index of enemy (target)
    //uni - enemy using attack
    //target - target of attack
    IEnumerator enemyAttack(int ata, int val, Unit uni, Unit target)
    {
        Debug.Log("val == " + val + ", target == " + target);
        bool crite = false;
        bool good = false;
        bool bad = false;

        yield return new WaitForSeconds(1f);
        int dami = uni.abilities[ata].damage * (uni.getAtk() / target.getDef());


        if (uni.abilities[ata].target == 0)
        {
            int crit = Random.Range(1, 101);
            if (crit <= (uni.lck / 4) + 3)
            {
                dami += (dami / 2);
                crite = true;
            }

            bool dead = target.takeDamage(dami);
            StartCoroutine(flash(val, false, 0));
            partyPrefabs[val].transform.GetChild(0).localScale = new Vector3(1.0f * PartyMembers[val].currentHP / PartyMembers[val].maxHP,
                partyPrefabs[val].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
            if (crite)
            {
                yield return textDisplay("It's a critical hit!", true);
                skipper = true;
            }
            if (dead)
            {
                partyDeaths++;
                yield return unitDeath(target);
                //yield return levelUp(target.giveEXP());
                if (partyDeaths >= PartyMembers.Count)
                {
                    state = battleState.LOSE;
                    yield return battleEnd();
                }
            }
            if (uni.abilities[ata].statusEffect != 0 && uni.abilities[ata].statusEffect != -1)
            {
                if (target.statuses[uni.abilities[ata].statusEffect] == -1)
                {
                    int rando = Random.Range(0, target.lck);
                    if (rando == 0)
                    {
                        target.statuses[uni.abilities[ata].statusEffect] = 3;
                        switch (uni.abilities[ata].statusEffect)
                        {
                            case 0:
                                partyIcons[val].transform.GetChild(0).gameObject.SetActive(true);
                                yield return textDisplay(target.unitName + "was inflicted with corruption", true);
                                break;
                            case 1:
                                partyIcons[val].transform.GetChild(1).gameObject.SetActive(true);
                                yield return textDisplay(target.unitName + "was inflicted with glitchy-ness", true);
                                break;
                            case 2:
                                partyIcons[val].transform.GetChild(2).gameObject.SetActive(true);
                                yield return textDisplay(target.unitName + "was inflicted with degradation", true);
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (PartyMembers[i] != null)
                {
                    if (PartyMembers[i].currentHP > 0)
                    {
                        dami = uni.abilities[ata].damage * (uni.getAtk() / PartyMembers[i].getDef());
                        int crit = Random.Range(1, 101);
                        if (crit <= (uni.lck / 4) + 3)
                        {
                            dami += (dami / 2);
                            crite = true;
                        }
                        else
                        {
                            crite = false;
                        }
                        bool dead = PartyMembers[i].takeDamage(dami);
                        StartCoroutine(flash(i, false, 0));
                        partyPrefabs[i].transform.GetChild(0).localScale = new Vector3(1.0f * PartyMembers[i].currentHP / PartyMembers[i].maxHP,
                            partyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                        if (crite)
                        {
                            yield return textDisplay("It's a critical hit!", true);
                            skipper = true;
                        }
                        if (dead)
                        {
                            partyDeaths++;
                            yield return unitDeath(PartyMembers[i]);
                            //yield return levelUp(target.giveEXP());
                            if (partyDeaths >= PartyMembers.Count)
                            {
                                state = battleState.LOSE;
                                yield return battleEnd();
                            }
                        }
                        if (uni.abilities[ata].statusEffect != 0 && uni.abilities[ata].statusEffect != -1)
                        {
                            if (PartyMembers[i].statuses[uni.abilities[ata].statusEffect] == -1)
                            {
                                int rando = Random.Range(0, PartyMembers[i].lck);
                                if (rando == 0)
                                {
                                    PartyMembers[i].statuses[uni.abilities[ata].statusEffect] = 3;
                                    switch (uni.abilities[ata].statusEffect)
                                    {
                                        case 0:
                                            partyIcons[i].transform.GetChild(0).gameObject.SetActive(true);
                                            yield return textDisplay(PartyMembers[i].unitName + "was inflicted with corruption", true);
                                            break;
                                        case 1:
                                            partyIcons[i].transform.GetChild(1).gameObject.SetActive(true);
                                            yield return textDisplay(PartyMembers[i].unitName + "was inflicted with glitchy-ness", true);
                                            break;
                                        case 2:
                                            partyIcons[i].transform.GetChild(2).gameObject.SetActive(true);
                                            yield return textDisplay(PartyMembers[i].unitName + "was inflicted with degradation", true);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0);
    }

    //An enemy uses a non-offensive ability
    IEnumerator enemyAbility(int ata, int val, Unit uni, Unit target)
    {
        Debug.Log("val == " + val + ", target == " + target);
        if (uni.abilities[ata].target == 0)
        {
            target.takeDamage(-uni.abilities[ata].damage);
            if (uni.abilities[ata].statusEffect != 0 && uni.abilities[ata].statusEffect != -1)
            {
                target.statuses[uni.abilities[ata].statusEffect] = 3;
                enemyIcons[val].GetComponent<Image>().color = new Color(1.0f, 212f / 255f, 0.0f); ;
                yield return textDisplay(target.unitName + " was empowered by their teammate");
            }
            StartCoroutine(flash(val, true, 1));
            enemyPrefabs[val].transform.GetChild(0).localScale = new Vector3(1.0f * EnemyMembers[val].currentHP / EnemyMembers[val].maxHP,
                enemyPrefabs[val].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (EnemyMembers[i] != null)
                {
                    if (EnemyMembers[i].currentHP > 0)
                    {
                        EnemyMembers[i].takeDamage(-uni.abilities[ata].damage);
                        if (uni.abilities[ata].statusEffect != 0 && uni.abilities[ata].statusEffect != -1)
                        {
                            EnemyMembers[i].statuses[uni.abilities[ata].statusEffect] = 3;
                            enemyIcons[i].GetComponent<Image>().color = new Color(1.0f, 212f / 255f, 0.0f); ;
                            yield return textDisplay(EnemyMembers[i].unitName + " was empowered by their teammate");
                        }
                        StartCoroutine(flash(i, true, 1));
                        enemyPrefabs[i].transform.GetChild(0).localScale = new Vector3(1.0f * EnemyMembers[i].currentHP / EnemyMembers[i].maxHP,
                            enemyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale.y, 0.0f);
                    }
                }
            }
        }
        yield return new WaitForSeconds(0);
    }

    //Fade out a unit from the screen when they die
    IEnumerator unitDeath(Unit bot)
    {
        StartCoroutine(textDisplay(bot.unitName + " has been defeated"));
        yield return new WaitForSeconds(1f);

        playSound(2);

        int ind = 0;

        if (bot.enemy)
        {
            for (int i = 0; i < 3; i++)
            {
                if (bot == EnemyMembers[i])
                {
                    ind = i;
                }
            }
            enemyPrefabs[ind].SetActive(false);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (bot == PartyMembers[i])
                {
                    ind = i;
                }
            }
            partyPrefabs[ind].SetActive(false);
        }
        
        /*
        if (bot.enemy)
        {
            if (bot.spBar != null)
            {
                bot.spBar.CrossFadeAlpha(0, 2f, false);
                bot.spSideText.CrossFadeAlpha(0, 2f, false);
                bot.spReadOut.CrossFadeAlpha(0, 2f, false);
                bot.sanBar.CrossFadeAlpha(0, 2f, false);
            }
        }
        else
        {
            bot.setHUD();
            Color dede = bot.deadIcon.color;
            dede.a = 1.0f;
            bot.view.CrossFadeAlpha(0.4f, 2f, false);
            bot.nameText.CrossFadeAlpha(0.4f, 2f, false);
            bot.BBackground.CrossFadeAlpha(0.4f, 2f, false);
            bot.WBackground.CrossFadeAlpha(0.4f, 2f, false);
            bot.levelText.CrossFadeAlpha(0.4f, 2f, false);
            bot.hpBar.CrossFadeAlpha(0.4f, 2f, false);
            bot.hpSideText.CrossFadeAlpha(0.4f, 2f, false);
            bot.hpReadOut.CrossFadeAlpha(0.4f, 2f, false);
            if (bot.spBar != null)
            {
                bot.spBar.CrossFadeAlpha(0.4f, 2f, false);
                bot.spSideText.CrossFadeAlpha(0.4f, 2f, false);
                bot.spReadOut.CrossFadeAlpha(0.4f, 2f, false);
                bot.sanBar.CrossFadeAlpha(0.4f, 2f, false);
            }
        }
        */

        yield return new WaitUntil(new System.Func<bool>(() => inputMan.inputSubmit_D));
    }

    //Fade into the battle scene (from black to screen)
    IEnumerator fadeIn()
    {
        transform.GetChild(0).Find("Fader").gameObject.SetActive(true);
        Color ori = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        transform.GetChild(0).Find("Fader").GetComponent<Image>().color = ori;
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).Find("Fader").GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
        yield return new WaitForSeconds(1.0f);

    }

    //Fade out of the battle scene (from scene to black)
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color ori = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        //transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(0).Find("Fader").GetComponent<Image>().CrossFadeAlpha(1, 2f, false);
    }

    //Cause unit to flash a color (ver: 0 == damage/red, 1 == healing/green, 2 == paralyzed)
    IEnumerator flash(int index, bool enemy, int ver = 0)
    {
        Debug.Log("Index for flash == " + index);
        Color ori;
        GameObject uni;
        if (enemy)
        {
            uni = enemyPrefabs[index];
            ori = enemyPrefabs[index].GetComponent<SpriteRenderer>().color;
        }
        else
        {
            uni = partyPrefabs[index];
            ori = partyPrefabs[index].GetComponent<SpriteRenderer>().color;
        }
        switch (ver)
        {
            case 0:
                yield return new WaitForSeconds(0.5f);
                uni.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.5f);
                if (!enemy)
                {
                    partyIcons[index].transform.Find("HPBar").GetChild(1).GetComponent<Image>().fillAmount = PartyMembers[index].currentHP / PartyMembers[index].maxHP;
                }
                else
                {
                    enemyIcons[index].transform.Find("HPBar").GetChild(1).GetComponent<Image>().fillAmount = EnemyMembers[index].currentHP / EnemyMembers[index].maxHP;
                }
                playSound(0);
                yield return new WaitForSeconds(0.5f);
                break;

            case 1:
                yield return new WaitForSeconds(0.5f);
                uni.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1.0f, 0.5f);
                playSound(1);
                yield return new WaitForSeconds(0.5f);
                break;

            case 2:
                yield return new WaitForSeconds(0.5f);
                uni.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f);
                playSound(1);
                yield return new WaitForSeconds(0.5f);
                break;

        }
        uni.GetComponent<SpriteRenderer>().color = ori;
        yield return new WaitForSeconds(0.0f);
    }

    //Display relevant text based on who wins the battle
    IEnumerator battleEnd()
    {
        StopCoroutine("performActions");
        StopCoroutine("playerAttack");
        StopCoroutine("basicAttack");
        StopCoroutine("enemyAttack");

        //If win, display text and give money (and rewards after rolling chances)
        if (state == battleState.WIN)
        {
            musicSource.Stop();
            musicSource.clip = Resources.Load<AudioClip>("Audio/Music/Victory");
            musicSource.loop = false;
            musicSource.Play();
            if (EnemyMembers.Count == 1)
            {
                yield return textDisplay("The " + EnemyMembers[0].unitName + " has been defeated", true);
            }
            else if (EnemyMembers.Count > 1)
            {
                yield return textDisplay("The group of enemies have been defeated", true);
            }
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            int avg = 0;
            int num = 10;
            int mone = 0;
            avg = avg / num;
            for (int i = 0; i < EnemyMembers.Count; i++)
            {
                mone += EnemyMembers[i].capital;
            }
            if (mone > 0)
            {
                yield return textDisplay("Received $" + mone + " buckaroos", true);
                //data.SetMoney(data.GetMoney() + mone);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            }
        }
        else if (state == battleState.LOSE)
        {
            musicSource.Stop();
            musicSource.clip = Resources.Load<AudioClip>("Audio/Music/Retro_No hope");
            musicSource.loop = false;
            musicSource.Play();
            yield return textDisplay("You Died", true);
            for (int i = 0; i < 4; i++)
            {
                if (PartyMembers[i] != null)
                {
                    if (PartyMembers[i].currentHP <= 0)
                    {
                        switch (PartyMembers[i].unitName)
                        {
                            /*
                            case "Pixal":
                                loader.dead[0] = true;
                                break;
                            case "Mama":
                                loader.dead[1] = true;
                                break;
                                */
                            //From previous JSON loader, to check what allies were dead
                        }
                    }
                }
            }
        }
        else if (state == battleState.FLEE)
        {
            yield return textDisplay("The party managed to escape", true);
        }
        else if (state == battleState.HUH)
        {
            yield return textDisplay("Nothing really happened", true);
        }
      
        //loader.money = data.GetMoney();
        //loader.Save(PlayerPrefs.GetInt("_active_save_file_"));
        //yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        yield return fadeOut();
        StartCoroutine(NextScene());
    }

    //Transfer to the next scene (most likely overworld or loading/transition screen
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2f);
        if (state != battleState.LOSE)
        {
            SceneManager.LoadScene("RPGWorld");
        }
        else
        {
            SceneManager.LoadScene("HubWorld");
        }
    }

    //Borrows code from menu manager script
    private void NavUpdate()
    {
        int navDiff = (inputMan.inputY > 0) ? -1 : 0 + ((inputMan.inputY < 0) ? 1 : 0);
        if (state == battleState.PLAYER)
        {
            if (!menu_input)
            { 
                //Base
                if (active_menu == 0)
                {
                    mainIndex += ((navDiff < 0 && mainIndex > 0) || (navDiff > 0 && mainIndex < mainButtons.Count - 1)) ? navDiff : 0;
                    mainButtons[mainIndex].Select();
                    menu_input = true;
                }
                //Action
                else if (active_menu == 1)
                {
                    actionIndex += ((navDiff < 0 && actionIndex > 0) || (navDiff > 0 && actionIndex < actionButtons.Count - 1 - (4 - PartyMembers[currentUnit].abilities.Count))) ? navDiff : 0;
                    actionButtons[actionIndex].Select();
                    menu_input = true;
                }
                //Target Enemy
                else if (active_menu == 2)
                {
                    enemyIndex += ((navDiff < 0 && enemyIndex > 0) || (navDiff > 0 && enemyIndex < enemyButtons.Count - 1)) ? navDiff : 0;
                    enemyButtons[enemyIndex].Select();
                    menu_input = true;
                }
                //Target Ally
                else if (active_menu == 3)
                {
                    allyIndex += ((navDiff < 0 && allyIndex > 0) || (navDiff > 0 && allyIndex < allyButtons.Count - 1)) ? navDiff : 0;
                    allyButtons[allyIndex].Select();
                    menu_input = true;
                }
            }
            menu_input = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        inputMan = GetComponent<InputManager>();

        menus = new List<GameObject>();
        for (int i = 3; i < transform.GetChild(0).childCount-1; i++)
        {
            menus.Add(transform.GetChild(0).GetChild(i).gameObject);
        }

        //StartCoroutine(fadeIn());
        menu_input = false;

        PartyMembers = new List<Unit>();
        EnemyMembers = new List<Unit>();

        dialogue = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        write_queue = new List<string>();
        scroll_speed = 20;

        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == battleState.PLAYER)
        {
            if (inputMan.inputY_D)
            {
                NavUpdate();
                seSource.clip = Resources.Load<AudioClip>("Audio/Sound Effects/vgmenuhighlight");
                seSource.Play();
            }
            else if (inputMan.inputSubmit_D)
            {
                seSource.clip = Resources.Load<AudioClip>("Audio/Sound Effects/Completetask_0");
                seSource.Play();
            }
        }

    }
}
