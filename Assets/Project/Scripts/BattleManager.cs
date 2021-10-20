using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //Int to track how many abilities away from the bottom before the menu can start scrolling
    private int ability_offset;
    //Int to track how many actions away from the bottom before the menu can start scrolling
    private int action_offset;

    //Current ability being highlighted
    private int highlighted_ability;
    //Current action in base menu (index) being highlighed by cursor
    private int highlighted_action;
    //Bool to check whether the menu is accepting input
    private bool menu_input;

    //Bool to check whether the player has the ability menu open
    private bool ability_select_menu;
    //Bool to check whether the player is selecting an enemy to attack
    private bool enemy_select_menu;
    //Bool to check whether the player is selecting an ally for an ability
    private bool unit_select_menu;

    public GameObject background;

    //GameObjects to use as basis for battle characters
    public List<GameObject> partyPrefabs;

    public List<GameObject> enemyPrefabs;

    private List<actionTag> actions;


    //Int to track the number of units actually in the party
    int activeUnits = 0;

    //Int to track the number of deaths in the party
    int partyDeaths = 0;

    //Int to track the number of enemies encountered in the battle
    int activeEnemies = 0;

    //Number of enemies that have died
    int enemyDeaths = 0;

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

    public void playerTurn()
    {
        DisplayText.text = PartyMembers[currentUnit] + "'s turn";
    }

    //Function to scroll through main menu in battle to choose what to do
    public void BaseMenuRoutine()
    {

    }
    //Function to scroll through possible abilites/actions
    public void ActionMenuRoutine()
    {

    }
    //Function to try and recruit an enemy as a party member
    public void RecruitMenuRoutine()
    {

    }
    //Function to enter the target/action for an attack
    public void takeAction(int target)
    {
        if (!currentActionType.Equals(""))
        {
            actions.Add(new actionTag(currentUnit, currentActionType, currentAction, target, PartyMembers[currentUnit].spd));
        }
        else
        {
            actions.Add(new actionTag(currentUnit, "Attack", currentAction, target, PartyMembers[currentUnit].spd));
        }
        currentUnit += 1;
        currentActionType = "";
        currentAction = 0;
        
        if (currentUnit >= 3)
        {
            //Calculate enemy actions, then go to perform actions
            StartCoroutine(PerformActions());
        }
        else
        {
            playerTurn();
        }
    }

    public void startAction(int type)
    {
        if (type == 0)
        {

        }
    }

    public void makeMenuVisible(int num)
    {
        if (num == 0)
        {
            menus[1].SetActive(true);
            menus[2].SetActive(false);
            menus[3].SetActive(false);
        }
        else if (num == 1)
        {
            menus[1].SetActive(false);
            menus[2].SetActive(true);
            menus[3].SetActive(false);
        }
        else if (num == 2)
        {
            menus[1].SetActive(false);
            menus[2].SetActive(false);
            menus[3].SetActive(true);
        }
    }

    //Start the enemy attack routine
    public void enemyAttacks()
    {
        int other = 0;

        for (int i = 0; i < EnemyMembers.Count; i++)
        {
            if (EnemyMembers[i].currentHP > 0)
            {
                other += 1;
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
                            //If a frontline unit, add twice
                            if (f < 2)
                            {
                                tochoos.Add(f);
                            }
                        }
                        int r = tochoos[Random.Range(0, tochoos.Count)];
                        while (PartyMembers[r] == null)
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
                        actionTag now = new actionTag(i, "enemyAbility", x, r, speed);
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

    IEnumerator setupBattle()
    {
        currentUnit = 0;
        for (int i = 0; i < 3; i++) PartyMembers.Add(null);
        for (int i = 0; i < 3; i++) EnemyMembers.Add(null);
        //Load in from json file or other source. Until available, load in prefabs
        PartyMembers[0] = new Pixal();
        PartyMembers[1] = new Mama();

        for (int i = 0; i < 3; i++)
        {
            if (PartyMembers[i] != null)
            {
                if (PartyMembers[i].sprites[0] != null)
                {
                    partyPrefabs[i].GetComponent<SpriteRenderer>().sprite = PartyMembers[i].sprites[0];
                }
                else
                {
                    partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
                }
                partyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale =
                    new Vector3(0.5f * PartyMembers[i].currentHP / PartyMembers[i].maxHP, 0.2f, 0.0f);
                partyPrefabs[i].transform.GetChild(1).GetComponent<SpriteRenderer>().transform.localScale =
                    new Vector3(0.5f * PartyMembers[i].currentStamina / PartyMembers[i].maxStamina, 0.2f, 0.0f);
                activeUnits += 1;
            }
            else
            {
                partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
        }

        EnemyMembers[0] = new Slime();
        EnemyMembers[1] = new Skeleton();
        EnemyMembers[2] = new Hound();
        activeEnemies = 3;

        actions = new List<Action>();

        if (activeEnemies == 1)
        {
            DisplayText.text = "A " + EnemyMembers[0].unitName + " has appeared.";
        }
        else if (activeEnemies >= 2)
        {
            DisplayText.text = "A group of enemies appeared";
        }
        yield return new WaitForSeconds(1.0f);
        currentUnit = 0;

        state = battleState.PLAYER;
        playerTurn();

        yield return new WaitForSeconds(1.0f);
    }

    //Perform the selected actions, after they have been selected
    public IEnumerator PerformActions()
    {
        yield return new WaitForSeconds(0.0f);
    }

    //Fade into the battle scene (from black to screen)
    IEnumerator fadeIn()
    {
        Color ori = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(1).Find("Fader").GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
        yield return new WaitForSeconds(0.5f);

    }

    //Fade out of the battle scene (from scene to black)
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color ori = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        //transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(1).Find("Fader").GetComponent<Image>().CrossFadeAlpha(1, 2f, false);
    }


    // Start is called before the first frame update
    void Start()
    {
        menus = new List<GameObject>();
        Debug.Log("Children # == " + transform.childCount);
        for (int i = 2; i < transform.GetChild(0).childCount-1; i++)
        {
            Debug.Log("i == " + i + ", menu == " + transform.GetChild(0).GetChild(i).name);
            menus.Add(transform.GetChild(0).GetChild(i).gameObject);
        }

        //StartCoroutine(fadeIn());
        menu_input = false;

        PartyMembers = new List<Unit>();
        EnemyMembers = new List<Unit>();


        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
